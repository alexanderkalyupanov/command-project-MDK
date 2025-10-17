-- Скрипт для добавления тестовых данных
USE OnlineLibraryDB;
GO

-- ========================================
-- Создание тестового администратора
-- ========================================
-- Логин: admin
-- Пароль: admin123
IF NOT EXISTS (SELECT 1 FROM Users WHERE Username = 'admin')
BEGIN
    INSERT INTO Users (Username, Email, PasswordHash, FullName, RoleID, IsActive)
    VALUES (
        'admin', 
        'admin@library.com', 
        '240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9', -- admin123 (SHA256)
        'Системный администратор',
        1,
        1
    );
    PRINT 'Администратор создан. Логин: admin, Пароль: admin123';
END
ELSE
BEGIN
    PRINT 'Администратор уже существует';
END
GO

-- ========================================
-- Создание тестового пользователя
-- ========================================
-- Логин: user
-- Пароль: user123
IF NOT EXISTS (SELECT 1 FROM Users WHERE Username = 'user')
BEGIN
    INSERT INTO Users (Username, Email, PasswordHash, FullName, RoleID, IsActive)
    VALUES (
        'user', 
        'user@library.com', 
        '6ca13d52ca70c883e0f0bb101e425a89e8624de51db2d2392593af6a84118090', -- user123 (SHA256)
        'Тестовый пользователь',
        2,
        1
    );
    PRINT 'Пользователь создан. Логин: user, Пароль: user123';
END
ELSE
BEGIN
    PRINT 'Пользователь уже существует';
END
GO

-- ========================================
-- Добавление тестовых авторов
-- ========================================
INSERT INTO Authors (FirstName, LastName, Biography, BirthDate, Country)
VALUES 
    ('Александр', 'Пушкин', 'Великий русский поэт, драматург и прозаик', '1799-06-06', 'Россия'),
    ('Лев', 'Толстой', 'Один из величайших русских писателей и мыслителей', '1828-09-09', 'Россия'),
    ('Федор', 'Достоевский', 'Великий русский писатель, мыслитель, философ', '1821-11-11', 'Россия'),
    ('Антон', 'Чехов', 'Русский писатель, прозаик, драматург', '1860-01-29', 'Россия'),
    ('Михаил', 'Булгаков', 'Русский писатель, драматург, театральный режиссёр', '1891-05-15', 'Россия');
GO

-- ========================================
-- Добавление тестовых книг
-- ========================================
DECLARE @AuthorPushkin INT, @AuthorTolstoy INT, @AuthorDostoevsky INT, @AuthorChekhov INT, @AuthorBulgakov INT;
DECLARE @GenreFiction INT, @GenreClassic INT, @GenrePoetry INT;
DECLARE @AdminUserID INT;

-- Получаем ID авторов
SELECT @AuthorPushkin = AuthorID FROM Authors WHERE LastName = 'Пушкин';
SELECT @AuthorTolstoy = AuthorID FROM Authors WHERE LastName = 'Толстой';
SELECT @AuthorDostoevsky = AuthorID FROM Authors WHERE LastName = 'Достоевский';
SELECT @AuthorChekhov = AuthorID FROM Authors WHERE LastName = 'Чехов';
SELECT @AuthorBulgakov = AuthorID FROM Authors WHERE LastName = 'Булгаков';

-- Получаем ID жанров
SELECT @GenreFiction = GenreID FROM Genres WHERE GenreName = 'Фантастика';
SELECT @GenreClassic = GenreID FROM Genres WHERE GenreName = 'Классика';
SELECT @GenrePoetry = GenreID FROM Genres WHERE GenreName = 'Поэзия';

-- Получаем ID администратора
SELECT @AdminUserID = UserID FROM Users WHERE Username = 'admin';

-- Вставка книг
DECLARE @BookID INT;

