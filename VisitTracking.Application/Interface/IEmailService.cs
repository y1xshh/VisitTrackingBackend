namespace VisitTracking.Application.Interface
{
    public interface IEmailService
    {
        Task SendEmail(string email, string subject, string body);
        Task SendEmailAsync(string toEmail, string subject, string body);
    }
}