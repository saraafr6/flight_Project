using Domain.Commons.Contract;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

public class EmailService : IEmailService
{
    private readonly string _smtpServer;
    private readonly string _smtpUser;
    private readonly string _smtpPassword;
    private readonly int _smtpPort;

    public EmailService(string smtpServer, string smtpUser, string smtpPassword, int smtpPort)
    {
        _smtpServer = smtpServer;
        _smtpUser = smtpUser;
        _smtpPassword = smtpPassword;
        _smtpPort = smtpPort;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var mailMessage = new MailMessage
        {
            From = new MailAddress(_smtpUser),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        mailMessage.To.Add(to);

        using (var smtpClient = new SmtpClient(_smtpServer, _smtpPort))
        {
            smtpClient.Credentials = new NetworkCredential(_smtpUser, _smtpPassword);
            smtpClient.EnableSsl = true;
            await smtpClient.SendMailAsync(mailMessage);
        }
    }

    public async Task SendBookingConfirmationAsync(string to, string flightNumber, DateTimeOffset departureTime)
    {
        var message = $"Your flight {flightNumber} is confirmed. Departure time: {departureTime}.";
        await SendEmailAsync(to, "Flight Booking Confirmation", message);
    }

}
