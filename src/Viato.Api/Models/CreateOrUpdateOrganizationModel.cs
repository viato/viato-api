using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
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
