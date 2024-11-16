using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TimeBasedPreventiveMeasures.Data;
using TimeBasedPreventiveMeasures.Models;
using TimeBasedPreventiveMeasures.Models.Data;

namespace TimeBasedPreventiveMeasures.Services
{
    public class NotificationService : INotificationService
    {
        private readonly LifecycleManagementDB _context;
        private readonly ILogger<NotificationService> _logger;
        private readonly EmailSender _emailSender;

        public NotificationService(LifecycleManagementDB context, ILogger<NotificationService> logger, EmailSender emailSender)
        {
            _context = context;
            _logger = logger;
            _emailSender = emailSender;
        }

        public async Task GenerateNotificationsAsync()
        {
            try
            {
                var currentDate = DateTime.UtcNow.Date;
                _logger.LogInformation("Notification generation started at {Time}", currentDate);

                var products = await _context.Products.ToListAsync();

                foreach (var product in products)
                {
                    await GenerateEOSNotificationForProductAsync(product, currentDate);
                    await GenerateEOLNotificationForProductAsync(product, currentDate);
                }

                await _context.SaveChangesAsync();
                _logger.LogInformation("Notification generation completed at {Time}", DateTime.UtcNow);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while generating notifications.");
                throw;
            }
        }

        public async Task GenerateEOSNotificationForProductAsync(Product product, DateTime currentDate)
        {
            if (product.EOSDate.HasValue && product.EOSDate.Value.Date == currentDate.AddYears(2))
            {
                var notification = new Notification
                {
                    Subject = "EOS Date Approaching",
                    Message = $@"The product {product.ProductName} with code {product.ProductCode} is approaching EOS date. <br/><br/><br/>

                            - Product Name: {product.ProductName} <br/>
                            - Product Code: {product.ProductCode} <br/>
                            - Product Version: {product.ProductVersion} <br/>
                            - Product Category: {product.ProductCategory} <br/>
                            - Status: {product.Status} <br/>
                            - Manufacturer: {product.Manufacturer} <br/>
                            - Vendor Name: {product.VendorName} <br/>
                            - Purchase Order No: {product.PurchaseOrderNo} <br/>
                            - EOS Date: {product.EOSDate?.ToString("MMM dd yyyy hh:mmtt")} <br/>
                            - EOL Date: {product.EOLDate?.ToString("MMM dd yyyy hh:mmtt")} <br/>
                            - Support Documentation URL: {product.SupportDocumentationURL} <br/>
                            - Support Email: {product.SupportEmail} <br/><br/>

                            This is an automated message, please do not reply.",
                    ITCustodianId = product.AssignedITCustodianId,
                    PurchaseOrderNo = product.PurchaseOrderNo,
                    CreatedAt = currentDate,
                    IsRead = false
                };

                await _context.Notifications.AddAsync(notification);
                _logger.LogInformation("EOS notification generated for product {ProductName}.", product.ProductName);
                await SendEmailAsync(notification);
            }
        }

        public async Task GenerateEOLNotificationForProductAsync(Product product, DateTime currentDate)
        {
            if (product.EOLDate.HasValue && product.EOLDate.Value.Date == currentDate.AddYears(1))
            {
                var notification = new Notification
                {
                    Subject = "EOL Date Approaching",
                    Message = $@"The product {product.ProductName} with code {product.ProductCode} is approaching EOS date. <br/><br/><br/>

                            - Product Name: {product.ProductName} <br/>
                            - Product Code: {product.ProductCode} <br/>
                            - Product Version: {product.ProductVersion} <br/>
                            - Product Category: {product.ProductCategory} <br/>
                            - Status: {product.Status} <br/>
                            - Manufacturer: {product.Manufacturer} <br/>
                            - Vendor Name: {product.VendorName} <br/>
                            - Purchase Order No: {product.PurchaseOrderNo} <br/>
                            - EOS Date: {product.EOSDate?.ToString("MMM dd yyyy hh:mmtt")} <br/>
                            - EOL Date: {product.EOLDate?.ToString("MMM dd yyyy hh:mmtt")} <br/>
                            - Support Documentation URL: {product.SupportDocumentationURL} <br/>
                            - Support Email: {product.SupportEmail} <br/><br/>

                            This is an automated message, please do not reply.",
                    ITCustodianId = product.AssignedITCustodianId,
                    PurchaseOrderNo = product.PurchaseOrderNo,
                    CreatedAt = currentDate,
                    IsRead = false
                };

                await _context.Notifications.AddAsync(notification);
                _logger.LogInformation("EOL notification generated for product {ProductName}.", product.ProductName);
                await SendEmailAsync(notification);
            }
        }

        private async Task SendEmailAsync(Notification notification)
        {
            var custodian = await _context.Custodians.FindAsync(notification.ITCustodianId);
            if (custodian != null)
            {
                await _emailSender.SendEmailAsync(custodian.Email, notification.Subject, notification.Message);
            }
        }
    }
}
