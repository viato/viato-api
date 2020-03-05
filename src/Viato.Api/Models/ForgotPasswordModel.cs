using System.ComponentModel.DataAnnotations;

namespace Viato.Api.Models
{
    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
