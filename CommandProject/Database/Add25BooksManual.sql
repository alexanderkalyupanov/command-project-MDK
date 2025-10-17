-- ========================================
-- СКРИПТ ДЛЯ ДОБАВЛЕНИЯ 25 КНИГ В MS SQL SERVER MANAGEMENT STUDIO
-- Скопируйте этот скрипт и выполните в SSMS для правильной кодировки
-- ========================================

USE OnlineLibraryDB;
GO

-- Получаем ID администратора
DECLARE @AdminUserID INT;
SELECT @AdminUserID = UserID FROM Users WHERE Username = 'admin';

-- Получаем ID жанров
DECLARE @GenreClassic INT, @GenreFantasy INT, @GenreDetective INT, @GenrePoetry INT;
SELECT @GenreClassic = GenreID FROM Genres WHERE GenreName = N'Классика';
SELECT @GenreFantasy = GenreID FROM Genres WHERE GenreName = N'Фантастика';
SELECT @GenreDetective = GenreID FROM Genres WHERE GenreName = N'Детектив';
SELECT @GenrePoetry = GenreID FROM Genres WHERE GenreName = N'Поэзия';

-- ========================================
-- ДОБАВЛЕНИЕ НОВЫХ АВТОРОВ
-- ========================================

-- Николай Гоголь
IF NOT EXISTS (SELECT 1 FROM Authors WHERE LastName = N'Гоголь')
INSERT INTO Authors (FirstName, LastName, Biography, BirthDate, Country)
VALUES (N'Николай', N'Гоголь', N'Русский прозаик, драматург', '1809-04-01', N'Россия');

-- Иван Тургенев
IF NOT EXISTS (SELECT 1 FROM Authors WHERE LastName = N'Тургенев')
INSERT INTO Authors (FirstName, LastName, Biography, BirthDate, Country)
VALUES (N'Иван', N'Тургенев', N'Русский писатель-реалист', '1818-11-09', N'Россия');

-- Максим Горький
IF NOT EXISTS (SELECT 1 FROM Authors WHERE LastName = N'Горький')
INSERT INTO Authors (FirstName, LastName, Biography, BirthDate, Country)
VALUES (N'Максим', N'Горький', N'Русский писатель, прозаик', '1868-03-28', N'Россия');

-- Борис Пастернак
IF NOT EXISTS (SELECT 1 FROM Authors WHERE LastName = N'Пастернак')
INSERT INTO Authors (FirstName, LastName, Biography, BirthDate, Country)
VALUES (N'Борис', N'Пастернак', N'Русский поэт, лауреат Нобелевской премии', '1890-02-10', N'Россия');

-- Джордж Оруэлл
IF NOT EXISTS (SELECT 1 FROM Authors WHERE LastName = N'Оруэлл')
INSERT INTO Authors (FirstName, LastName, Biography, BirthDate, Country)
VALUES (N'Джордж', N'Оруэлл', N'Английский писатель', '1903-06-25', N'Великобритания');

-- Рэй Брэдбери
IF NOT EXISTS (SELECT 1 FROM Authors WHERE LastName = N'Брэдбери')
INSERT INTO Authors (FirstName, LastName, Biography, BirthDate, Country)
VALUES (N'Рэй', N'Брэдбери', N'Американский писатель-фантаст', '1920-08-22', N'США');

-- Айзек Азимов
IF NOT EXISTS (SELECT 1 FROM Authors WHERE LastName = N'Азимов')
INSERT INTO Authors (FirstName, LastName, Biography, BirthDate, Country)
VALUES (N'Айзек', N'Азимов', N'Американский писатель-фантаст', '1920-01-02', N'США');

-- Агата Кристи
IF NOT EXISTS (SELECT 1 FROM Authors WHERE LastName = N'Кристи')
INSERT INTO Authors (FirstName, LastName, Biography, BirthDate, Country)
VALUES (N'Агата', N'Кристи', N'Английская писательница детективов', '1890-09-15', N'Великобритания');

-- Александр Куприн
IF NOT EXISTS (SELECT 1 FROM Authors WHERE LastName = N'Куприн')
INSERT INTO Authors (FirstName, LastName, Biography, BirthDate, Country)
VALUES (N'Александр', N'Куприн', N'Русский писатель', '1870-09-07', N'Россия');

