using System.ComponentModel.DataAnnotations;

namespace Viato.Api.Entities
{
    public abstract class EntityBase<TKey> : AuditableEntityBase
    {
        [Key]
        public TKey Id { get; set; }
    }
}
