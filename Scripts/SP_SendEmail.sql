USE [LifecycleManagementDB]
GO

CREATE OR ALTER TRIGGER trg_Notification 
ON [dbo].Notifications
FOR INSERT
AS
BEGIN
    DECLARE @Subject NVARCHAR(255),
            @Body NVARCHAR(MAX),
            @Recipients NVARCHAR(255);

    DECLARE @Title NVARCHAR(255), @Message NVARCHAR(MAX), @NotificationId INT;

    SELECT @Title = Subject, @Message = Message, @NotificationId = NotificationId
    FROM inserted;

    SET @Subject = @Title;
    SET @Body = @Message;

    SELECT @Body = 'Alert #: ' + CAST(i.NotificationId as NVARCHAR) + 
                   '<h3>' + @Body + '</h3>' +
                   '<h4> Product Info: </h4>' + 
                   '<ul>' + 
                   CONCAT('<li>Product Code: ' , p.ProductCode, '</li>') +
                   CONCAT('<li>Product Name: ' , p.ProductName, '</li>') +
                   CONCAT('<li>Product Version: ' , p.ProductVersion, '</li>') +
                   CONCAT('<li>Product Category: ' , p.ProductCategory, '</li>') +
                   CONCAT('<li>Status: ' , p.Status, '</li>') +
                   CONCAT('<li>Manufacturer: ' , p.Manufacturer, '</li>') +
                   CONCAT('<li>Vendor Name: ' , p.VendorName, '</li>') +
                   CONCAT('<li>Purchase Order No: ' , p.PurchaseOrderNo, '</li>') +
                   CONCAT('<li>Assigned IT Custodian Id: ' , p.AssignedITCustodianId, '</li>') +
                   CONCAT('<li>EOS Date: ' , p.EOSDate, '</li>') +
                   CONCAT('<li>EOL Date: ' , p.EOLDate, '</li>') +
                   CONCAT('<li>Support Documentation URL: ' , p.SupportDocumentationURL, '</li>') +
                   CONCAT('<li>Support Email: ' , p.SupportEmail, '</li>') +
                   '</ul>' + 
                   '<h4> Custodian Info: </h4>' +
                   '<ul>' +
                   CONCAT('<li>Custodian Name: ' , c.FirstName, ' ', c.LastName, '</li>') +
                   CONCAT('<li>Custodian Role: ' , c.CustodianRole, '</li>') +
                   CONCAT('<li>Custodian Email: ' , c.Email, '</li>') +
                   '</ul>' + 
                   '<i> This is an automated message, please do not reply.</i>'
    FROM inserted i
    JOIN [dbo].Products p ON i.PurchaseOrderNo = p.PurchaseOrderNo
    JOIN [dbo].Custodians c ON i.ITCustodianId = c.CustodianId;

    SET @Recipients = 'faith.getulle27@gmail.com';

    EXEC [dbo].SP_SendEmail @Subject, @Body, @Recipients;
END
GO
