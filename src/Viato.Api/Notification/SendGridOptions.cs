using System.ComponentModel.DataAnnotations;

namespace Viato.Api.Notification
{
    public class SendGridOptions
    {
        [Required]
        public string FromEmail { get; set; }

        [Required]
        public string FromName { get; set; }

        [Required]
        public string ApiKey { get; set; }
    }
}
