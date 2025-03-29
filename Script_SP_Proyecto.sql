-- =====================================================
--                  Store Procedures
-- =====================================================

USE AntojeriaTicaDB;
GO

-- =====================================================
-- Registro
-- =====================================================
CREATE PROCEDURE [dbo].[RegistrarCuenta]
    @Username NVARCHAR(50),
    @Email NVARCHAR(100),
    @PasswordHash NVARCHAR(256)
AS
BEGIN
    SET NOCOUNT ON;

    -- Verificar que no exista un usuario con el mismo Username o Email
    IF NOT EXISTS(
        SELECT 1 
        FROM Users
        WHERE Username = @Username OR Email = @Email
    )
    BEGIN
        INSERT INTO Users (Username, Email, PasswordHash, RoleId)
        VALUES (
            @Username,
            @Email,
            @PasswordHash,
            (SELECT TOP 1 Id FROM Roles WHERE Name = 'Cliente')
        );
    END
END;
GO

-- =====================================================
-- Login
-- =====================================================
CREATE PROCEDURE [dbo].[IniciarSesion]
    @Email NVARCHAR(100),
    @PasswordHash NVARCHAR(256)
AS
BEGIN
    SELECT  U.Id,
            U.Username,
            U.Email,
            U.PasswordHash,
            U.RoleId,
            R.Name AS NombrePerfil,
            U.DayCreate
    FROM    dbo.Users U
    INNER JOIN dbo.Roles R ON U.RoleId = R.Id
    WHERE   U.Email = @Email
        AND U.PasswordHash = @PasswordHash;
END;
GO



