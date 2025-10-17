# 📘 Руководство для команды: Использование API авторизации

Этот документ предназначен для членов команды, которые будут разрабатывать другие части приложения (главное меню, просмотр книг, админ-панель и т.д.).

## 🎯 Быстрый старт

### Проверка авторизации пользователя

```csharp
using CommandProject.Managers;

// В любой форме или классе можно проверить:
if (SessionManager.IsLoggedIn)
{
    // Пользователь авторизован
    var currentUser = SessionManager.CurrentUser;
    MessageBox.Show($"Привет, {currentUser.FullName}!");
}
else
{
    // Пользователь не авторизован
    // Перенаправить на форму входа
}
```

### Проверка прав администратора

```csharp
if (SessionManager.IsAdmin)
{
    // Показать административные функции
    btnAddBook.Visible = true;
    btnEditBook.Visible = true;
    btnDeleteBook.Visible = true;
}
else
{
    // Скрыть административные функции
    btnAddBook.Visible = false;
    btnEditBook.Visible = false;
    btnDeleteBook.Visible = false;
}
```

## 📋 Объект User - Доступные свойства

```csharp
User user = SessionManager.CurrentUser;

// Основные свойства
int userId = user.UserID;              // ID пользователя
string username = user.Username;        // Имя пользователя (логин)
string email = user.Email;              // Email
string fullName = user.FullName;        // Полное имя
int roleId = user.RoleID;              // ID роли (1 - Admin, 2 - User)
string roleName = user.RoleName;        // Название роли
DateTime createdAt = user.CreatedAt;    // Дата регистрации
DateTime? lastLogin = user.LastLogin;   // Последний вход (может быть null)
bool isActive = user.IsActive;          // Активен ли пользователь

// Вспомогательные свойства
bool isAdmin = user.IsAdmin;            // Является ли администратором
bool isRegularUser = user.IsRegularUser; // Является ли обычным пользователем
```

## 🔧 Примеры использования

### Пример 1: Главное меню с приветствием

```csharp
public partial class MainMenuForm : Form
{
    public MainMenuForm()
    {
        InitializeComponent();
        InitializeUI();
    }

    private void InitializeUI()
    {
        // Проверяем авторизацию
        if (!SessionManager.IsLoggedIn)
        {
            MessageBox.Show("Необходимо войти в систему");
            this.Close();
            return;
        }

        // Получаем текущего пользователя
        var user = SessionManager.CurrentUser;

        // Устанавливаем приветствие
        lblWelcome.Text = $"Добро пожаловать, {user.FullName}!";

        // Настраиваем UI в зависимости от роли
        SetupUIByRole();
    }

    private void SetupUIByRole()
    {
        if (SessionManager.IsAdmin)
        {
            // Администратор видит все
            menuItemAdmin.Visible = true;
            menuItemUserManagement.Visible = true;
            menuItemBookManagement.Visible = true;
        }
        else
        {
            // Обычный пользователь не видит админ-панель
            menuItemAdmin.Visible = false;
            menuItemUserManagement.Visible = false;
            menuItemBookManagement.Visible = false;
        }
    }

    private void btnLogout_Click(object sender, EventArgs e)
    {
        // Выход из системы
        SessionManager.Logout();
        
        // Показываем форму входа
        LoginForm loginForm = new LoginForm();
        loginForm.Show();
        
        // Закрываем текущую форму
        this.Close();
    }
}
```

### Пример 2: Добавление книги (только для админа)

```csharp
public partial class AddBookForm : Form
{
    private DatabaseHelper dbHelper;

    public AddBookForm()
    {
        InitializeComponent();
        dbHelper = new DatabaseHelper();

        // Проверяем права доступа
        if (!SessionManager.IsAdmin)
        {
            MessageBox.Show(
                "У вас нет прав для добавления книг",
                "Доступ запрещен",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            );
            this.Close();
            return;
        }
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
        // Используем ID текущего пользователя для отслеживания, кто добавил книгу
        int addedByUserId = SessionManager.CurrentUser.UserID;

        // SQL запрос для добавления книги
        string query = @"
            INSERT INTO Books 
            (Title, ISBN, Description, PublishedYear, Publisher, AddedByUserID) 
            VALUES 
            (@Title, @ISBN, @Description, @Year, @Publisher, @AddedBy)";

        // Ваш код для выполнения запроса...
    }
}
```