-- Александр Островский
IF NOT EXISTS (SELECT 1 FROM Authors WHERE LastName = N'Островский')
INSERT INTO Authors (FirstName, LastName, Biography, BirthDate, Country)
VALUES (N'Александр', N'Островский', N'Русский драматург', '1823-04-12', N'Россия');

-- Сергей Есенин
IF NOT EXISTS (SELECT 1 FROM Authors WHERE LastName = N'Есенин')
INSERT INTO Authors (FirstName, LastName, Biography, BirthDate, Country)
VALUES (N'Сергей', N'Есенин', N'Русский поэт', '1895-10-03', N'Россия');

-- Марина Цветаева
IF NOT EXISTS (SELECT 1 FROM Authors WHERE LastName = N'Цветаева')
INSERT INTO Authors (FirstName, LastName, Biography, BirthDate, Country)
VALUES (N'Марина', N'Цветаева', N'Русская поэтесса', '1892-10-08', N'Россия');

GO

-- ========================================
-- ДОБАВЛЕНИЕ 25 КНИГ
-- ========================================

DECLARE @BookID INT, @AuthorID INT, @AdminUserID INT;
DECLARE @GenreClassic INT, @GenreFantasy INT, @GenreDetective INT, @GenrePoetry INT;

SELECT @AdminUserID = UserID FROM Users WHERE Username = 'admin';
SELECT @GenreClassic = GenreID FROM Genres WHERE GenreName = N'Классика';
SELECT @GenreFantasy = GenreID FROM Genres WHERE GenreName = N'Фантастика';
SELECT @GenreDetective = GenreID FROM Genres WHERE GenreName = N'Детектив';
SELECT @GenrePoetry = GenreID FROM Genres WHERE GenreName = N'Поэзия';

-- Книга 6: Мёртвые души
SELECT @AuthorID = AuthorID FROM Authors WHERE LastName = N'Гоголь';
INSERT INTO Books (Title, ISBN, Description, PublishedYear, Publisher, PageCount, Language, FileFormat, Rating, AddedByUserID)
VALUES (N'Мёртвые души', '978-5-17-098770-9', N'Поэма Николая Гоголя', 1842, N'АСТ', 352, N'Русский', 'PDF', 4.72, @AdminUserID);
SET @BookID = SCOPE_IDENTITY();
INSERT INTO BookAuthors (BookID, AuthorID) VALUES (@BookID, @AuthorID);
INSERT INTO BookGenres (BookID, GenreID) VALUES (@BookID, @GenreClassic);

-- Книга 7: Отцы и дети
SELECT @AuthorID = AuthorID FROM Authors WHERE LastName = N'Тургенев';
INSERT INTO Books (Title, ISBN, Description, PublishedYear, Publisher, PageCount, Language, FileFormat, Rating, AddedByUserID)
VALUES (N'Отцы и дети', '978-5-17-098771-6', N'Роман о конфликте поколений', 1862, N'Эксмо', 288, N'Русский', 'PDF', 4.68, @AdminUserID);
SET @BookID = SCOPE_IDENTITY();
INSERT INTO BookAuthors (BookID, AuthorID) VALUES (@BookID, @AuthorID);
INSERT INTO BookGenres (BookID, GenreID) VALUES (@BookID, @GenreClassic);

-- Книга 8: Анна Каренина
SELECT @AuthorID = AuthorID FROM Authors WHERE LastName = N'Толстой';
INSERT INTO Books (Title, ISBN, Description, PublishedYear, Publisher, PageCount, Language, FileFormat, Rating, AddedByUserID)
VALUES (N'Анна Каренина', '978-5-17-098772-3', N'Роман о трагической любви', 1877, N'АСТ', 864, N'Русский', 'PDF', 4.89, @AdminUserID);
SET @BookID = SCOPE_IDENTITY();
INSERT INTO BookAuthors (BookID, AuthorID) VALUES (@BookID, @AuthorID);
INSERT INTO BookGenres (BookID, GenreID) VALUES (@BookID, @GenreClassic);

