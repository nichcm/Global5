CREATE TABLE VehicleBrands (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    IsNational BIT NOT NULL,
    Status BIT NOT NULL,
    BrandName VARCHAR(255) NOT NULL,
    CreatedBy INT,
    Updated TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UpdateBy INT,
    IsDeleted BIT,
    DeletedBy INT,
    IsActive BIT
);

CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(255) NOT NULL,
    Email VARCHAR(255) NOT NULL,
    Password VARCHAR(255) NOT NULL,
    Updated TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UpdateBy INT,
    IsDeleted BIT,
    DeletedBy INT,
    IsActive BIT
);

CREATE TABLE LogRegistration (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ChangeDate DATETIME,
    IdUser INT,
    ModifiedField VARCHAR(255),
    OldValue VARCHAR(255),
    NewValue VARCHAR(255)
);

CREATE TABLE Functionality (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    IdUser INT NOT NULL,
    FunctionalityCode VARCHAR(255)
);

INSERT INTO Users (Name, IsActive, Email, Password)
VALUES
    ('User 1', 1, 'user1@example.com', 'password1'),
    ('User 2', 1, 'user2@example.com', 'password2'),
    ('User 3', 1, 'user3@example.com', 'password3'),
    ('User 4', 1, 'user4@example.com', 'password4'),
    ('User 5', 0, 'user5@example.com', 'password5');

INSERT INTO Functionality (IdUser, FunctionalityCode)
VALUES
    (1, 'Select'),
    (1, 'Insert'),
    (1, 'Update'),
    (1, 'Update'),
    (2, 'Select'),
    (2, 'Insert');

INSERT INTO VehicleBrands (IsNational, Status, BrandName, CreatedBy, IsActive)
VALUES
    (1, 1, 'Brand 1', 1, 1),
    (0, 1, 'Brand 2', 1, 1),
    (1, 1, 'Brand 3', 2, 1),
    (1, 0, 'Brand 4', 1, 1),
    (0, 0, 'Brand 5', 2, 1);
