using System.Threading.Tasks;

namespace Viato.Api.Notification
{
    public interface IEmailTemplateRenderer
    {
        Task<string> RenderAsync<T>(string templateName, T model);
    }
}
