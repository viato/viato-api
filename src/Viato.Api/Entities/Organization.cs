using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Viato.Api.Entities
{
    public class Organization : EntityBase
    {
        public string Name { get; set; }

        public string Descripiton { get; set; }

        public string LogoBlobId { get; set; }

        public string Website { get; set; }

        public OrganizationType Type { get; set; }

        public long AppUserId { get; set; }

        public virtual AppUser AppUser { get; set; }

        public virtual List<ContributionPipeline> ContributionPiplines { get; set; } = new List<ContributionPipeline>();
    }
}
