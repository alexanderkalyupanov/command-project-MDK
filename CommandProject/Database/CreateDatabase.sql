-- Создание базы данных для онлайн библиотеки
-- Online Library Database Schema

USE master;
GO

-- Создание базы данных если её нет
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'OnlineLibraryDB')
BEGIN
    CREATE DATABASE OnlineLibraryDB;
END
GO

USE OnlineLibraryDB;
GO

-- ========================================
-- Таблица ролей
-- ========================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Roles')
BEGIN
    CREATE TABLE Roles (
        RoleID INT PRIMARY KEY IDENTITY(1,1),
        RoleName NVARCHAR(50) NOT NULL UNIQUE,
        Description NVARCHAR(255)
    );
    
    -- Вставка базовых ролей
    INSERT INTO Roles (RoleName, Description) VALUES 
    ('Admin', 'Администратор системы с полными правами'),
    ('User', 'Обычный пользователь библиотеки');
END
GO

-- ========================================
-- Таблица пользователей
-- ========================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Users')
BEGIN
    CREATE TABLE Users (
        UserID INT PRIMARY KEY IDENTITY(1,1),
        Username NVARCHAR(50) NOT NULL UNIQUE,
        Email NVARCHAR(100) NOT NULL UNIQUE,
        PasswordHash NVARCHAR(255) NOT NULL,
        FullName NVARCHAR(100),
        RoleID INT NOT NULL DEFAULT 2,
        CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        LastLogin DATETIME NULL,
        IsActive BIT NOT NULL DEFAULT 1,
        CONSTRAINT FK_Users_Roles FOREIGN KEY (RoleID) REFERENCES Roles(RoleID)
    );
    
    CREATE INDEX IX_Users_Username ON Users(Username);
    CREATE INDEX IX_Users_Email ON Users(Email);
END
GO

-- ========================================
-- Таблица авторов
-- ========================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Authors')
BEGIN
    CREATE TABLE Authors (
        AuthorID INT PRIMARY KEY IDENTITY(1,1),
        FirstName NVARCHAR(100) NOT NULL,
        LastName NVARCHAR(100) NOT NULL,
        Biography NVARCHAR(MAX),
        BirthDate DATE,
        Country NVARCHAR(100)
    );
    
    CREATE INDEX IX_Authors_Name ON Authors(LastName, FirstName);
END
GO

-- ========================================
-- Таблица жанров
-- ========================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Genres')
BEGIN
    CREATE TABLE Genres (
        GenreID INT PRIMARY KEY IDENTITY(1,1),
        GenreName NVARCHAR(50) NOT NULL UNIQUE,
        Description NVARCHAR(255)
    );
    
    -- Добавление базовых жанров
    INSERT INTO Genres (GenreName, Description) VALUES 
    ('Фантастика', 'Научная фантастика и фэнтези'),
    ('Детектив', 'Детективные романы и триллеры'),
    ('Роман', 'Романтическая литература'),
    ('Классика', 'Классическая литература'),
    ('Научная', 'Научная и образовательная литература'),
    ('Биография', 'Биографии и мемуары'),
    ('История', 'Историческая литература'),
    ('Поэзия', 'Поэтические произведения');
END
GO

-- ========================================
-- Таблица книг
-- ========================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Books')
BEGIN
    CREATE TABLE Books (
        BookID INT PRIMARY KEY IDENTITY(1,1),
        Title NVARCHAR(255) NOT NULL,
        ISBN NVARCHAR(20) UNIQUE,
        Description NVARCHAR(MAX),
        PublishedYear INT,
        Publisher NVARCHAR(100),
        PageCount INT,
        Language NVARCHAR(50) DEFAULT 'Русский',
        CoverImagePath NVARCHAR(500),
        FilePath NVARCHAR(500),
        FileFormat NVARCHAR(10) DEFAULT 'PDF',
        Rating DECIMAL(3,2) DEFAULT 0.00 CHECK (Rating >= 0 AND Rating <= 5),
        AddedDate DATETIME NOT NULL DEFAULT GETDATE(),
        AddedByUserID INT,
        IsAvailable BIT NOT NULL DEFAULT 1,
        CONSTRAINT FK_Books_Users FOREIGN KEY (AddedByUserID) REFERENCES Users(UserID)
    );
    
    CREATE INDEX IX_Books_Title ON Books(Title);
    CREATE INDEX IX_Books_ISBN ON Books(ISBN);
END
GO

-- ========================================
-- Связь книг и авторов (многие ко многим)
-- ========================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'BookAuthors')
BEGIN
    CREATE TABLE BookAuthors (
        BookID INT NOT NULL,
        AuthorID INT NOT NULL,
        PRIMARY KEY (BookID, AuthorID),
        CONSTRAINT FK_BookAuthors_Books FOREIGN KEY (BookID) REFERENCES Books(BookID) ON DELETE CASCADE,
        CONSTRAINT FK_BookAuthors_Authors FOREIGN KEY (AuthorID) REFERENCES Authors(AuthorID) ON DELETE CASCADE
    );
END
GO

-- ========================================
-- Связь книг и жанров (многие ко многим)
-- ========================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'BookGenres')
BEGIN
    CREATE TABLE BookGenres (
        BookID INT NOT NULL,
        GenreID INT NOT NULL,
        PRIMARY KEY (BookID, GenreID),
        CONSTRAINT FK_BookGenres_Books FOREIGN KEY (BookID) REFERENCES Books(BookID) ON DELETE CASCADE,
        CONSTRAINT FK_BookGenres_Genres FOREIGN KEY (GenreID) REFERENCES Genres(GenreID) ON DELETE CASCADE
    );
END
GO

-- ========================================
-- Таблица истории чтения
-- ========================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ReadingHistory')
BEGIN
    CREATE TABLE ReadingHistory (
        HistoryID INT PRIMARY KEY IDENTITY(1,1),
        UserID INT NOT NULL,
        BookID INT NOT NULL,
        StartDate DATETIME NOT NULL DEFAULT GETDATE(),
        LastReadDate DATETIME,
        CurrentPage INT DEFAULT 0,
        IsCompleted BIT DEFAULT 0,
        CONSTRAINT FK_ReadingHistory_Users FOREIGN KEY (UserID) REFERENCES Users(UserID) ON DELETE CASCADE,
        CONSTRAINT FK_ReadingHistory_Books FOREIGN KEY (BookID) REFERENCES Books(BookID) ON DELETE CASCADE
    );
    
    CREATE INDEX IX_ReadingHistory_User ON ReadingHistory(UserID);
END
GO

-- ========================================
-- Таблица избранного
-- ========================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Favorites')
BEGIN
    CREATE TABLE Favorites (
        FavoriteID INT PRIMARY KEY IDENTITY(1,1),
        UserID INT NOT NULL,
        BookID INT NOT NULL,
        AddedDate DATETIME NOT NULL DEFAULT GETDATE(),
        CONSTRAINT FK_Favorites_Users FOREIGN KEY (UserID) REFERENCES Users(UserID) ON DELETE CASCADE,
        CONSTRAINT FK_Favorites_Books FOREIGN KEY (BookID) REFERENCES Books(BookID) ON DELETE CASCADE,
        CONSTRAINT UQ_Favorites UNIQUE (UserID, BookID)
    );
END
GO

PRINT 'База данных успешно создана!';
GO

