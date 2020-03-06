using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Viato.Api.Models
{
    public class SetTwoFactorModel
    {
        public bool Enabled { get; set; }

        [Required]
        public string Code { get; set; }
    }
}
