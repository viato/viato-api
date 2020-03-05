using System.Threading.Tasks;

namespace Viato.Api.Notification
{
    public interface IEmailSender
    {
        Task SendAsync(string email, string subject, string htmlMessage);

        Task SendAsync<T>(string email, string subject, string template, T model);
    }
}