### Пример 3: История чтения (для авторизованных пользователей)

```csharp
public partial class ReadingHistoryForm : Form
{
    private DatabaseHelper dbHelper;

    public ReadingHistoryForm()
    {
        InitializeComponent();
        dbHelper = new DatabaseHelper();
        LoadReadingHistory();
    }

    private void LoadReadingHistory()
    {
        if (!SessionManager.IsLoggedIn)
        {
            MessageBox.Show("Необходимо войти в систему");
            return;
        }

        int userId = SessionManager.CurrentUser.UserID;

        // SQL запрос для получения истории чтения
        string query = @"
            SELECT 
                b.Title,
                b.CoverImagePath,
                rh.StartDate,
                rh.LastReadDate,
                rh.CurrentPage,
                rh.IsCompleted
            FROM ReadingHistory rh
            INNER JOIN Books b ON rh.BookID = b.BookID
            WHERE rh.UserID = @UserID
            ORDER BY rh.LastReadDate DESC";

        // Ваш код для выполнения запроса и отображения данных...
    }
}
```

### Пример 4: Добавление в избранное

```csharp
public void AddToFavorites(int bookId)
{
    if (!SessionManager.IsLoggedIn)
    {
        MessageBox.Show("Войдите в систему, чтобы добавить книгу в избранное");
        return;
    }

    int userId = SessionManager.CurrentUser.UserID;

    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        connection.Open();
        
        string query = @"
            INSERT INTO Favorites (UserID, BookID, AddedDate)
            VALUES (@UserID, @BookID, GETDATE())";

        using (SqlCommand command = new SqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@UserID", userId);
            command.Parameters.AddWithValue("@BookID", bookId);

            try
            {
                command.ExecuteNonQuery();
                MessageBox.Show("Книга добавлена в избранное!");
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627) // Duplicate key error
                {
                    MessageBox.Show("Эта книга уже в избранном");
                }
                else
                {
                    MessageBox.Show($"Ошибка: {ex.Message}");
                }
            }
        }
    }
}
```

### Пример 5: Подписка на события сессии

```csharp
public partial class MainForm : Form
{
    public MainForm()
    {
        InitializeComponent();

        // Подписываемся на события
        SessionManager.OnUserLoggedIn += SessionManager_OnUserLoggedIn;
        SessionManager.OnUserLoggedOut += SessionManager_OnUserLoggedOut;
    }

    private void SessionManager_OnUserLoggedIn(object sender, UserEventArgs e)
    {
        // Пользователь вошел в систему
        UpdateUI();
        lblStatus.Text = $"Пользователь {e.User.Username} вошел в систему";
    }

    private void SessionManager_OnUserLoggedOut(object sender, UserEventArgs e)
    {
        // Пользователь вышел из системы
        lblStatus.Text = "Вы вышли из системы";
        
        // Закрываем форму или перенаправляем на форму входа
        LoginForm loginForm = new LoginForm();
        loginForm.Show();
        this.Hide();
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        // Отписываемся от событий
        SessionManager.OnUserLoggedIn -= SessionManager_OnUserLoggedIn;
        SessionManager.OnUserLoggedOut -= SessionManager_OnUserLoggedOut;
        
        base.OnFormClosing(e);
    }
}
```

## 🗄️ Работа с базой данных

### Получение DatabaseHelper

```csharp
using CommandProject.Database;

DatabaseHelper dbHelper = new DatabaseHelper();
```

### Готовые SQL запросы для работы с книгами

#### Получение всех книг

```sql
SELECT 
    b.BookID,
    b.Title,
    b.ISBN,
    b.Description,
    b.PublishedYear,
    b.Publisher,
    b.PageCount,
    b.CoverImagePath,
    b.IsAvailable,
    STRING_AGG(CONCAT(a.FirstName, ' ', a.LastName), ', ') AS Authors,
    STRING_AGG(g.GenreName, ', ') AS Genres
FROM Books b
LEFT JOIN BookAuthors ba ON b.BookID = ba.BookID
LEFT JOIN Authors a ON ba.AuthorID = a.AuthorID
LEFT JOIN BookGenres bg ON b.BookID = bg.BookID
LEFT JOIN Genres g ON bg.GenreID = g.GenreID
WHERE b.IsAvailable = 1
GROUP BY b.BookID, b.Title, b.ISBN, b.Description, 
         b.PublishedYear, b.Publisher, b.PageCount, 
         b.CoverImagePath, b.IsAvailable
ORDER BY b.Title;
```

