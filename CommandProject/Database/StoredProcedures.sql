-- Хранимые процедуры для работы с пользователями и авторизацией
USE OnlineLibraryDB;
GO

-- ========================================
-- Процедура регистрации нового пользователя
-- ========================================
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'sp_RegisterUser')
    DROP PROCEDURE sp_RegisterUser;
GO

CREATE PROCEDURE sp_RegisterUser
    @Username NVARCHAR(50),
    @Email NVARCHAR(100),
    @PasswordHash NVARCHAR(255),
    @FullName NVARCHAR(100),
    @Result INT OUTPUT,
    @Message NVARCHAR(255) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        -- Проверка существования пользователя
        IF EXISTS (SELECT 1 FROM Users WHERE Username = @Username)
        BEGIN
            SET @Result = -1;
            SET @Message = 'Пользователь с таким именем уже существует';
            RETURN;
        END
        
        IF EXISTS (SELECT 1 FROM Users WHERE Email = @Email)
        BEGIN
            SET @Result = -2;
            SET @Message = 'Email уже зарегистрирован';
            RETURN;
        END
        
        -- Вставка нового пользователя (RoleID = 2 для обычного пользователя)
        INSERT INTO Users (Username, Email, PasswordHash, FullName, RoleID)
        VALUES (@Username, @Email, @PasswordHash, @FullName, 2);
        
        SET @Result = SCOPE_IDENTITY();
        SET @Message = 'Регистрация успешна';
        
    END TRY
    BEGIN CATCH
        SET @Result = -999;
        SET @Message = ERROR_MESSAGE();
    END CATCH
END
GO

-- ========================================
-- Процедура авторизации пользователя
-- ========================================
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'sp_LoginUser')
    DROP PROCEDURE sp_LoginUser;
GO

CREATE PROCEDURE sp_LoginUser
    @Username NVARCHAR(50),
    @PasswordHash NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Обновление времени последнего входа
    UPDATE Users 
    SET LastLogin = GETDATE() 
    WHERE Username = @Username AND PasswordHash = @PasswordHash AND IsActive = 1;
    
    -- Возврат данных пользователя
    SELECT 
        u.UserID,
        u.Username,
        u.Email,
        u.FullName,
        u.RoleID,
        r.RoleName,
        u.CreatedAt,
        u.LastLogin
    FROM Users u
    INNER JOIN Roles r ON u.RoleID = r.RoleID
    WHERE u.Username = @Username 
        AND u.PasswordHash = @PasswordHash 
        AND u.IsActive = 1;
END
GO

-- ========================================
-- Процедура получения информации о пользователе
-- ========================================
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'sp_GetUserInfo')
    DROP PROCEDURE sp_GetUserInfo;
GO

CREATE PROCEDURE sp_GetUserInfo
    @UserID INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        u.UserID,
        u.Username,
        u.Email,
        u.FullName,
        u.RoleID,
        r.RoleName,
        u.CreatedAt,
        u.LastLogin,
        u.IsActive
    FROM Users u
    INNER JOIN Roles r ON u.RoleID = r.RoleID
    WHERE u.UserID = @UserID;
END
GO

-- ========================================
-- Процедура получения всех пользователей (для админа)
-- ========================================
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'sp_GetAllUsers')
    DROP PROCEDURE sp_GetAllUsers;
GO

CREATE PROCEDURE sp_GetAllUsers
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        u.UserID,
        u.Username,
        u.Email,
        u.FullName,
        r.RoleName,
        u.CreatedAt,
        u.LastLogin,
        u.IsActive
    FROM Users u
    INNER JOIN Roles r ON u.RoleID = r.RoleID
    ORDER BY u.CreatedAt DESC;
END
GO

-- ========================================
-- Процедура изменения роли пользователя (для админа)
-- ========================================
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'sp_UpdateUserRole')
    DROP PROCEDURE sp_UpdateUserRole;
GO

CREATE PROCEDURE sp_UpdateUserRole
    @UserID INT,
    @RoleID INT,
    @Result BIT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        UPDATE Users 
        SET RoleID = @RoleID 
        WHERE UserID = @UserID;
        
        SET @Result = 1;
    END TRY
    BEGIN CATCH
        SET @Result = 0;
    END CATCH
END
GO

-- ========================================
-- Процедура деактивации пользователя (для админа)
-- ========================================
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'sp_DeactivateUser')
    DROP PROCEDURE sp_DeactivateUser;
GO

CREATE PROCEDURE sp_DeactivateUser
    @UserID INT,
    @Result BIT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        UPDATE Users 
        SET IsActive = 0 
        WHERE UserID = @UserID;
        
        SET @Result = 1;
    END TRY
    BEGIN CATCH
        SET @Result = 0;
    END CATCH
END
GO

-- ========================================
-- Процедура активации пользователя (для админа)
-- ========================================
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'sp_ActivateUser')
    DROP PROCEDURE sp_ActivateUser;
GO

CREATE PROCEDURE sp_ActivateUser
    @UserID INT,
    @Result BIT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        UPDATE Users 
        SET IsActive = 1 
        WHERE UserID = @UserID;
        
        SET @Result = 1;
    END TRY
    BEGIN CATCH
        SET @Result = 0;
    END CATCH
END
GO

PRINT 'Хранимые процедуры успешно созданы!';
GO

