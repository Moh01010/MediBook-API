using MediBook.Models;

namespace MediBook.Interface
{
    public interface INotificationRepository
    {
        Task CreateAsync(Notification notification);
        Task<IEnumerable<Notification>> GetUserNotificationsAsync(string userId);
        Task MarkAsReadAsync(int notificationId);
    }
}
