using System.ComponentModel.DataAnnotations;

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

        public string StagedContributionId { get; set; }
    }
}
