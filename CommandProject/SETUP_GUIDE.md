# 🚀 Руководство по установке и запуску проекта

## Предварительные требования

- ✅ Windows 10/11
- ✅ Visual Studio 2019 или новее (с поддержкой .NET Framework 4.8)
- ✅ SQL Server (один из вариантов):
  - SQL Server LocalDB (рекомендуется, обычно идет с Visual Studio)
  - SQL Server Express
  - SQL Server (полная версия)
- ✅ SQL Server Management Studio (SSMS) - опционально, но рекомендуется

## 📝 Пошаговая инструкция

### Шаг 1: Клонирование репозитория

```bash
git clone <URL-репозитория>
cd command-project-MDK/CommandProject
```

### Шаг 2: Проверка установки SQL Server

1. Откройте командную строку и выполните:
```bash
sqllocaldb info
```

2. Если увидите список экземпляров (например, `MSSQLLocalDB`), значит LocalDB установлен.

3. Если LocalDB не установлен:
   - Откройте Visual Studio Installer
   - Выберите "Изменить"
   - В разделе "Отдельные компоненты" найдите и установите "SQL Server Express 2019 LocalDB"

### Шаг 3: Создание базы данных

#### Вариант A: Через SQL Server Management Studio (SSMS)

1. Откройте SSMS
2. Подключитесь к серверу: `(localdb)\MSSQLLocalDB`
   - Server type: Database Engine
   - Server name: `(localdb)\MSSQLLocalDB`
   - Authentication: Windows Authentication
3. Откройте файл `Database/CreateDatabase.sql`
4. Нажмите Execute (F5)
5. Откройте файл `Database/StoredProcedures.sql`
6. Нажмите Execute (F5)
7. Откройте файл `Database/InsertTestData.sql`
8. Нажмите Execute (F5)

#### Вариант B: Через командную строку

```bash
# Перейдите в папку проекта
cd C:\Users\alexa\OneDrive\Документы\GitHub\command-project-MDK\CommandProject

# Создайте базу данных
sqlcmd -S (localdb)\MSSQLLocalDB -i Database\CreateDatabase.sql

# Создайте хранимые процедуры
sqlcmd -S (localdb)\MSSQLLocalDB -i Database\StoredProcedures.sql

# Добавьте тестовые данные
sqlcmd -S (localdb)\MSSQLLocalDB -i Database\InsertTestData.sql
```

### Шаг 4: Настройка строки подключения

1. Откройте файл `App.config`
2. Проверьте строку подключения:

```xml
<connectionStrings>
    <add name="OnlineLibraryDB" 
         connectionString="Server=(localdb)\MSSQLLocalDB;Database=OnlineLibraryDB;Integrated Security=true;" 
         providerName="System.Data.SqlClient" />
</connectionStrings>
```

3. Если используете SQL Server Express, измените на:
```xml
connectionString="Server=.\SQLEXPRESS;Database=OnlineLibraryDB;Integrated Security=true;"
```

### Шаг 5: Открытие проекта в Visual Studio

1. Запустите Visual Studio
2. Выберите "Open a project or solution"
3. Откройте файл `CommandProject.sln`
4. Подождите, пока Visual Studio восстановит NuGet пакеты

### Шаг 6: Сборка проекта

1. В Visual Studio выберите Build → Build Solution (Ctrl+Shift+B)
2. Убедитесь, что сборка прошла успешно (смотрите Output окно)

### Шаг 7: Запуск приложения

1. Нажмите F5 или кнопку "Start"
2. Должна открыться форма входа в систему

## 🧪 Тестирование

### Тестовые учетные записи

После выполнения скрипта `InsertTestData.sql` будут доступны:

**Администратор:**
- Логин: `admin`
- Пароль: `admin123`

**Пользователь:**
- Логин: `user`
- Пароль: `user123`

### Тестирование регистрации

1. На форме входа нажмите "Нет аккаунта? Зарегистрироваться"
2. Заполните форму:
   - Имя пользователя: `testuser`
   - Email: `test@example.com`
   - Полное имя: `Тест Тестов`
   - Пароль: `test123`
   - Подтверждение пароля: `test123`
3. Нажмите "Зарегистрироваться"
4. После успешной регистрации войдите с новыми учетными данными

## 🐛 Решение проблем

### Проблема: "Cannot connect to database"

**Решение:**
1. Убедитесь, что SQL Server LocalDB запущен:
```bash
sqllocaldb start MSSQLLocalDB
```

2. Проверьте подключение:
```bash
sqlcmd -S (localdb)\MSSQLLocalDB -Q "SELECT @@VERSION"
```

### Проблема: "Database 'OnlineLibraryDB' does not exist"

**Решение:**
Выполните заново скрипты создания БД (Шаг 3)

### Проблема: "Login failed for user"

**Решение:**
1. Убедитесь, что используется Windows Authentication
2. Проверьте, что ваш пользователь Windows имеет права на SQL Server

### Проблема: Ошибки компиляции в Visual Studio

**Решение:**
1. Очистите решение: Build → Clean Solution
2. Восстановите NuGet пакеты: Tools → NuGet Package Manager → Restore NuGet Packages
3. Пересоберите: Build → Rebuild Solution

### Проблема: "System.Configuration.ConfigurationErrorsException"

**Решение:**
1. Убедитесь, что в проекте есть ссылка на System.Configuration
2. Проверьте, что файл App.config скопирован в выходную директорию

## 📁 Структура проекта после установки

```
CommandProject/
├── bin/
│   └── Debug/
│       ├── CommandProject.exe
│       └── CommandProject.exe.config
├── Database/
│   ├── CreateDatabase.sql
│   ├── StoredProcedures.sql
│   └── InsertTestData.sql
├── Forms/
│   ├── LoginForm.cs
│   └── RegisterForm.cs
├── Database/
│   └── DatabaseHelper.cs
├── Managers/
│   └── SessionManager.cs
├── Models/
│   └── User.cs
├── Utils/
│   └── ValidationHelper.cs
└── App.config
```

## 🔄 Обновление базы данных

Если нужно пересоздать БД:

```sql
-- 1. Удалите существующую БД
USE master;
DROP DATABASE OnlineLibraryDB;
GO

-- 2. Выполните скрипты заново
-- (следуйте Шагу 3)
```

## 📊 Проверка структуры БД

Выполните в SSMS:

```sql
USE OnlineLibraryDB;

-- Проверка таблиц
SELECT TABLE_NAME 
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_TYPE = 'BASE TABLE'
ORDER BY TABLE_NAME;

-- Проверка пользователей
SELECT UserID, Username, Email, RoleName, IsActive
FROM Users u
JOIN Roles r ON u.RoleID = r.RoleID;

-- Проверка книг
SELECT BookID, Title, PublishedYear
FROM Books;
```

## 📞 Получение помощи

Если у вас возникли проблемы:

1. Проверьте раздел "Решение проблем" выше
2. Убедитесь, что все предварительные требования выполнены
3. Проверьте лог ошибок в Output окне Visual Studio
4. Обратитесь к разработчику модуля

## ✅ Чек-лист готовности

- [ ] SQL Server установлен и запущен
- [ ] База данных создана
- [ ] Хранимые процедуры созданы
- [ ] Тестовые данные загружены
- [ ] Проект собран без ошибок
- [ ] Форма входа открывается
- [ ] Можно войти с тестовыми учетными данными
- [ ] Регистрация новых пользователей работает

---

**Удачи в разработке!** 🎉

