using System.Net.Mail;
using System.Net;

public class EmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var email = _config.GetSection("Email");

        var smtp = new SmtpClient(email["SmtpHost"], int.Parse(email["SmtpPort"]))
        {
            Credentials = new NetworkCredential(
                email["SmtpUser"],
                email["SmtpPass"]
            ),
            EnableSsl = true,
            UseDefaultCredentials = false,
            DeliveryMethod = SmtpDeliveryMethod.Network
        };

        var message = new MailMessage
        {
            From = new MailAddress(email["FromEmail"], "Supplements App"),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        message.To.Add(to);

        await smtp.SendMailAsync(message);
    }
}
