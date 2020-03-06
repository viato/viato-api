using System.ComponentModel.DataAnnotations;

namespace Viato.Api.Entities
{
    public abstract class EntityBase
    {
        [Key]
        public long Id { get; set; }
    }
}
