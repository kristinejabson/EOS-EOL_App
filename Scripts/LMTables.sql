USE LifecycleManagementDB
GO

IF NOT EXISTS (SELECT * FROM [sys].[tables] WHERE name = N'Custodians' AND [schema_id] = SCHEMA_ID('dbo'))
BEGIN 
	CREATE TABLE [LifecycleManagementDB].[dbo].[Custodians](
		[CustodianId] INT PRIMARY KEY IDENTITY(1, 1),
		[FirstName] NVARCHAR(255),
		[LastName] NVARCHAR(255),
		[CustodianRole] NVARCHAR(255),
		[Email] NVARCHAR(100),
	);
END

-- Status: Active / No Action Plan / Decomissioned
IF NOT EXISTS (SELECT * FROM [sys].[tables] WHERE name = N'Products' AND [schema_id] = SCHEMA_ID('dbo'))
BEGIN
	CREATE TABLE [LifecycleManagementDB].[dbo].[Products] (
		[ProductId] INT PRIMARY KEY IDENTITY(1, 1),
		[ProductCode] VARCHAR(255) UNIQUE NOT NULL,
		[ProductName] VARCHAR(255) NOT NULL,
		[ProductVersion] VARCHAR(255) NOT NULL,
		[ProductCategory] VARCHAR(255) NOT NULL,
		[Status] VARCHAR(255) NOT NULL,
		[Manufacturer] VARCHAR(255) NOT NULL,
		[VendorName] VARCHAR(255) NOT NULL,
		[PurchaseOrderNo] VARCHAR(255) NOT NULL,
		[AssignedITCustodianId] INT FOREIGN KEY REFERENCES [LifecycleManagementDB].[dbo].[Custodians]([CustodianId]),
		[EOSDate] DATETIME,
		[EOLDate] DATETIME,
		[EOESDate] DATETIME,
		[SupportDocumentationURL] VARCHAR(255),
		[SupportEmail] VARCHAR(255)
	);
END

IF NOT EXISTS (SELECT * FROM [sys].[tables] WHERE name = N'ActionPlans' AND [schema_id] = SCHEMA_ID('dbo'))
BEGIN 
	CREATE TABLE [dbo].[ActionPlans](
		[ActionPlanId] INT PRIMARY KEY IDENTITY(1, 1),
		[ProductId] INT FOREIGN KEY REFERENCES [dbo].[Products]([ProductId]),
		[Title] VARCHAR(255),
		[Description] VARCHAR(MAX),
		[AssignedToCustodianId] INT FOREIGN KEY REFERENCES [LifecycleManagementDB].[dbo].[Custodians]([CustodianId]),
		[StartDate] DATETIME,
		[EndDate] DATETIME,
		[Status] VARCHAR(255),
		[Reminder] VARCHAR(MAX),
	);
END

IF NOT EXISTS (SELECT * FROM [sys].[tables] WHERE name = N'Notifications' AND [schema_id] = SCHEMA_ID('dbo'))
BEGIN
	CREATE TABLE [dbo].[Notifications](
		[NotificationId] INT PRIMARY KEY IDENTITY(1, 1),
		[Subject] NVARCHAR(255),
		[Message] NVARCHAR(MAX),
		[ITCustodianId] INT FOREIGN KEY REFERENCES [dbo].[Custodians]([CustodianId]),
		[PurchaseOrderNo] VARCHAR(255),
		[CreatedAt] DATETIME,
		[IsRead] BIT
	);
END