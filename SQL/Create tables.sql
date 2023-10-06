CREATE TABLE VehicleBrands (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    IsNational BOOLEAN NOT NULL,
    Status BOOLEAN NOT NULL,
    BrandName VARCHAR(255) NOT NULL,
    CreatedBy INT,
    Updated TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    UpdateBy INT,
    IsDeleted BOOLEAN,
    DeletedBy INT,
    IsActive BOOLEAN
);

CREATE TABLE Users (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(255) NOT NULL,
    Email VARCHAR(255) NOT NULL,
    Password VARCHAR(255) NOT NULL,
    Updated TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    UpdateBy INT,
    IsDeleted BOOLEAN,
    DeletedBy INT,
    IsActive BOOLEAN
);

CREATE TABLE LogRegistration (
  Id INT PRIMARY KEY AUTO_INCREMENT,
  ChangeDate DATETIME,
  IdUser INT,
  ModifiedField VARCHAR(255),
  OldValue VARCHAR(255),
  NewValue VARCHAR(255)
);

CREATE TABLE Functionality (
  Id INT PRIMARY KEY AUTO_INCREMENT,
  IdUser INT not NULL,
  FunctionalityCode VARCHAR(255)
);

INSERT INTO Users (Name, IsActive, Email, Password)
VALUES
    ('User 1', true, 'user1@example.com', 'password1'),
    ('User 2', TRUE, 'user2@example.com', 'password2'),
    ('User 3', TRUE, 'user3@example.com', 'password3'),
    ('User 4', TRUE, 'user4@example.com', 'password4'),
    ('User 5', FALSE, 'user5@example.com', 'password5');

   
INSERT INTO Functionality (IdUser,FunctionalityCode)
VALUES
    (1, 'InsertVehicleBrand'),
   	(1, 'UpdateVehicleBrand'),
   	(2, 'UpdateVehicleBrand'),
   	(2, 'InsertVehicleBrand');
   
INSERT INTO VehicleBrands (IsNational, Status, BrandName, CreatedBy, IsActive)
VALUES
    (TRUE, TRUE, 'Brand 1', 1, TRUE),
    (FALSE, TRUE, 'Brand 2', 1, TRUE),
    (TRUE, TRUE, 'Brand 3', 2, TRUE),
    (TRUE, FALSE, 'Brand 4', 1, TRUE),
    (FALSE, FALSE, 'Brand 5', 2, TRUE);
   