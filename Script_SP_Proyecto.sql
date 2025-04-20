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

-- =====================================================
--                  Stored Procedures / Addendum
-- =====================================================

USE AntojeriaTicaDB;
GO

-------------------------------------------------------------------------------
--  A.1 InsertarRol
-------------------------------------------------------------------------------
CREATE PROCEDURE InsertarRol
    @Name NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Roles (Name)
    VALUES (@Name);
END;
GO

-------------------------------------------------------------------------------
--  A.2 ListarRoles
-------------------------------------------------------------------------------
CREATE PROCEDURE ListarRoles
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, Name
    FROM Roles;
END;
GO

-------------------------------------------------------------------------------
--  A.3 ObtenerRolPorId
-------------------------------------------------------------------------------
CREATE PROCEDURE ObtenerRolPorId
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, Name
    FROM Roles
    WHERE Id = @Id;
END;
GO

-------------------------------------------------------------------------------
--  A.4 ActualizarRol
-------------------------------------------------------------------------------
CREATE PROCEDURE ActualizarRol
    @Id INT,
    @Name NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Roles
       SET Name = @Name
     WHERE Id = @Id;
END;
GO

-------------------------------------------------------------------------------
--  A.5 EliminarRol
-------------------------------------------------------------------------------
CREATE PROCEDURE EliminarRol
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Roles
    WHERE Id = @Id;
END;
GO

SELECT * FROM Users;
SELECT * FROM Roles;
SELECT * FROM Categorias;
-------------------------------------------------------------------------------
--  B.1 InsertarUsuario
-------------------------------------------------------------------------------
CREATE PROCEDURE InsertarUsuario
    @Username NVARCHAR(50),
    @Email NVARCHAR(100),
    @PasswordHash NVARCHAR(256),
    @RoleId INT
AS
BEGIN
    IF EXISTS (SELECT 1 FROM Users WHERE Email = @Email)
    BEGIN
        PRINT 'Ya existe un usuario con el mismo correo electrónico.';
        RETURN;
    END

    BEGIN TRY
        INSERT INTO Users (Username, Email, PasswordHash, RoleId)
        VALUES (@Username, @Email, @PasswordHash, @RoleId);

        PRINT 'Usuario insertado correctamente.';
    END TRY
    BEGIN CATCH
        PRINT 'Error al insertar usuario: ' + ERROR_MESSAGE();
    END CATCH
END;
GO

-------------------------------------------------------------------------------
--  B.2 ListarUsuarios
-------------------------------------------------------------------------------
CREATE PROCEDURE ListarUsuarios
AS
BEGIN
    SET NOCOUNT ON;

    SELECT U.Id,
           U.Username,
           U.Email,
           U.PasswordHash,
           U.RoleId,
           R.Name AS NombreRol,
           U.DayCreate
    FROM Users U
    INNER JOIN Roles R ON U.RoleId = R.Id;
END;
GO


-------------------------------------------------------------------------------
--  B.4 ObtenerUsuarioPorNombre
-------------------------------------------------------------------------------
CREATE PROCEDURE ObtenerUsuarioPorNombre
    @Username NVARCHAR(50)
AS
BEGIN
    SELECT U.*, R.Name AS RoleName
    FROM Users U
    INNER JOIN Roles R ON U.RoleId = R.Id
    WHERE U.Username = @Username;
END;
GO

-------------------------------------------------------------------------------
--  B.3 ObtenerUsuarioPorId
-------------------------------------------------------------------------------
CREATE PROCEDURE ObtenerUsuarioPorId
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT U.Id,
           U.Username,
           U.Email,
           U.PasswordHash,
           U.RoleId,
           R.Name AS NombreRol,
           U.DayCreate
    FROM Users U
    INNER JOIN Roles R ON U.RoleId = R.Id
    WHERE U.Id = @Id;
END;
GO

-------------------------------------------------------------------------------
--  B.4 ActualizarUsuario
-------------------------------------------------------------------------------
CREATE PROCEDURE ActualizarUsuario
    @Id INT,
    @Username NVARCHAR(50) = NULL,
    @Email NVARCHAR(100) = NULL,
    @PasswordHash NVARCHAR(256) = NULL,
    @RoleId INT = NULL
