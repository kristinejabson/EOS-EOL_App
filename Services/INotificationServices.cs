using System;
using System.Threading.Tasks;
using TimeBasedPreventiveMeasures.Models.Data;

namespace TimeBasedPreventiveMeasures.Services
{
    public interface INotificationService
    {
        Task GenerateNotificationsAsync();
        Task GenerateEOSNotificationForProductAsync(Product product, DateTime currentDate);
        Task GenerateEOLNotificationForProductAsync(Product product, DateTime currentDate);
    }
}