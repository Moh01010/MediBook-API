namespace MediBook.Interface
{
    public interface INotificationService
    {
        Task NotifyAsync(string userId, string title, string message);
    }
}
