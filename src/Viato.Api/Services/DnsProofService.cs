using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DnsClient;
using Viato.Api.Entities;

namespace Viato.Api.Services
{
    public class DnsProofService : IDnsProofService
    {
        public async Task<bool> VerifyProofAsync(Organization organization)
        {
            var lookup = new LookupClient();
            var result = await lookup.QueryAsync(organization.Domain, QueryType.TXT);

            var record = result.Answers.TxtRecords().FirstOrDefault(s => s.Text.Any(t => t == $"viato={organization.Id}"));
            if (record != null)
            {
                return true;
            }

            return false;
        }
    }
}
