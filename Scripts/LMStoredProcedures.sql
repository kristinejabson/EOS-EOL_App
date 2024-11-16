USE LifecycleManagementDB
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jacob Del Rosario
-- Create date: July 13, 2024
-- Description:	Add product to the table
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[SP_InsertProduct]
	@ProductName VARCHAR(255),
	@ProductCode VARCHAR(255),
	@ProductVersion VARCHAR(255),
	@ProductCategory VARCHAR(255),
	@Status VARCHAR(255),
	@Manufacturer VARCHAR(255),
	@VendorName VARCHAR(255),
	@PurchaseOrderNo VARCHAR(255),
	@AssignedITCustodianId INT,
	@EOSDate DATETIME = NULL,
	@EOLDate DATETIME = NULL,
	@EOESDate DATETIME = NULL,
	@SupportDocumentationURL VARCHAR(255) = NULL,
	@SupportEmail VARCHAR(255) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	IF EXISTS (SELECT 1 FROM [dbo].[Products] WHERE ProductCode = @ProductCode) 
	BEGIN
		RAISERROR ('Product code already exists', 16, 1);
		RETURN;
	END
	ELSE
	BEGIN
		INSERT INTO [dbo].[Products] (
			[ProductName], 
			[ProductCode],
			[ProductVersion], 
			[ProductCategory], 
			[Status], 
			[Manufacturer],
			[VendorName],
			[PurchaseOrderNo],
			[AssignedITCustodianId],
			[EOSDate],
			[EOLDate],
			[EOESDate],
			[SupportDocumentationURL],
			[SupportEmail]
		)
		VALUES
		(
			@ProductName,
			@ProductCode,
			@ProductVersion,
			@ProductCategory,
			@Status,
			@Manufacturer,
			@VendorName,
			@PurchaseOrderNo,
			@AssignedITCustodianId,
			@EOSDate,
			@EOLDate,
			@EOESDate,
			@SupportDocumentationURL,
			@SupportEmail
		);
		SELECT * FROM [dbo].[Products] WHERE ProductCode = @ProductCode;
	END
END
