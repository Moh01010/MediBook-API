using MediBook.Interface;
using MediBook.Models;

namespace MediBook.Repository
{
    public class NotificationService: INotificationService
    {
        private readonly INotificationRepository _repo;

        public NotificationService(INotificationRepository repo)
        {
            _repo = repo;
        }

        public async Task NotifyAsync(string userId, string title, string message)
        {
            var notification = new Notification
            {
                UserId = userId,
                Title = title,
                Message = message
            };

            await _repo.CreateAsync(notification);
        }
    }
}