-- Книга 9: Идиот
SELECT @AuthorID = AuthorID FROM Authors WHERE LastName = N'Достоевский';
INSERT INTO Books (Title, ISBN, Description, PublishedYear, Publisher, PageCount, Language, FileFormat, Rating, AddedByUserID)
VALUES (N'Идиот', '978-5-17-098773-0', N'Роман о князе Мышкине', 1869, N'Эксмо', 640, N'Русский', 'PDF', 4.81, @AdminUserID);
SET @BookID = SCOPE_IDENTITY();
INSERT INTO BookAuthors (BookID, AuthorID) VALUES (@BookID, @AuthorID);
INSERT INTO BookGenres (BookID, GenreID) VALUES (@BookID, @GenreClassic);

-- Книга 10: Братья Карамазовы
SELECT @AuthorID = AuthorID FROM Authors WHERE LastName = N'Достоевский';
INSERT INTO Books (Title, ISBN, Description, PublishedYear, Publisher, PageCount, Language, FileFormat, Rating, AddedByUserID)
VALUES (N'Братья Карамазовы', '978-5-17-098774-7', N'Последний роман Достоевского', 1880, N'АСТ', 840, N'Русский', 'PDF', 4.90, @AdminUserID);
SET @BookID = SCOPE_IDENTITY();
INSERT INTO BookAuthors (BookID, AuthorID) VALUES (@BookID, @AuthorID);
INSERT INTO BookGenres (BookID, GenreID) VALUES (@BookID, @GenreClassic);

-- Книга 11: Собачье сердце
SELECT @AuthorID = AuthorID FROM Authors WHERE LastName = N'Булгаков';
INSERT INTO Books (Title, ISBN, Description, PublishedYear, Publisher, PageCount, Language, FileFormat, Rating, AddedByUserID)
VALUES (N'Собачье сердце', '978-5-17-098775-4', N'Повесть о профессоре Преображенском', 1925, N'АСТ', 192, N'Русский', 'PDF', 4.76, @AdminUserID);
SET @BookID = SCOPE_IDENTITY();
INSERT INTO BookAuthors (BookID, AuthorID) VALUES (@BookID, @AuthorID);
INSERT INTO BookGenres (BookID, GenreID) VALUES (@BookID, @GenreClassic);

-- Книга 12: На дне
SELECT @AuthorID = AuthorID FROM Authors WHERE LastName = N'Горький';
INSERT INTO Books (Title, ISBN, Description, PublishedYear, Publisher, PageCount, Language, FileFormat, Rating, AddedByUserID)
VALUES (N'На дне', '978-5-17-098776-1', N'Пьеса о жизни людей на социальном дне', 1902, N'Азбука', 128, N'Русский', 'PDF', 4.55, @AdminUserID);
SET @BookID = SCOPE_IDENTITY();
INSERT INTO BookAuthors (BookID, AuthorID) VALUES (@BookID, @AuthorID);
INSERT INTO BookGenres (BookID, GenreID) VALUES (@BookID, @GenreClassic);

-- Книга 13: Доктор Живаго
SELECT @AuthorID = AuthorID FROM Authors WHERE LastName = N'Пастернак';
INSERT INTO Books (Title, ISBN, Description, PublishedYear, Publisher, PageCount, Language, FileFormat, Rating, AddedByUserID)
VALUES (N'Доктор Живаго', '978-5-17-098777-8', N'Роман о русском интеллигенте', 1957, N'АСТ', 592, N'Русский', 'PDF', 4.70, @AdminUserID);
SET @BookID = SCOPE_IDENTITY();
INSERT INTO BookAuthors (BookID, AuthorID) VALUES (@BookID, @AuthorID);
INSERT INTO BookGenres (BookID, GenreID) VALUES (@BookID, @GenreClassic);

-- Книга 14: 1984
SELECT @AuthorID = AuthorID FROM Authors WHERE LastName = N'Оруэлл';
INSERT INTO Books (Title, ISBN, Description, PublishedYear, Publisher, PageCount, Language, FileFormat, Rating, AddedByUserID)
VALUES (N'1984', '978-5-17-098778-5', N'Антиутопия о тоталитарном обществе', 1949, N'АСТ', 320, N'Русский', 'PDF', 4.82, @AdminUserID);
SET @BookID = SCOPE_IDENTITY();
INSERT INTO BookAuthors (BookID, AuthorID) VALUES (@BookID, @AuthorID);
INSERT INTO BookGenres (BookID, GenreID) VALUES (@BookID, @GenreFantasy);

