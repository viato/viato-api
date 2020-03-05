using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Viato.Api.Entities
{
    public abstract class EntityBase
    {
        [Key]
        public long Id { get; set; }
    }
}
