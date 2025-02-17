using System.Threading.Tasks;

namespace Domain.Commons.Contract
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);
        Task SendBookingConfirmationAsync(string to, string flightNumber, DateTimeOffset departureTime);
    }

}