-- Книга 1: Евгений Онегин
INSERT INTO Books (Title, ISBN, Description, PublishedYear, Publisher, PageCount, Language, FileFormat, Rating, AddedByUserID)
VALUES (
    'Евгений Онегин',
    '978-5-17-098765-4',
    'Роман в стихах Александра Пушкина, написанный в 1823—1831 годах',
    1833,
    'Азбука-классика',
    416,
    'Русский',
    'PDF',
    4.85,
    @AdminUserID
);
SET @BookID = SCOPE_IDENTITY();
INSERT INTO BookAuthors (BookID, AuthorID) VALUES (@BookID, @AuthorPushkin);
INSERT INTO BookGenres (BookID, GenreID) VALUES (@BookID, @GenreClassic), (@BookID, @GenrePoetry);

-- Книга 2: Война и мир
INSERT INTO Books (Title, ISBN, Description, PublishedYear, Publisher, PageCount, Language, FileFormat, Rating, AddedByUserID)
VALUES (
    'Война и мир',
    '978-5-17-098766-1',
    'Роман-эпопея Льва Толстого, описывающий русское общество в эпоху войн против Наполеона',
    1869,
    'АСТ',
    1300,
    'Русский',
    'PDF',
    4.92,
    @AdminUserID
);
SET @BookID = SCOPE_IDENTITY();
INSERT INTO BookAuthors (BookID, AuthorID) VALUES (@BookID, @AuthorTolstoy);
INSERT INTO BookGenres (BookID, GenreID) VALUES (@BookID, @GenreClassic);

-- Книга 3: Преступление и наказание
INSERT INTO Books (Title, ISBN, Description, PublishedYear, Publisher, PageCount, Language, FileFormat, Rating, AddedByUserID)
VALUES (
    'Преступление и наказание',
    '978-5-17-098767-8',
    'Социально-психологический и социально-философский роман Фёдора Достоевского',
    1866,
    'Эксмо',
    608,
    'Русский',
    'PDF',
    4.78,
    @AdminUserID
);
SET @BookID = SCOPE_IDENTITY();
INSERT INTO BookAuthors (BookID, AuthorID) VALUES (@BookID, @AuthorDostoevsky);
INSERT INTO BookGenres (BookID, GenreID) VALUES (@BookID, @GenreClassic);

-- Книга 4: Вишнёвый сад
INSERT INTO Books (Title, ISBN, Description, PublishedYear, Publisher, PageCount, Language, FileFormat, Rating, AddedByUserID)
VALUES (
    'Вишнёвый сад',
    '978-5-17-098768-5',
    'Пьеса в четырёх действиях Антона Чехова',
    1904,
    'Азбука',
    192,
    'Русский',
    'PDF',
    4.65,
    @AdminUserID
);
SET @BookID = SCOPE_IDENTITY();
INSERT INTO BookAuthors (BookID, AuthorID) VALUES (@BookID, @AuthorChekhov);
INSERT INTO BookGenres (BookID, GenreID) VALUES (@BookID, @GenreClassic);

-- Книга 5: Мастер и Маргарита
INSERT INTO Books (Title, ISBN, Description, PublishedYear, Publisher, PageCount, Language, FileFormat, Rating, AddedByUserID)
VALUES (
    'Мастер и Маргарита',
    '978-5-17-098769-2',
    'Роман Михаила Булгакова, работа над которым началась в конце 1920-х годов',
    1967,
    'АСТ',
    480,
    'Русский',
    'PDF',
    4.88,
    @AdminUserID
);
SET @BookID = SCOPE_IDENTITY();
INSERT INTO BookAuthors (BookID, AuthorID) VALUES (@BookID, @AuthorBulgakov);
INSERT INTO BookGenres (BookID, GenreID) VALUES (@BookID, @GenreClassic), (@BookID, @GenreFiction);

PRINT 'Тестовые данные успешно добавлены!';
PRINT '';
PRINT '========================================';
PRINT 'ТЕСТОВЫЕ УЧЕТНЫЕ ЗАПИСИ:';
PRINT '========================================';
PRINT 'Администратор:';
PRINT '  Логин: admin';
PRINT '  Пароль: admin123';
PRINT '';
PRINT 'Пользователь:';
PRINT '  Логин: user';
PRINT '  Пароль: user123';
PRINT '========================================';
GO

