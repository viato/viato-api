﻿using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;
using Viato.Api.Entities;

namespace Viato.Api.Services
{
    public class BlobService : IBlobService
    {
        private readonly CloudBlobClient _cloudBlobClient;

        public BlobService(CloudBlobClient cloudBlobClient)
        {
            _cloudBlobClient = cloudBlobClient ?? throw new ArgumentNullException(nameof(cloudBlobClient));
        }

        public async Task UpdateOrganizationLogoAsync(Organization organization, Stream stream, string logoExtension)
        {
            var container = _cloudBlobClient.GetContainerReference("organizationlogos");
            await container.CreateIfNotExistsAsync();
            await container.SetPermissionsAsync(new BlobContainerPermissions()
            {
                PublicAccess = BlobContainerPublicAccessType.Blob,
            });

            var logoBlob = container.GetBlockBlobReference($"logo_{organization.Id}{logoExtension}");

            await logoBlob.UploadFromStreamAsync(stream);
            organization.LogoBlobUri = logoBlob.Uri.AbsoluteUri;
        }
    }
}