AS
BEGIN
    IF NOT EXISTS (SELECT 1 FROM Users WHERE Id = @Id)
    BEGIN
        PRINT 'El usuario con el ID especificado no existe.';
        RETURN;
    END

    UPDATE Users
    SET
        Username = ISNULL(@Username, Username),
        Email = ISNULL(@Email, Email),
        PasswordHash = ISNULL(@PasswordHash, PasswordHash),
        RoleId = ISNULL(@RoleId, RoleId)
    WHERE Id = @Id;

    PRINT 'Usuario actualizado correctamente.';
END;
GO

-------------------------------------------------------------------------------
--  B.5 EliminarUsuario
-------------------------------------------------------------------------------
CREATE PROCEDURE EliminarUsuario
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM Users WHERE Id = @Id)
    BEGIN
        RAISERROR('El usuario no existe', 16, 1);
        RETURN;
    END

    DELETE FROM Users WHERE Id = @Id;
END;
GO
-----------------------------------------------------------
-- Procedimiento: InsertarCategoria
-----------------------------------------------------------
CREATE PROCEDURE InsertarCategoria
    @Nombre NVARCHAR(50),
    @Descripcion NVARCHAR(255),
    @NuevoId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
 
    INSERT INTO Categorias (Nombre, Descripcion, CreatedAt)
    VALUES (@Nombre, @Descripcion, GETDATE());
 
    SET @NuevoId = SCOPE_IDENTITY();
END;
-----------------------------------------------------------
-- Procedimiento: ListarCategorias
-----------------------------------------------------------
CREATE PROCEDURE ListarCategorias
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Nombre, Descripcion, CreatedAt FROM Categorias;
END;
-----------------------------------------------------------
-- Procedimiento: ObtenerCategoriaPorId
-----------------------------------------------------------
CREATE PROCEDURE ObtenerCategoriaPorId
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Nombre, Descripcion, CreatedAt FROM Categorias
    WHERE Id = @Id;
END;
-----------------------------------------------------------
-- Procedimiento: ActualizarCategoria
-----------------------------------------------------------
CREATE PROCEDURE ActualizarCategoria
    @Id INT,
    @Nombre NVARCHAR(50) = NULL,
    @Descripcion NVARCHAR(255) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE Categorias
       SET Nombre = @Nombre,
           Descripcion = @Descripcion
     WHERE Id = @Id;

	 PRINT 'Categoria actualizada correctamente.';
END
GO
-----------------------------------------------------------
-- Procedimiento: EliminarCategoria
-----------------------------------------------------------
CREATE PROCEDURE EliminarCategoria
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Categorias WHERE Id = @Id;
END;
-------------------------------------------------------------------------------
--  D.1 InsertarProducto
-------------------------------------------------------------------------------
CREATE PROCEDURE InsertarProducto
    @Nombre NVARCHAR(100),
    @Descripcion NVARCHAR(255),
    @Precio DECIMAL(10,2),
    @CategoriaId INT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Productos (Nombre, Descripcion, Precio, CategoriaId, IsActive, CreatedAt)
    VALUES (@Nombre, @Descripcion, @Precio, @CategoriaId, 1, GETDATE());
END;
GO

-------------------------------------------------------------------------------
--  D.2 ListarProductos
-------------------------------------------------------------------------------
CREATE PROCEDURE ListarProductos
AS
BEGIN
    SET NOCOUNT ON;

    SELECT P.Id,
           P.Nombre,
           P.Descripcion,
           P.Precio,
           P.CategoriaId,
           C.Nombre AS CategoriaNombre,
           P.IsActive,
           P.CreatedAt
    FROM Productos P
    INNER JOIN Categorias C ON P.CategoriaId = C.Id;
END;
GO

-------------------------------------------------------------------------------
--  D.3 ObtenerProductoPorId
-------------------------------------------------------------------------------
CREATE PROCEDURE ObtenerProductoPorId
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT P.Id,
           P.Nombre,
           P.Descripcion,
           P.Precio,
           P.CategoriaId,
           C.Nombre AS CategoriaNombre,
           P.IsActive,
           P.CreatedAt
    FROM Productos P
    INNER JOIN Categorias C ON P.CategoriaId = C.Id
    WHERE P.Id = @Id;
