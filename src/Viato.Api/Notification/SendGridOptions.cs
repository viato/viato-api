using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
