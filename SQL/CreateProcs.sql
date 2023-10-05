CREATE PROCEDURE `Global5`.`SelectUserByEmail`(IN p_email VARCHAR(255))
BEGIN
    SELECT Id, Name, Email, Password
    FROM Users
    WHERE Email = p_email;
end


CREATE PROCEDURE InsertVehicleBrand(
    IN p_IsNational BOOLEAN,
    IN p_Status BOOLEAN,
    IN p_BrandName VARCHAR(255),
    IN p_CreatedBy INT
)
BEGIN
    INSERT INTO VehicleBrands (IsNational, Status, BrandName, CreatedBy)
    VALUES (p_IsNational, p_Status, p_BrandName, p_CreatedBy);
    
    SELECT LAST_INSERT_ID() AS NewBrandId; -- Retorna o ID da marca recém-inserida
end

CREATE PROCEDURE SelectVehicleBrand(
    IN p_National BOOLEAN,
    IN p_Name VARCHAR(255),
    IN p_Active BOOLEAN,
    IN p_pageSize INT,
    IN p_pageNumber INT
)
BEGIN
    DECLARE offset INT;
    SET offset = (p_pageNumber - 1) * p_pageSize;

    SELECT *
    FROM VehicleBrands
    WHERE (@p_National IS NULL OR IsNational = @p_National)
        AND (@p_Name IS NULL OR BrandName = @p_Name)
        AND (@p_Active IS NULL OR IsActive = @p_Active)
        AND Status = 1
    LIMIT p_pageSize OFFSET offset;
END

CREATE PROCEDURE SelectVehicleBrandById(IN p_scheduleFileId INT)
BEGIN
    SELECT *
    FROM VehicleBrands
    WHERE Id = p_scheduleFileId;
end

CREATE PROCEDURE UpdateVehicleBrand(
    IN p_Id INT,
    IN p_IsNational BOOLEAN,
    IN p_Status BOOLEAN,
    IN p_BrandName VARCHAR(255),
    IN p_UpdatedBy INT
)
BEGIN
    UPDATE VehicleBrands
    SET
        IsNational = p_IsNational,
        Status = p_Status,
        BrandName = p_BrandName,
        UpdatedBy = p_UpdatedBy
    WHERE Id = p_Id;
END 

CREATE PROCEDURE SelectVehicleBrandByName(
    IN p_BrandName VARCHAR(255)
)
BEGIN
    SELECT *
    FROM VehicleBrands
    WHERE BrandName = p_BrandName;
END

CREATE PROCEDURE ToggleVehicleBrandActiveStatus(
    IN p_Id INT
)
BEGIN
    DECLARE currentActiveStatus BOOLEAN;

    -- Obtém o valor atual do campo IsActive
    SELECT IsActive INTO currentActiveStatus
    FROM VehicleBrands
    WHERE Id = p_Id;

    -- Inverte o valor do campo IsActive
    SET currentActiveStatus = NOT currentActiveStatus;

    -- Atualiza o campo IsActive
    UPDATE VehicleBrands
    SET IsActive = currentActiveStatus
    WHERE Id = p_Id;
END

CREATE PROCEDURE InsertLogRegistration(
    IN p_ChangeDate DATETIME,
    IN p_IdUser int,
    IN p_ModifiedField VARCHAR(255),
    IN p_OldValue VARCHAR(255),
    IN p_NewValue VARCHAR(255)
)
BEGIN
    INSERT INTO LogRegistration (ChangeDate, IdUser, ModifiedField, OldValue, NewValue)
    VALUES (p_ChangeDate, p_IdUser, p_ModifiedField, p_OldValue, p_NewValue);
END


CREATE PROCEDURE CheckFunctionalityByUserIdAndCode(
    IN p_IdUser INT,
    IN p_FunctionalityCode VARCHAR(255),
    OUT p_Exists BOOLEAN
)
BEGIN
    DECLARE v_Count INT;

    SELECT COUNT(*) INTO v_Count
    FROM Functionality
    WHERE IdUser = p_IdUser AND FunctionalityCode = p_FunctionalityCode;

    IF v_Count > 0 THEN
        SET p_Exists = TRUE;
    ELSE
        SET p_Exists = FALSE;
    END IF;
end


