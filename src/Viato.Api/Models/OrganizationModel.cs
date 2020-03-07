using Viato.Api.Entities;

namespace Viato.Api.Models
{
    public class OrganizationModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Descripiton { get; set; }

        public string LogoBlobId { get; set; }

        public string Website { get; set; }

        public OrganizationType Type { get; set; }
    }
}