END;
GO

-------------------------------------------------------------------------------
--  D.4 ActualizarProducto
-------------------------------------------------------------------------------
CREATE PROCEDURE ActualizarProducto
    @Id INT,
    @Nombre NVARCHAR(100),
    @Descripcion NVARCHAR(255),
    @Precio DECIMAL(10,2),
    @CategoriaId INT,
    @IsActive BIT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Productos
       SET Nombre = @Nombre,
           Descripcion = @Descripcion,
           Precio = @Precio,
           CategoriaId = @CategoriaId,
           IsActive = @IsActive
     WHERE Id = @Id;
END;
GO

-------------------------------------------------------------------------------
--  D.5 EliminarProducto
-------------------------------------------------------------------------------
CREATE PROCEDURE EliminarProducto
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Productos
    WHERE Id = @Id;
END;
GO

-------------------------------------------------------------------------------
--  E.1 RealizarPedido (Insert)
-------------------------------------------------------------------------------
CREATE PROCEDURE RealizarPedido
    @UserId INT,
    @OrderType NVARCHAR(20),
    @Mesa INT = NULL,
    @DireccionEntrega NVARCHAR(255) = NULL,
    @Total DECIMAL(10,2)
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @PedidoId INT;

    INSERT INTO Pedidos (
        UserId, EmployeeId, OrderType, Mesa, DireccionEntrega, FechaPedido, Estado, Total
    ) VALUES (
        @UserId, NULL, @OrderType, @Mesa, @DireccionEntrega, GETDATE(), 'Pendiente', @Total
    );

    SET @PedidoId = SCOPE_IDENTITY();
    SELECT @PedidoId AS PedidoId;
END;
GO

-------------------------------------------------------------------------------
--  E.2 ListarPedidos
-------------------------------------------------------------------------------
CREATE PROCEDURE ListarPedidos
AS
BEGIN
    SET NOCOUNT ON;

    SELECT P.Id,
           P.UserId,
           P.EmployeeId,
           P.OrderType,
           P.Mesa,
           P.DireccionEntrega,
           P.FechaPedido,
           P.Estado,
           P.Total
    FROM Pedidos P;
END;
GO

-------------------------------------------------------------------------------
--  E.3 ObtenerPedidoPorId
-------------------------------------------------------------------------------
CREATE PROCEDURE ObtenerPedidoPorId
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT P.Id,
           P.UserId,
           P.EmployeeId,
           P.OrderType,
           P.Mesa,
           P.DireccionEntrega,
           P.FechaPedido,
           P.Estado,
           P.Total
    FROM Pedidos P
    WHERE P.Id = @Id;
END;
GO

-------------------------------------------------------------------------------
--  E.4 ActualizarPedido
-------------------------------------------------------------------------------
CREATE PROCEDURE ActualizarPedido
    @Id INT,
    @UserId INT,
    @EmployeeId INT = NULL,
    @OrderType NVARCHAR(20),
    @Mesa INT = NULL,
    @DireccionEntrega NVARCHAR(255) = NULL,
    @Estado NVARCHAR(50),
    @Total DECIMAL(10,2) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Pedidos
       SET UserId = @UserId,
           EmployeeId = @EmployeeId,
           OrderType = @OrderType,
           Mesa = @Mesa,
           DireccionEntrega = @DireccionEntrega,
           Estado = @Estado,
           Total = @Total
     WHERE Id = @Id;
END;
GO

-------------------------------------------------------------------------------
--  E.5 EliminarPedido
-------------------------------------------------------------------------------
CREATE PROCEDURE EliminarPedido
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Pedidos
    WHERE Id = @Id;
END;
GO

-------------------------------------------------------------------------------
--  E.6 ActualizarEstadoPedido 
-------------------------------------------------------------------------------
CREATE PROCEDURE ActualizarEstadoPedido
    @PedidoId INT,
    @NuevoEstado NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Pedidos
       SET Estado = @NuevoEstado
     WHERE Id = @PedidoId;
