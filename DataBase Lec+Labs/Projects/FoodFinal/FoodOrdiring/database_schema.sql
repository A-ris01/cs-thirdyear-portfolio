-- ============================================================
--  Food Ordering & Delivery System — Database Schema
--
--  HOW TO RUN (MySQL Workbench):
--  1. Open MySQL Workbench and connect to your local server
--  2. Click File > Open SQL Script and select this file
--  3. Press Ctrl+Shift+Enter to run the entire script
--
--  Connection string used in the app:
--  server=localhost;port=3306;username=root;password=YOUR_PASSWORD;database=FoodOrdiring;
-- ============================================================

CREATE DATABASE IF NOT EXISTS FoodOrdiring
    CHARACTER SET utf8mb4
    COLLATE utf8mb4_unicode_ci;

USE FoodOrdiring;

-- ── Users (unified auth table) ──────────────────────────────
CREATE TABLE IF NOT EXISTS Users (
    Id       INT AUTO_INCREMENT PRIMARY KEY,
    Name     VARCHAR(100) NOT NULL,
    Email    VARCHAR(150) NOT NULL UNIQUE,
    Password VARCHAR(100) NOT NULL,
    Role     ENUM('Admin','Customer','Staff','Driver') NOT NULL
);

-- ── Admin (extended profile, same Id as Users) ──────────────
CREATE TABLE IF NOT EXISTS Admin (
    Id    INT PRIMARY KEY,
    Name  VARCHAR(100),
    Email VARCHAR(150),
    FOREIGN KEY (Id) REFERENCES Users(Id) ON DELETE CASCADE
);

-- ── Customer ─────────────────────────────────────────────────
CREATE TABLE IF NOT EXISTS Customer (
    Id      INT PRIMARY KEY,
    Name    VARCHAR(100),
    Email   VARCHAR(150),
    Address VARCHAR(255),
    FOREIGN KEY (Id) REFERENCES Users(Id) ON DELETE CASCADE
);

-- ── Restaurant ───────────────────────────────────────────────
CREATE TABLE IF NOT EXISTS Restaurant (
    Id      INT AUTO_INCREMENT PRIMARY KEY,
    Name    VARCHAR(150) NOT NULL,
    AdminId INT,
    FOREIGN KEY (AdminId) REFERENCES Users(Id) ON DELETE SET NULL
);

-- ── RestaurantStaff ──────────────────────────────────────────
CREATE TABLE IF NOT EXISTS RestaurantStaff (
    Id     INT PRIMARY KEY,
    Name   VARCHAR(100),
    Email  VARCHAR(150),
    RestId INT,
    FOREIGN KEY (Id)     REFERENCES Users(Id)       ON DELETE CASCADE,
    FOREIGN KEY (RestId) REFERENCES Restaurant(Id)  ON DELETE SET NULL
);

-- ── DeliveryPerson ───────────────────────────────────────────
CREATE TABLE IF NOT EXISTS DeliveryPerson (
    Id      INT PRIMARY KEY,
    Name    VARCHAR(100),
    Email   VARCHAR(150),
    Vehicle VARCHAR(100),
    FOREIGN KEY (Id) REFERENCES Users(Id) ON DELETE CASCADE
);

-- ── MenuItem ─────────────────────────────────────────────────
CREATE TABLE IF NOT EXISTS MenuItem (
    Id     INT AUTO_INCREMENT PRIMARY KEY,
    Name   VARCHAR(150) NOT NULL,
    Price  DOUBLE       NOT NULL,
    RestId INT,
    FOREIGN KEY (RestId) REFERENCES Restaurant(Id) ON DELETE CASCADE
);

-- ── Orders ───────────────────────────────────────────────────
CREATE TABLE IF NOT EXISTS Orders (
    Id     INT AUTO_INCREMENT PRIMARY KEY,
    Status VARCHAR(50) DEFAULT 'Pending',
    CustId INT,
    RestId INT,
    FOREIGN KEY (CustId) REFERENCES Users(Id)        ON DELETE SET NULL,
    FOREIGN KEY (RestId) REFERENCES Restaurant(Id)   ON DELETE SET NULL
);

-- ── OrderDetails ─────────────────────────────────────────────
CREATE TABLE IF NOT EXISTS OrderDetails (
    OrderId INT,
    ItemId  INT,
    Qty     INT NOT NULL DEFAULT 1,
    PRIMARY KEY (OrderId, ItemId),
    FOREIGN KEY (OrderId) REFERENCES Orders(Id)   ON DELETE CASCADE,
    FOREIGN KEY (ItemId)  REFERENCES MenuItem(Id) ON DELETE CASCADE
);

-- ── Delivery ─────────────────────────────────────────────────
CREATE TABLE IF NOT EXISTS Delivery (
    Id       INT AUTO_INCREMENT PRIMARY KEY,
    OrderId  INT UNIQUE,
    DriverId INT,
    FOREIGN KEY (OrderId)  REFERENCES Orders(Id)        ON DELETE CASCADE,
    FOREIGN KEY (DriverId) REFERENCES DeliveryPerson(Id) ON DELETE SET NULL
);

-- ── Seed: default admin account (password: admin123) ─────────
INSERT IGNORE INTO Users (Name, Email, Password, Role)
    VALUES ('Admin','admin@food.com','admin123','Admin');

INSERT IGNORE INTO Admin (Id, Name, Email)
    SELECT Id, Name, Email FROM Users WHERE Email='admin@food.com';

-- ── Seed: sample restaurant & menu ───────────────────────────
INSERT IGNORE INTO Restaurant (Id, Name, AdminId)
    SELECT 1, 'Baghdad Bites', Id FROM Users WHERE Email='admin@food.com';

INSERT IGNORE INTO MenuItem (Name, Price, RestId) VALUES
    ('Grilled Chicken', 8500, 1),
    ('Beef Kebab',      9500, 1),
    ('Falafel Wrap',    5500, 1),
    ('Rice & Stew',     7000, 1),
    ('Lentil Soup',     3500, 1);
