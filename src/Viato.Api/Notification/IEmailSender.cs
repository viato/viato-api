using System.Threading.Tasks;

namespace Viato.Api.Notification
{
    public interface IEmailSender
    {
        Task SendAsync(string email, string subject, string htmlMessage);
    }
}
