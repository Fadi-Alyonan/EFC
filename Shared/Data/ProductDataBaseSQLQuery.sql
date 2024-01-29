DROP TABLE Products
DROP TABLE Prices
DROP TABLE ProductionInformation
DROP TABLE Manufacturers
DROP TABLE Categories


CREATE TABLE Categories (
    CategoryID INT PRIMARY KEY IDENTITY,
    CategoryName NVARCHAR(200) not null,
);

CREATE TABLE Manufacturers (
    ManufacturerID INT PRIMARY KEY IDENTITY,
    ManufacturerName NVARCHAR(150) not null,
    
);

CREATE TABLE ProductionInformation (
    ProductionID INT PRIMARY KEY IDENTITY,
    ProductionDate DATE DEFAULT GETDATE(),
);

CREATE TABLE Prices (
    PriceID INT PRIMARY KEY IDENTITY,
    ProductPrice MONEY not null,
    PriceDate DATE DEFAULT GETDATE(),
);

CREATE TABLE Products (
    ProductID UNIQUEIDENTIFIER PRIMARY KEY,
    ProductName NVARCHAR(200) not null,
    ProductDescription NVARCHAR(MAX) not null,
    QuantityInStock INT not null,
    CategoryID INT,
    ManufacturerID INT,
    ProductionID INT,
    PriceID INT,
    FOREIGN KEY (CategoryID) REFERENCES Categories(CategoryID),
    FOREIGN KEY (ManufacturerID) REFERENCES Manufacturers(ManufacturerID),
    FOREIGN KEY (ProductionID) REFERENCES ProductionInformation(ProductionID),
    FOREIGN KEY (PriceID) REFERENCES Prices(PriceID),
);