END;
GO

-------------------------------------------------------------------------------
--  F.1 InsertarDetallePedido
-------------------------------------------------------------------------------
CREATE PROCEDURE InsertarDetallePedido
    @PedidoId INT,
    @ProductoId INT,
    @Cantidad INT,
    @PrecioUnitario DECIMAL(10,2)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO DetallePedidos (PedidoId, ProductoId, Cantidad, PrecioUnitario)
    VALUES (@PedidoId, @ProductoId, @Cantidad, @PrecioUnitario);
END;
GO

-------------------------------------------------------------------------------
--  F.2 ListarDetallePedidos
-------------------------------------------------------------------------------
CREATE PROCEDURE ListarDetallePedidos
AS
BEGIN
    SET NOCOUNT ON;

    SELECT D.Id,
           D.PedidoId,
           D.ProductoId,
           D.Cantidad,
           D.PrecioUnitario
    FROM DetallePedidos D;
END;
GO

-------------------------------------------------------------------------------
--  F.3 ObtenerDetallePedidoPorId
-------------------------------------------------------------------------------
CREATE PROCEDURE ObtenerDetallePedidoPorId
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT D.Id,
           D.PedidoId,
           D.ProductoId,
           D.Cantidad,
           D.PrecioUnitario
    FROM DetallePedidos D
    WHERE D.Id = @Id;
END;
GO

-------------------------------------------------------------------------------
--  F.4 ActualizarDetallePedido
-------------------------------------------------------------------------------
CREATE PROCEDURE ActualizarDetallePedido
    @Id INT,
    @PedidoId INT,
    @ProductoId INT,
    @Cantidad INT,
    @PrecioUnitario DECIMAL(10,2)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE DetallePedidos
       SET PedidoId = @PedidoId,
           ProductoId = @ProductoId,
           Cantidad = @Cantidad,
           PrecioUnitario = @PrecioUnitario
     WHERE Id = @Id;
END;
GO

-------------------------------------------------------------------------------
--  F.5 EliminarDetallePedido
-------------------------------------------------------------------------------
CREATE PROCEDURE EliminarDetallePedido
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM DetallePedidos
    WHERE Id = @Id;
END;
GO

-------------------------------------------------------------------------------
--  G.1 RegistrarVenta
-------------------------------------------------------------------------------
CREATE PROCEDURE RegistrarVenta
    @PedidoId INT,
    @MetodoPago NVARCHAR(50),
    @Total DECIMAL(10,2)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Ventas (PedidoId, FechaVenta, MetodoPago, Total)
    VALUES (@PedidoId, GETDATE(), @MetodoPago, @Total);
END;
GO

-------------------------------------------------------------------------------
--  G.2 ListarVentas
-------------------------------------------------------------------------------
CREATE PROCEDURE ListarVentas
AS
BEGIN
    SET NOCOUNT ON;

    SELECT V.Id,
           V.PedidoId,
           V.FechaVenta,
           V.MetodoPago,
           V.Total
    FROM Ventas V;
END;
GO

-------------------------------------------------------------------------------
--  G.3 ObtenerVentaPorId
-------------------------------------------------------------------------------
CREATE PROCEDURE ObtenerVentaPorId
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT V.Id,
           V.PedidoId,
           V.FechaVenta,
           V.MetodoPago,
           V.Total
    FROM Ventas V
    WHERE V.Id = @Id;
END;
GO

-------------------------------------------------------------------------------
--  G.4 ActualizarVenta
-------------------------------------------------------------------------------
CREATE PROCEDURE ActualizarVenta
    @Id INT,
    @PedidoId INT,
    @FechaVenta DATETIME,
    @MetodoPago NVARCHAR(50),
    @Total DECIMAL(10,2)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Ventas
       SET PedidoId = @PedidoId,
           FechaVenta = @FechaVenta,
           MetodoPago = @MetodoPago,
           Total = @Total
     WHERE Id = @Id;
END;
GO

-------------------------------------------------------------------------------
--  G.5 EliminarVenta
-------------------------------------------------------------------------------
CREATE PROCEDURE EliminarVenta
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Ventas
    WHERE Id = @Id;
END;
GO




