--Using the database created
--Change database name
use testing;

--For creating the User Table
CREATE TABLE [User] (
  [User_Id] int PRIMARY KEY IDENTITY (1, 1) NOT NULL,
  [Employee_Id] int NOT NULL,
  [Email] nvarchar(255) NOT NULL,
  [Is_Admin] bit NOT NULL,
  [Is_Enabled] bit NOT NULL,
  [Created_At] datetime NOT NULL
)
GO

--For creating the HardwareInfo Table
CREATE TABLE [HardwareInfo] (
  [Hardware_Id] int PRIMARY KEY IDENTITY (1, 1) NOT NULL,
  [Name] nvarchar(100) NOT NULL,
  [Description] nvarchar(255) NOT NULL,
  [Brand] nvarchar(50) NOT NULL,
  [Serial_Number] nvarchar(50) NOT NULL,
  [Firmware_Version] nvarchar,
  [Status] nvarchar(50) NOT NULL,
  [Expiration_Date] datetime NOT NULL,
  [Installation_Date] datetime
)
GO

--For creating the License Table
CREATE TABLE [License] (
  [License_Id] int PRIMARY KEY IDENTITY (1, 1) NOT NULL,
  [Hardware_Id] int NOT NULL,
  [Name] nvarchar(50) NOT NULL,
  [Purchase_Date] datetime NOT NULL,
  [Expiration_Date] datetime NOT NULL,
  [Renewal_Price] float
)
GO

--For creating the Maintenance Table
CREATE TABLE [Maintenance] (
  [Maintenance_Id] int PRIMARY KEY IDENTITY (1, 1) NOT NULL,
  [User_Id] int NOT NULL,
  [Hardware_Id] int NOT NULL,
  [Title] nvarchar(50) NOT NULL,
  [Description] nvarchar(1000) NOT NULL,
  [Status] nvarchar(50) NOT NULL,
  [Cost] float,
  [Maintenance_Date] datetime NOT NULL,
  [Completion_Date] datetime
)
GO

--For creating the Part Table
CREATE TABLE [Part] (
  [Part_Id] int PRIMARY KEY IDENTITY (1, 1) NOT NULL,
  [Hardware_id] int NOT NULL,
  [Name] nvarchar(100) NOT NULL,
  [Cost] float NOT NULL,
  [Purchase_Date] datetime NOT NULL
)
GO

--For creating the Notifications Table
CREATE TABLE [Notifications] (
  [Notification_Id] int PRIMARY KEY NOT NULL,
  [Hardware_Id] int,
  [User_Id] int,
  [Is_Read] bit NOT NULL,
  [Message] nvarchar(255) NOT NULL,
  [Title] nvarchar(255) NOT NULL,
  [Created_At] datetime NOT NULL
)
GO

ALTER TABLE [License] ADD FOREIGN KEY ([Hardware_Id]) REFERENCES [HardwareInfo] ([Hardware_Id])
GO

ALTER TABLE [Maintenance] ADD FOREIGN KEY ([User_Id]) REFERENCES [User] ([User_Id])
GO

ALTER TABLE [Maintenance] ADD FOREIGN KEY ([Hardware_Id]) REFERENCES [HardwareInfo] ([Hardware_Id])
GO

ALTER TABLE [Part] ADD FOREIGN KEY ([Hardware_id]) REFERENCES [HardwareInfo] ([Hardware_Id])
GO

ALTER TABLE [Notifications] ADD FOREIGN KEY ([Hardware_Id]) REFERENCES [HardwareInfo] ([Hardware_Id])
GO

ALTER TABLE [Notifications] ADD FOREIGN KEY ([User_Id]) REFERENCES [User] ([User_Id])
GO

