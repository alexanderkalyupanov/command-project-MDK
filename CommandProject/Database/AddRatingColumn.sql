-- Скрипт миграции: добавление поля Rating в таблицу Books
-- Используйте этот скрипт, если БД уже была создана без поля Rating

USE OnlineLibraryDB;
GO

-- Проверяем и добавляем колонку Rating
IF NOT EXISTS (
    SELECT 1
    FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_NAME = 'Books' 
    AND COLUMN_NAME = 'Rating'
)
BEGIN
    -- Добавляем колонку Rating
    ALTER TABLE Books
    ADD Rating DECIMAL(3,2) DEFAULT 0.00
    CONSTRAINT CK_Books_Rating CHECK (Rating >= 0 AND Rating <= 5);
    
    PRINT 'Колонка Rating успешно добавлена в таблицу Books';
END
ELSE
BEGIN
    PRINT 'Колонка Rating уже существует в таблице Books';
END
GO

-- Обновляем рейтинги тестовых книг по BookID (если они существуют)
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Books' AND COLUMN_NAME = 'Rating')
BEGIN
    UPDATE Books SET Rating = 4.85 WHERE BookID = 1 AND (Rating IS NULL OR Rating = 0.00);  -- Евгений Онегин
    UPDATE Books SET Rating = 4.92 WHERE BookID = 2 AND (Rating IS NULL OR Rating = 0.00);  -- Война и мир
    UPDATE Books SET Rating = 4.78 WHERE BookID = 3 AND (Rating IS NULL OR Rating = 0.00);  -- Преступление и наказание
    UPDATE Books SET Rating = 4.65 WHERE BookID = 4 AND (Rating IS NULL OR Rating = 0.00);  -- Вишнёвый сад
    UPDATE Books SET Rating = 4.88 WHERE BookID = 5 AND (Rating IS NULL OR Rating = 0.00);  -- Мастер и Маргарита
    
    PRINT 'Ratings for test books have been updated';
END
GO

