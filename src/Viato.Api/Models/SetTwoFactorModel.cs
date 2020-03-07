using System.ComponentModel.DataAnnotations;

namespace Viato.Api.Models
{
    public class SetTwoFactorModel
    {
        public bool Enabled { get; set; }

        [Required]
        public string Code { get; set; }
    }
}
