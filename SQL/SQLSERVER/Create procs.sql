CREATE PROCEDURE SelectUserByEmail
    @p_email VARCHAR(255)
AS
BEGIN
    SELECT Id, Name, Email, Password
    FROM Users
    WHERE Email = @p_email;
END;

CREATE PROCEDURE InsertVehicleBrand
    @p_IsNational BIT,
    @p_Status BIT,
    @p_BrandName VARCHAR(255),
    @p_CreatedBy INT
AS
BEGIN
    INSERT INTO VehicleBrands (IsNational, Status, BrandName, CreatedBy)
    VALUES (@p_IsNational, @p_Status, @p_BrandName, @p_CreatedBy);
    
    SELECT SCOPE_IDENTITY() AS NewBrandId;
END;

CREATE PROCEDURE SelectVehicleBrand
    @p_National BIT,
    @p_Name VARCHAR(255),
    @p_Active BIT,
    @p_pageSize INT,
    @p_pageNumber INT
AS
BEGIN
    DECLARE @offset INT;
    SET @offset = (@p_pageNumber - 1) * @p_pageSize;

    SELECT *
    FROM VehicleBrands
    WHERE (@p_National IS NULL OR IsNational = @p_National)
        AND (@p_Name IS NULL OR BrandName = @p_Name)
        AND (@p_Active IS NULL OR IsActive = @p_Active)
        AND Status = 1
    OFFSET @offset ROWS FETCH NEXT @p_pageSize ROWS ONLY;
END;

CREATE PROCEDURE SelectVehicleBrandById
    @p_scheduleFileId INT
AS
BEGIN
    SELECT *
    FROM VehicleBrands
    WHERE Id = @p_scheduleFileId;
END;

CREATE PROCEDURE UpdateVehicleBrand
    @p_Id INT,
    @p_IsNational BIT,
    @p_Status BIT,
    @p_BrandName VARCHAR(255),
    @p_UpdatedBy INT
AS
BEGIN
    UPDATE VehicleBrands
    SET
        IsNational = @p_IsNational,
        Status = @p_Status,
        BrandName = @p_BrandName,
        UpdatedBy = @p_UpdatedBy
    WHERE Id = @p_Id;
END;

CREATE PROCEDURE SelectVehicleBrandByName
    @p_BrandName VARCHAR(255)
AS
BEGIN
    SELECT *
    FROM VehicleBrands
    WHERE BrandName = @p_BrandName;
END;

CREATE PROCEDURE ToggleVehicleBrandActiveStatus
    @p_Id INT
AS
BEGIN
    DECLARE @currentActiveStatus BIT;

    SELECT @currentActiveStatus = IsActive
    FROM VehicleBrands
    WHERE Id = @p_Id;

    SET @currentActiveStatus = ~@currentActiveStatus;

    UPDATE VehicleBrands
    SET IsActive = @currentActiveStatus
    WHERE Id = @p_Id;
END;

CREATE PROCEDURE InsertLogRegistration
    @p_ChangeDate DATETIME,
    @p_IdUser INT,
    @p_ModifiedField VARCHAR(255),
    @p_OldValue VARCHAR(255),
    @p_NewValue VARCHAR(255)
AS
BEGIN
    INSERT INTO LogRegistration (ChangeDate, IdUser, ModifiedField, OldValue, NewValue)
    VALUES (@p_ChangeDate, @p_IdUser, @p_ModifiedField, @p_OldValue, @p_NewValue);
END;

CREATE PROCEDURE CheckFunctionalityByUserIdAndCode
    @p_IdUser INT,
    @p_FunctionalityCode VARCHAR(255),
    @p_Exists BIT OUTPUT
AS
BEGIN
    DECLARE @v_Count INT;

    SELECT @v_Count = COUNT(*)
    FROM Functionality
    WHERE IdUser = @p_IdUser AND FunctionalityCode = @p_FunctionalityCode;

    IF @v_Count > 0
        SET @p_Exists = 1;
    ELSE
        SET @p_Exists = 0;
END;
