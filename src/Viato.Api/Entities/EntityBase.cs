using System;
using System.ComponentModel.DataAnnotations;

namespace Viato.Api.Entities
{
    public abstract class EntityBase
    {
        [Key]
        public long Id { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public DateTimeOffset? UpdatedDate { get; set; }
    }
}
