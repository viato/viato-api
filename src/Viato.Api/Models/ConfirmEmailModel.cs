using System.ComponentModel.DataAnnotations;

namespace Viato.Api.Models
{
    public class ConfirmEmailModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Code { get; set; }
    }
}
