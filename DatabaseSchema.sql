CREATE DATABASE BloodBankDb;
GO

USE BloodBankDb;
GO

-- Employee Table for Login
CREATE TABLE EmployeeTbl (
    EmpNum INT IDENTITY(1,1) PRIMARY KEY,
    EmpId VARCHAR(50) NOT NULL,
    EmpPass VARCHAR(50) NOT NULL
);
GO

INSERT INTO EmployeeTbl (EmpId, EmpPass) VALUES ('Admin', 'Admin');
GO

-- Donor Table
CREATE TABLE DonorTbl (
    DNum INT IDENTITY(1,1) PRIMARY KEY,
    DName VARCHAR(50) NOT NULL,
    DAge INT NOT NULL,
    DGen VARCHAR(10) NOT NULL,
    DPhone VARCHAR(20) NOT NULL,
    DAddress VARCHAR(100) NOT NULL,
    DBGroup VARCHAR(5) NOT NULL,
    DDate DATE DEFAULT GETDATE()
);
GO

-- Patient Table
CREATE TABLE PatientTbl (
    PNum INT IDENTITY(1,1) PRIMARY KEY,
    PName VARCHAR(50) NOT NULL,
    PAge INT NOT NULL,
    PGen VARCHAR(10) NOT NULL,
    PPhone VARCHAR(20) NOT NULL,
    PAddress VARCHAR(100) NOT NULL,
    PBGroup VARCHAR(5) NOT NULL,
    PDate DATE DEFAULT GETDATE()
);
GO

-- Blood Stock Table
CREATE TABLE BloodTbl (
    BGroup VARCHAR(5) PRIMARY KEY,
    BStock INT NOT NULL DEFAULT 0
);
GO

INSERT INTO BloodTbl (BGroup, BStock) VALUES
('A+', 0),
('O+', 0),
('B+', 0),
('AB+', 0),
('A-', 0),
('O-', 0),
('B-', 0),
('AB-', 0);
GO

-- Blood Transfer Table
CREATE TABLE TransferTbl (
    TNum INT IDENTITY(1,1) PRIMARY KEY,
    PName VARCHAR(50) NOT NULL,
    BGroup VARCHAR(5) NOT NULL,
    TDate DATE DEFAULT GETDATE()
);
GO

-- =============================================================
-- If you have an EXISTING database, run these ALTER statements
-- to add the date columns (skip if running fresh):
-- =============================================================
ALTER TABLE DonorTbl ADD DDate DATE DEFAULT GETDATE();
ALTER TABLE PatientTbl ADD PDate DATE DEFAULT GETDATE();
ALTER TABLE TransferTbl ADD TDate DATE DEFAULT GETDATE();
GO

-- Fill existing rows with demo dates if they are NULL
UPDATE DonorTbl SET DDate = DATEADD(DAY, -ABS(CHECKSUM(NEWID())) % 365, GETDATE()) WHERE DDate IS NULL;
UPDATE PatientTbl SET PDate = DATEADD(DAY, -ABS(CHECKSUM(NEWID())) % 365, GETDATE()) WHERE PDate IS NULL;
UPDATE TransferTbl SET TDate = DATEADD(DAY, -ABS(CHECKSUM(NEWID())) % 365, GETDATE()) WHERE TDate IS NULL;
GO
