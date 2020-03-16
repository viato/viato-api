using System.ComponentModel.DataAnnotations;

namespace Viato.Api.Models
{
    public class ScanTorRequestModel
    {
        [Required]
        public string TorToken { get; set; }
    }
}
