using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Viato.Api.Notification
{
    public interface IEmailSender
    {
        Task SendAsync(string email, string subject, string htmlMessage);
    }
}