-- Книга 15: Скотный двор
SELECT @AuthorID = AuthorID FROM Authors WHERE LastName = N'Оруэлл';
INSERT INTO Books (Title, ISBN, Description, PublishedYear, Publisher, PageCount, Language, FileFormat, Rating, AddedByUserID)
VALUES (N'Скотный двор', '978-5-17-098779-2', N'Сатирическая повесть-притча', 1945, N'АСТ', 112, N'Русский', 'PDF', 4.64, @AdminUserID);
SET @BookID = SCOPE_IDENTITY();
INSERT INTO BookAuthors (BookID, AuthorID) VALUES (@BookID, @AuthorID);
INSERT INTO BookGenres (BookID, GenreID) VALUES (@BookID, @GenreFantasy);

-- Книга 16: 451 градус по Фаренгейту
SELECT @AuthorID = AuthorID FROM Authors WHERE LastName = N'Брэдбери';
INSERT INTO Books (Title, ISBN, Description, PublishedYear, Publisher, PageCount, Language, FileFormat, Rating, AddedByUserID)
VALUES (N'451 градус по Фаренгейту', '978-5-17-098780-8', N'Антиутопия о сжигании книг', 1953, N'Эксмо', 256, N'Русский', 'PDF', 4.75, @AdminUserID);
SET @BookID = SCOPE_IDENTITY();
INSERT INTO BookAuthors (BookID, AuthorID) VALUES (@BookID, @AuthorID);
INSERT INTO BookGenres (BookID, GenreID) VALUES (@BookID, @GenreFantasy);

-- Книга 17: Марсианские хроники
SELECT @AuthorID = AuthorID FROM Authors WHERE LastName = N'Брэдбери';
INSERT INTO Books (Title, ISBN, Description, PublishedYear, Publisher, PageCount, Language, FileFormat, Rating, AddedByUserID)
VALUES (N'Марсианские хроники', '978-5-17-098781-5', N'Сборник рассказов о Марсе', 1950, N'Эксмо', 304, N'Русский', 'PDF', 4.69, @AdminUserID);
SET @BookID = SCOPE_IDENTITY();
INSERT INTO BookAuthors (BookID, AuthorID) VALUES (@BookID, @AuthorID);
INSERT INTO BookGenres (BookID, GenreID) VALUES (@BookID, @GenreFantasy);

-- Книга 18: Основание
SELECT @AuthorID = AuthorID FROM Authors WHERE LastName = N'Азимов';
INSERT INTO Books (Title, ISBN, Description, PublishedYear, Publisher, PageCount, Language, FileFormat, Rating, AddedByUserID)
VALUES (N'Основание', '978-5-17-098782-2', N'Первый роман серии Основание', 1951, N'Эксмо', 288, N'Русский', 'PDF', 4.73, @AdminUserID);
SET @BookID = SCOPE_IDENTITY();
INSERT INTO BookAuthors (BookID, AuthorID) VALUES (@BookID, @AuthorID);
INSERT INTO BookGenres (BookID, GenreID) VALUES (@BookID, @GenreFantasy);

-- Книга 19: Я, робот
SELECT @AuthorID = AuthorID FROM Authors WHERE LastName = N'Азимов';
INSERT INTO Books (Title, ISBN, Description, PublishedYear, Publisher, PageCount, Language, FileFormat, Rating, AddedByUserID)
VALUES (N'Я, робот', '978-5-17-098783-9', N'Сборник рассказов о роботах', 1950, N'Эксмо', 224, N'Русский', 'PDF', 4.67, @AdminUserID);
SET @BookID = SCOPE_IDENTITY();
INSERT INTO BookAuthors (BookID, AuthorID) VALUES (@BookID, @AuthorID);
INSERT INTO BookGenres (BookID, GenreID) VALUES (@BookID, @GenreFantasy);

