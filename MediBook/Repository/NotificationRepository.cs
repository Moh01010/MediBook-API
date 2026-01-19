using MediBook.Data;
using MediBook.Interface;
using MediBook.Models;
using Microsoft.EntityFrameworkCore;

namespace MediBook.Repository
{
    public class NotificationRepository: INotificationRepository
    {
        private readonly MediBookContext _context;

        public NotificationRepository(MediBookContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Notification notification)
        {
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Notification>> GetUserNotificationsAsync(string userId)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task MarkAsReadAsync(int notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);
            if (notification == null) return;

            notification.IsRead = true;
            await _context.SaveChangesAsync();
        }
    }
}
