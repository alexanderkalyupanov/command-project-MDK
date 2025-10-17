# 🔄 Инструкция по применению обновлений

## ✨ Что нового в версии 1.1

### 📊 База данных
- **Добавлено поле Rating** в таблицу Books (рейтинг книг от 0.00 до 5.00)
- Поле CoverImagePath уже было в структуре БД

### 🎨 Дизайн
- **Новая цветовая схема:** Оранжевый (#FF6B35), Черный (#1A1A1A), Белый
- Обновлены формы входа и регистрации

---

## 🚀 Как применить обновления

### Вариант 1: У вас ЕЩЁ НЕТ созданной БД

**Просто создайте БД с нуля:**

```powershell
cd "C:\Users\alexa\OneDrive\Документы\GitHub\command-project-MDK\CommandProject"

sqlcmd -S DESKTOP-UQACTGM\MSSQLSERVALEX -i Database\CreateDatabase.sql
sqlcmd -S DESKTOP-UQACTGM\MSSQLSERVALEX -i Database\StoredProcedures.sql
sqlcmd -S DESKTOP-UQACTGM\MSSQLSERVALEX -i Database\InsertTestData.sql
```

✅ Готово! Поле Rating будет создано автоматически.

---

### Вариант 2: БД УЖЕ СОЗДАНА (нужно добавить поле Rating)

**Выполните скрипт миграции:**

```powershell
cd "C:\Users\alexa\OneDrive\Документы\GitHub\command-project-MDK\CommandProject"

sqlcmd -S DESKTOP-UQACTGM\MSSQLSERVALEX -i Database\AddRatingColumn.sql
```

Этот скрипт:
- Проверит, есть ли уже поле Rating
- Добавит его, если его нет
- Обновит рейтинги тестовых книг

✅ Готово! Поле Rating добавлено в существующую БД.

---

### Вариант 3: Пересоздать БД полностью

**Если хотите начать с чистого листа:**

```sql
-- 1. Откройте SQL Server Management Studio (SSMS)
-- 2. Подключитесь к DESKTOP-UQACTGM\MSSQLSERVALEX
-- 3. Выполните:

USE master;
GO
DROP DATABASE OnlineLibraryDB;
GO
```

**Затем создайте БД заново (Вариант 1)**

---

## 🎨 Обновление интерфейса

**Формы обновляются автоматически!**

Просто пересоберите проект:
1. Откройте Visual Studio
2. Откройте `CommandProject.sln`
3. Нажмите `Ctrl+Shift+B` (Build)
4. Запустите `F5`

### Новая цветовая схема:

| Элемент | Цвет | RGB |
|---------|------|-----|
| Заголовки | Оранжевый | (255, 107, 53) |
| Кнопки | Оранжевый | (255, 107, 53) |
| Ссылки | Оранжевый | (255, 107, 53) |
| Текст/Лейблы | Черный | (26, 26, 26) |
| Фон | Белый | (255, 255, 255) |

---

## ✅ Проверка обновлений

### Проверьте поле Rating в БД:

```sql
USE OnlineLibraryDB;
GO

-- Проверка структуры таблицы
SELECT COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'Books' AND COLUMN_NAME = 'Rating';

-- Должно вывести: Rating | decimal | NULL

-- Проверка данных
SELECT Title, Rating FROM Books;

-- Должны увидеть рейтинги книг (4.85, 4.92, и т.д.)
```

### Проверьте новый дизайн:

1. Запустите приложение (F5)
2. Должны увидеть:
   - ✅ Оранжевый заголовок "Вход"
   - ✅ Оранжевую кнопку "Войти"
   - ✅ Черный текст лейблов
   - ✅ Белый фон

---

## 📝 Использование Rating в коде

### Добавление книги с рейтингом:

```sql
INSERT INTO Books (Title, ISBN, Description, Rating, AddedByUserID)
VALUES (
    'Название книги',
    '978-5-12345-678-9',
    'Описание книги',
    4.50,  -- Рейтинг
    @UserID
);
```

### Получение книг с рейтингом:

```sql
SELECT 
    BookID,
    Title,
    Rating,
    CoverImagePath
FROM Books
WHERE IsAvailable = 1
ORDER BY Rating DESC;  -- Сортировка по рейтингу
```

### В C# коде:

```csharp
// При работе с книгами
string query = @"
    SELECT BookID, Title, Rating, CoverImagePath
    FROM Books
    WHERE Rating >= @MinRating
    ORDER BY Rating DESC";

SqlParameter ratingParam = new SqlParameter("@MinRating", 4.0);
// Выполнить запрос...
```

---

## 🐛 Решение проблем

### Ошибка: "Invalid column name 'Rating'"

**Причина:** Поле Rating не добавлено в БД

**Решение:**
```powershell
sqlcmd -S DESKTOP-UQACTGM\MSSQLSERVALEX -i Database\AddRatingColumn.sql
```

### Ошибка: "Cannot connect to database"

**Причина:** Неверное имя сервера

**Решение:** Проверьте строку подключения в `App.config`:
```xml
Server=DESKTOP-UQACTGM\MSSQLSERVALEX;Database=OnlineLibraryDB;Integrated Security=true;
```

### Формы выглядят по-старому

**Решение:**
1. Очистите решение: Build → Clean Solution
2. Пересоберите: Build → Rebuild Solution
3. Запустите заново

---

## 📞 Дополнительная информация

- **CHANGELOG.md** - Полная история изменений
- **README_AUTH.md** - Документация модуля
- **TEAM_API_GUIDE.md** - Примеры использования

---

## ✨ Готово!

После выполнения обновлений:
- ✅ Таблица Books содержит поле Rating
- ✅ Формы используют новую цветовую схему (оранжевый/черный/белый)
- ✅ Все работает как раньше + новые возможности

**Можно продолжать разработку!** 🚀

