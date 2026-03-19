IF DB_ID(N'FishermanBoardDb') IS NULL
BEGIN
    CREATE DATABASE FishermanBoardDb;
END
GO

USE FishermanBoardDb;
GO

IF OBJECT_ID(N'dbo.Fisherman', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Fisherman
    (
        Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        FullName NVARCHAR(120) NOT NULL,
        LicenseNumber NVARCHAR(30) NOT NULL,
        HomePort NVARCHAR(100) NOT NULL,
        PhoneNumber NVARCHAR(30) NOT NULL,
        CONSTRAINT UQ_Fisherman_LicenseNumber UNIQUE (LicenseNumber)
    );
END
GO

IF OBJECT_ID(N'dbo.Boats', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Boats
    (
        Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        Name NVARCHAR(80) NOT NULL,
        RegistrationNumber NVARCHAR(30) NOT NULL,
        LengthMeters DECIMAL(6,2) NOT NULL,
        EngineModel NVARCHAR(80) NOT NULL,
        FuelConsumptionPerHour DECIMAL(8,2) NOT NULL,
        FishermanId INT NOT NULL,
        CONSTRAINT UQ_Boats_RegistrationNumber UNIQUE (RegistrationNumber),
        CONSTRAINT FK_Boats_Fisherman FOREIGN KEY (FishermanId) REFERENCES dbo.Fisherman (Id)
    );
END
GO

IF OBJECT_ID(N'dbo.Hunt', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Hunt
    (
        Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        FishermanId INT NOT NULL,
        BoatId INT NOT NULL,
        StartedAt DATETIME2 NOT NULL,
        EndedAt DATETIME2 NOT NULL,
        FishType NVARCHAR(80) NOT NULL,
        QuantityKg DECIMAL(10,2) NOT NULL,
        FishingArea NVARCHAR(120) NOT NULL,
        CONSTRAINT FK_Hunt_Fisherman FOREIGN KEY (FishermanId) REFERENCES dbo.Fisherman (Id),
        CONSTRAINT FK_Hunt_Boats FOREIGN KEY (BoatId) REFERENCES dbo.Boats (Id)
    );
END
GO