-- Книга 20: Убийство в Восточном экспрессе
SELECT @AuthorID = AuthorID FROM Authors WHERE LastName = N'Кристи';
INSERT INTO Books (Title, ISBN, Description, PublishedYear, Publisher, PageCount, Language, FileFormat, Rating, AddedByUserID)
VALUES (N'Убийство в Восточном экспрессе', '978-5-17-098784-6', N'Детектив о расследовании Эркюля Пуаро', 1934, N'Эксмо', 256, N'Русский', 'PDF', 4.79, @AdminUserID);
SET @BookID = SCOPE_IDENTITY();
INSERT INTO BookAuthors (BookID, AuthorID) VALUES (@BookID, @AuthorID);
INSERT INTO BookGenres (BookID, GenreID) VALUES (@BookID, @GenreDetective);

-- Книга 21: Десять негритят
SELECT @AuthorID = AuthorID FROM Authors WHERE LastName = N'Кристи';
INSERT INTO Books (Title, ISBN, Description, PublishedYear, Publisher, PageCount, Language, FileFormat, Rating, AddedByUserID)
VALUES (N'Десять негритят', '978-5-17-098785-3', N'Классический детектив Агаты Кристи', 1939, N'Эксмо', 272, N'Русский', 'PDF', 4.83, @AdminUserID);
SET @BookID = SCOPE_IDENTITY();
INSERT INTO BookAuthors (BookID, AuthorID) VALUES (@BookID, @AuthorID);
INSERT INTO BookGenres (BookID, GenreID) VALUES (@BookID, @GenreDetective);

-- Книга 22: Гранатовый браслет
SELECT @AuthorID = AuthorID FROM Authors WHERE LastName = N'Куприн';
INSERT INTO Books (Title, ISBN, Description, PublishedYear, Publisher, PageCount, Language, FileFormat, Rating, AddedByUserID)
VALUES (N'Гранатовый браслет', '978-5-17-098786-0', N'Повесть о безответной любви', 1911, N'АСТ', 96, N'Русский', 'PDF', 4.61, @AdminUserID);
SET @BookID = SCOPE_IDENTITY();
INSERT INTO BookAuthors (BookID, AuthorID) VALUES (@BookID, @AuthorID);
INSERT INTO BookGenres (BookID, GenreID) VALUES (@BookID, @GenreClassic);

-- Книга 23: Капитанская дочка
SELECT @AuthorID = AuthorID FROM Authors WHERE LastName = N'Пушкин';
INSERT INTO Books (Title, ISBN, Description, PublishedYear, Publisher, PageCount, Language, FileFormat, Rating, AddedByUserID)
VALUES (N'Капитанская дочка', '978-5-17-098787-7', N'Роман о Пугачёвском восстании', 1836, N'АСТ', 144, N'Русский', 'PDF', 4.58, @AdminUserID);
SET @BookID = SCOPE_IDENTITY();
INSERT INTO BookAuthors (BookID, AuthorID) VALUES (@BookID, @AuthorID);
INSERT INTO BookGenres (BookID, GenreID) VALUES (@BookID, @GenreClassic);

-- Книга 24: Повести Белкина
SELECT @AuthorID = AuthorID FROM Authors WHERE LastName = N'Пушкин';
INSERT INTO Books (Title, ISBN, Description, PublishedYear, Publisher, PageCount, Language, FileFormat, Rating, AddedByUserID)
VALUES (N'Повести Белкина', '978-5-17-098788-4', N'Цикл повестей Пушкина', 1831, N'АСТ', 128, N'Русский', 'PDF', 4.52, @AdminUserID);
SET @BookID = SCOPE_IDENTITY();
INSERT INTO BookAuthors (BookID, AuthorID) VALUES (@BookID, @AuthorID);
INSERT INTO BookGenres (BookID, GenreID) VALUES (@BookID, @GenreClassic);

-- Книга 25: Бесприданница
SELECT @AuthorID = AuthorID FROM Authors WHERE LastName = N'Островский';
INSERT INTO Books (Title, ISBN, Description, PublishedYear, Publisher, PageCount, Language, FileFormat, Rating, AddedByUserID)
VALUES (N'Бесприданница', '978-5-17-098789-1', N'Драма о судьбе бедной девушки', 1879, N'Азбука', 96, N'Русский', 'PDF', 4.47, @AdminUserID);
SET @BookID = SCOPE_IDENTITY();
INSERT INTO BookAuthors (BookID, AuthorID) VALUES (@BookID, @AuthorID);
INSERT INTO BookGenres (BookID, GenreID) VALUES (@BookID, @GenreClassic);

