-- ============================================
-- Tạo cơ sở dữ liệu nếu chưa tồn tại
-- ============================================
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'BanHang')
BEGIN
    CREATE DATABASE BanHang;
END
GO

-- Sử dụng cơ sở dữ liệu BanHang
USE BanHang;
GO

-- ============================================
-- Xóa kết nối tới cơ sở dữ liệu trước khi xóa
-- ============================================
IF EXISTS (SELECT * FROM sys.databases WHERE name = 'BanHang')
BEGIN
    -- Ngắt tất cả kết nối đến cơ sở dữ liệu
    ALTER DATABASE BanHang SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE BanHang;
END
GO

-- ============================================
-- Xóa bảng nếu đã tồn tại
-- ============================================

-- Brand
DROP TABLE IF EXISTS Brand;

-- Category
DROP TABLE IF EXISTS Category;

-- Order
DROP TABLE IF EXISTS `Order`;

-- Users
DROP TABLE IF EXISTS Users;

-- Product
DROP TABLE IF EXISTS Product;

-- ============================================
-- Tạo bảng
-- ============================================

-- Brand
CREATE TABLE Brand (
    Id INT PRIMARY KEY IDENTITY(1,1), -- Khóa chính, tự động tăng
    Name NVARCHAR(100) NOT NULL,      -- Tên thương hiệu
    Avatar NVARCHAR(100),             -- Ảnh đại diện
    Slug VARCHAR(100),                -- Slug thân thiện với URL
    ShowOnHomePage bit DEFAULT 0, -- Hiển thị trên trang chủ (mặc định là 0)
    DisplayOrder INT DEFAULT 0,       -- Thứ tự hiển thị (mặc định là 0)
    CreatedOnUtc DATETIME DEFAULT CURRENT_TIMESTAMP, -- Thời gian tạo (UTC)
    UpdatedOnUtc DATETIME NULL,       -- Thời gian cập nhật cuối
    Deleted bit DEFAULT 0      -- Cờ xóa (mặc định là 0)
);

-- Category
CREATE TABLE Category (
    Id INT PRIMARY KEY IDENTITY(1,1), -- Khóa chính, tự động tăng
    Name NVARCHAR(100) NOT NULL,      -- Tên danh mục
    Avatar NVARCHAR(100),             -- Ảnh đại diện
    Slug VARCHAR(100),                -- Slug thân thiện với URL
    ShowOnHomePage bit DEFAULT 0, -- Hiển thị trên trang chủ (mặc định là 0)
    DisplayOrder INT DEFAULT 0,       -- Thứ tự hiển thị (mặc định là 0)
    Deleted bit DEFAULT 0,     -- Cờ xóa (mặc định là 0)
    CreatedOnUtc DATETIME DEFAULT CURRENT_TIMESTAMP, -- Thời gian tạo (UTC)
    UpdatedOnUtc DATETIME NULL        -- Thời gian cập nhật cuối
);

-- Order
CREATE TABLE [Order] (
    Id INT PRIMARY KEY IDENTITY(1,1), -- Khóa chính, tự động tăng
    Name NVARCHAR(100) NOT NULL,      -- Tên đơn hàng hoặc khách hàng
    ProductId INT NOT NULL,           -- Liên kết với sản phẩm
    Price FLOAT NOT NULL,             -- Giá
    Status INT DEFAULT 0,             -- Trạng thái (mặc định là 0)
    CreatedOnUtc DATETIME DEFAULT CURRENT_TIMESTAMP -- Thời gian tạo (UTC)
);

-- Product
CREATE TABLE Product (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Avatar NVARCHAR(100),
    CategoryId INT NOT NULL,
    ShortDes NVARCHAR(100),
    FullDescription NVARCHAR(500),
    Price FLOAT,
    PriceDiscount FLOAT,
    TypeId INT,
    Slug VARCHAR(50),
    BrandId INT,
    Deleted bit DEFAULT 0,
    ShowOnHomePage bit DEFAULT 0,
    DisplayOrder INT,
    CreatedOnUtc DATETIME DEFAULT CURRENT_TIMESTAMP,
    UpdatedOnUtc DATETIME NULL
);

-- Users
CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY(1,1),
    FirstName VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    Email VARCHAR(50) NOT NULL UNIQUE,
    Password VARCHAR(255) NOT NULL, -- Password có thể mã hóa, vì vậy sử dụng VARCHAR(255)
    IsAdmin bit DEFAULT 0 -- Quyền quản trị
);

-- ============================================
-- Thêm dữ liệu mẫu vào bảng Brand
-- ============================================

-- Brand
INSERT INTO Brand (Name, Avatar, Slug, ShowOnHomePage, DisplayOrder, CreatedOnUtc, UpdatedOnUtc, Deleted)
VALUES 
('Nike', 'nike-logo.png', 'nike', 1, 1, CURRENT_TIMESTAMP, NULL, 0),
('Adidas', 'adidas-logo.png', 'adidas', 1, 2, CURRENT_TIMESTAMP, NULL, 0),
('Puma', 'puma-logo.png', 'puma', 0, 3, CURRENT_TIMESTAMP, NULL, 0),
('Reebok', 'reebok-logo.png', 'reebok', 1, 4, CURRENT_TIMESTAMP, NULL, 0),
('New Balance', 'newbalance-logo.png', 'new-balance', 0, 5, CURRENT_TIMESTAMP, NULL, 0);

-- Category
INSERT INTO Category (Name, Avatar, Slug, ShowOnHomePage, DisplayOrder, Deleted, CreatedOnUtc, UpdatedOnUtc)
VALUES 
('Electronics', 'electronics.png', 'electronics', 1, 1, 0, CURRENT_TIMESTAMP, NULL),
('Clothing', 'clothing.png', 'clothing', 1, 2, 0, CURRENT_TIMESTAMP, NULL),
('Books', 'books.png', 'books', 0, 3, 0, CURRENT_TIMESTAMP, NULL),
('Home Appliances', 'home-appliances.png', 'home-appliances', 1, 4, 0, CURRENT_TIMESTAMP, NULL),
('Toys', 'toys.png', 'toys', 0, 5, 0, CURRENT_TIMESTAMP, NULL);


-- ============================================
-- Kiểm tra dữ liệu trong bảng
-- ============================================
SELECT * FROM Brand;
SELECT * FROM Category;
SELECT * FROM [Order];
SELECT * FROM Product;
SELECT * FROM Users;
