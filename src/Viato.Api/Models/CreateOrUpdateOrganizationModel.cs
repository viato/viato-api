using System.ComponentModel.DataAnnotations;
using Viato.Api.Entities;

namespace Viato.Api.Models
{
    public class CreateOrUpdateOrganizationModel
    {
        [Required]
        public string Name { get; set; }

        public OrganizationType? OrganizationType { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string Descripiton { get; set; }

        [Required]
        [Url]
        public string Website { get; set; }
    }
}