-- Книга 26: Стихотворения (Есенин)
SELECT @AuthorID = AuthorID FROM Authors WHERE LastName = N'Есенин';
INSERT INTO Books (Title, ISBN, Description, PublishedYear, Publisher, PageCount, Language, FileFormat, Rating, AddedByUserID)
VALUES (N'Стихотворения', '978-5-17-098790-7', N'Избранные стихотворения Есенина', 1925, N'АСТ', 256, N'Русский', 'PDF', 4.86, @AdminUserID);
SET @BookID = SCOPE_IDENTITY();
INSERT INTO BookAuthors (BookID, AuthorID) VALUES (@BookID, @AuthorID);
INSERT INTO BookGenres (BookID, GenreID) VALUES (@BookID, @GenrePoetry);

-- Книга 27: Избранное (Цветаева)
SELECT @AuthorID = AuthorID FROM Authors WHERE LastName = N'Цветаева';
INSERT INTO Books (Title, ISBN, Description, PublishedYear, Publisher, PageCount, Language, FileFormat, Rating, AddedByUserID)
VALUES (N'Избранное', '978-5-17-098791-4', N'Избранные стихотворения Цветаевой', 1941, N'АСТ', 288, N'Русский', 'PDF', 4.71, @AdminUserID);
SET @BookID = SCOPE_IDENTITY();
INSERT INTO BookAuthors (BookID, AuthorID) VALUES (@BookID, @AuthorID);
INSERT INTO BookGenres (BookID, GenreID) VALUES (@BookID, @GenrePoetry);

-- Книга 28: Руслан и Людмила
SELECT @AuthorID = AuthorID FROM Authors WHERE LastName = N'Пушкин';
INSERT INTO Books (Title, ISBN, Description, PublishedYear, Publisher, PageCount, Language, FileFormat, Rating, AddedByUserID)
VALUES (N'Руслан и Людмила', '978-5-17-098792-1', N'Поэма о богатыре Руслане', 1820, N'АСТ', 144, N'Русский', 'PDF', 4.63, @AdminUserID);
SET @BookID = SCOPE_IDENTITY();
INSERT INTO BookAuthors (BookID, AuthorID) VALUES (@BookID, @AuthorID);
INSERT INTO BookGenres (BookID, GenreID) VALUES (@BookID, @GenrePoetry);

-- Книга 29: Белая гвардия
SELECT @AuthorID = AuthorID FROM Authors WHERE LastName = N'Булгаков';
INSERT INTO Books (Title, ISBN, Description, PublishedYear, Publisher, PageCount, Language, FileFormat, Rating, AddedByUserID)
VALUES (N'Белая гвардия', '978-5-17-098793-8', N'Роман о гражданской войне', 1924, N'АСТ', 384, N'Русский', 'PDF', 4.74, @AdminUserID);
SET @BookID = SCOPE_IDENTITY();
INSERT INTO BookAuthors (BookID, AuthorID) VALUES (@BookID, @AuthorID);
INSERT INTO BookGenres (BookID, GenreID) VALUES (@BookID, @GenreClassic);

-- Книга 30: Дубровский
SELECT @AuthorID = AuthorID FROM Authors WHERE LastName = N'Пушкин';
INSERT INTO Books (Title, ISBN, Description, PublishedYear, Publisher, PageCount, Language, FileFormat, Rating, AddedByUserID)
VALUES (N'Дубровский', '978-5-17-098794-5', N'Роман о благородном разбойнике', 1833, N'АСТ', 128, N'Русский', 'PDF', 4.56, @AdminUserID);
SET @BookID = SCOPE_IDENTITY();
INSERT INTO BookAuthors (BookID, AuthorID) VALUES (@BookID, @AuthorID);
INSERT INTO BookGenres (BookID, GenreID) VALUES (@BookID, @GenreClassic);

GO

-- Проверка результата
PRINT '========================================';
PRINT 'Добавление завершено!';
PRINT '========================================';
SELECT COUNT(*) AS [Всего книг] FROM Books;
SELECT COUNT(*) AS [Всего авторов] FROM Authors;
PRINT '';
PRINT 'ТОП-10 книг по рейтингу:';
SELECT TOP 10 Title AS [Название], Rating AS [Рейтинг] FROM Books ORDER BY Rating DESC;
GO

