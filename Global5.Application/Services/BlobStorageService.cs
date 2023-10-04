using Global5.Application.Interfaces;
using Global5.Application.Services.Base;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace Global5.Application.Services
{
    public class BlobStorageService : BaseService, IBlobStorageService
    {
        private readonly BlobContainerClient _account;
        private readonly IConfiguration _configuration;
        public BlobStorageService
       (
           IConfiguration configuration,
           ITranslateService translateService) : base(translateService)
        {
            _configuration = configuration;
            _account = new BlobContainerClient(_configuration.GetSection("StorageBlobSettings").GetSection("ConnectionString").Value, _configuration.GetSection("StorageBlobSettings").GetSection("ContainerName").Value);
        }
        public async Task Upload(string blobUrl, byte[] file)
        {
            BlobClient blobClient = _account.GetBlobClient(blobUrl);

            BinaryData binaryData = new BinaryData(file);

            await blobClient.UploadAsync(binaryData, true);
        }
        public async Task<string> Download(string blobUrl)
        {
            BlobClient blobClient = _account.GetBlobClient(blobUrl);

            BlobDownloadResult downloadResult = await blobClient.DownloadContentAsync();

            return downloadResult.Content.ToString();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}