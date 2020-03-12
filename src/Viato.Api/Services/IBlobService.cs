using System;
using System.IO;
using System.Threading.Tasks;
using Viato.Api.Entities;

namespace Viato.Api.Services
{
    public interface IBlobService
    {
        Task<Uri> UploadOrganizationLogoAsync(Organization organization, Stream stream, string logoExtension);
    }
}
