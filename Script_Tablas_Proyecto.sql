CREATE DATABASE AntojeriaTicaDB;
GO

USE AntojeriaTicaDB;
GO

-- =====================================================
-- 2. Tabla de Roles
-- =====================================================
CREATE TABLE Roles (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(50) NOT NULL UNIQUE
);
GO

INSERT INTO Roles (Name) VALUES ('Cliente'), ('Administrador'), ('Empleado');
GO

SELECT * FROM Users;
SELECT * FROM Roles;

-- =====================================================
-- 3. Tabla de Usuarios
-- =====================================================
CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(256) NOT NULL,
    RoleId INT NOT NULL,
    DayCreate DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (RoleId) REFERENCES Roles(Id)
);
GO

-- =====================================================
-- 4. Tabla de Categorias
-- =====================================================
CREATE TABLE Categorias (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(50) NOT NULL UNIQUE,
    Descripcion NVARCHAR(255) NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE()
);
GO

-- =====================================================
-- 5. Tabla de Productos (Menu)
-- =====================================================
CREATE TABLE Productos (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100) NOT NULL,
    Descripcion NVARCHAR(255) NULL,
    Precio DECIMAL(10,2) NOT NULL,
    CategoriaId INT NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (CategoriaId) REFERENCES Categorias(Id)
);
GO

-- =====================================================
-- 6. Tabla de Pedidos
-- =====================================================
CREATE TABLE Pedidos (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL,            -- Cliente que realiza el pedido
    EmployeeId INT NULL,            -- Empleado asignado para atender el pedido
    OrderType NVARCHAR(20) NOT NULL, -- 'Mesa' o 'Domicilio'
    Mesa INT NULL,                  -- Numero de mesa (si aplica)
    DireccionEntrega NVARCHAR(255) NULL,  -- Direccinn de entrega
    FechaPedido DATETIME NOT NULL DEFAULT GETDATE(),
    Estado NVARCHAR(50) NOT NULL,   -- Ej: 'Pendiente', 'En preparacion', 'Entregado', 'Pagado'
    Total DECIMAL(10,2) NULL,       -- Total del pedido
    FOREIGN KEY (UserId) REFERENCES Users(Id),
    FOREIGN KEY (EmployeeId) REFERENCES Users(Id)
);
GO

-- =====================================================
-- 7. Tabla de Detalle de Pedidos
-- =====================================================
CREATE TABLE DetallePedidos (
    Id INT PRIMARY KEY IDENTITY(1,1),
    PedidoId INT NOT NULL,
    ProductoId INT NOT NULL,
    Cantidad INT NOT NULL,
    PrecioUnitario DECIMAL(10,2) NOT NULL,  -- Precio del producto al momento del pedido
    FOREIGN KEY (PedidoId) REFERENCES Pedidos(Id),
    FOREIGN KEY (ProductoId) REFERENCES Productos(Id)
);
GO

-- =====================================================
-- 8. Tabla de Ventas
-- =====================================================
CREATE TABLE Ventas (
    Id INT PRIMARY KEY IDENTITY(1,1),
    PedidoId INT NOT NULL,
    FechaVenta DATETIME NOT NULL DEFAULT GETDATE(),
    Total DECIMAL(10,2) NOT NULL,
    MetodoPago NVARCHAR(50) NOT NULL,  -- Ej: Sinpe
    FOREIGN KEY (PedidoId) REFERENCES Pedidos(Id)
);
GO