#### Поиск книг

```sql
SELECT 
    b.BookID,
    b.Title,
    b.Description,
    STRING_AGG(CONCAT(a.FirstName, ' ', a.LastName), ', ') AS Authors
FROM Books b
LEFT JOIN BookAuthors ba ON b.BookID = ba.BookID
LEFT JOIN Authors a ON ba.AuthorID = a.AuthorID
WHERE 
    b.Title LIKE @SearchTerm OR
    b.Description LIKE @SearchTerm OR
    CONCAT(a.FirstName, ' ', a.LastName) LIKE @SearchTerm
GROUP BY b.BookID, b.Title, b.Description;
```

#### Фильтрация по жанру

```sql
SELECT DISTINCT b.*
FROM Books b
INNER JOIN BookGenres bg ON b.BookID = bg.BookID
WHERE bg.GenreID = @GenreID AND b.IsAvailable = 1;
```

## 🎨 Рекомендации по UI

### Показывать/скрывать элементы по ролям

```csharp
private void ConfigureUIForRole()
{
    if (SessionManager.IsAdmin)
    {
        // Админ видит все
        panelAdminControls.Visible = true;
        btnEdit.Enabled = true;
        btnDelete.Enabled = true;
        btnAdd.Enabled = true;
    }
    else
    {
        // Обычный пользователь - ограниченный доступ
        panelAdminControls.Visible = false;
        btnEdit.Enabled = false;
        btnDelete.Enabled = false;
        btnAdd.Enabled = false;
    }
}
```

### Отображение информации о пользователе в StatusStrip

```csharp
private void UpdateStatusBar()
{
    if (SessionManager.IsLoggedIn)
    {
        var user = SessionManager.CurrentUser;
        statusLabelUser.Text = $"Пользователь: {user.FullName}";
        statusLabelRole.Text = $"Роль: {user.RoleName}";
    }
    else
    {
        statusLabelUser.Text = "Не авторизован";
        statusLabelRole.Text = "";
    }
}
```

## ⚠️ Важные замечания

### 1. Всегда проверяйте авторизацию

```csharp
// ПЛОХО - не проверяем авторизацию
private void DeleteBook(int bookId)
{
    // Удаление книги...
}

// ХОРОШО - проверяем авторизацию и права
private void DeleteBook(int bookId)
{
    if (!SessionManager.IsLoggedIn)
    {
        MessageBox.Show("Необходимо войти в систему");
        return;
    }

    if (!SessionManager.IsAdmin)
    {
        MessageBox.Show("У вас нет прав для удаления книг");
        return;
    }

    // Удаление книги...
}
```

### 2. Проверка null при работе с CurrentUser

```csharp
// ПЛОХО - может быть NullReferenceException
string userName = SessionManager.CurrentUser.Username;

// ХОРОШО - проверяем перед использованием
if (SessionManager.CurrentUser != null)
{
    string userName = SessionManager.CurrentUser.Username;
    // Работа с userName...
}

// ИЛИ используем IsLoggedIn
if (SessionManager.IsLoggedIn)
{
    string userName = SessionManager.CurrentUser.Username;
}
```

### 3. Обработка выхода пользователя

```csharp
// При выходе всегда вызывайте Logout
private void HandleLogout()
{
    // Сохраняем незавершенные действия, если нужно
    SaveUnsavedData();

    // Очищаем UI
    ClearUserData();

    // Выход из сессии
    SessionManager.Logout();

    // Перенаправляем на форму входа
    this.Hide();
    LoginForm loginForm = new LoginForm();
    if (loginForm.ShowDialog() == DialogResult.OK)
    {
        this.Show();
        InitializeUI();
    }
    else
    {
        Application.Exit();
    }
}
```

## 🔗 Полезные ссылки

- **DatabaseHelper API**: см. `Database/DatabaseHelper.cs`
- **SessionManager API**: см. `Managers/SessionManager.cs`
- **User Model**: см. `Models/User.cs`
- **Validation Helper**: см. `Utils/ValidationHelper.cs`

## 📞 Вопросы?

Если у вас возникли вопросы по использованию API авторизации:
1. Проверьте примеры выше
2. Посмотрите исходный код в соответствующих классах
3. Обратитесь к разработчику модуля авторизации

---

**Успешной разработки!** 🚀

