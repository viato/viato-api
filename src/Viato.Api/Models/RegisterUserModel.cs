using System.ComponentModel.DataAnnotations;
using Viato.Api.Entities;

namespace Viato.Api.Models
{
    public class RegisterUserModel
    {
        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public AppUserRole Role { get; set; }
    }
}
