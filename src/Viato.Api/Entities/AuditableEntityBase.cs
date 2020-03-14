using System;

namespace Viato.Api.Entities
{
    public abstract class AuditableEntityBase
    {
        public DateTimeOffset CreatedDate { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }
    }
}
