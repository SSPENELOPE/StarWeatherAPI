using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.IO;
using System.Threading.Tasks;
using react_weatherapp.Models;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;

namespace react_weatherapp.Controllers
{
    public interface IDownloadApiService
    {
        Task DownloadFileAsync(string containerName, string blobName, string downloadPath);
    }

    public class DownloadApiService : IDownloadApiService
    {
        private readonly IOptions<StorageKey> _storagekey;
        public DownloadApiService(IOptions<StorageKey> storagekey)
        {
            _storagekey = storagekey;
        }

        public async Task DownloadFileAsync(string containerName, string blobName, string downloadPath)
        {
            await Task.Run(() =>
            {
                // Create directory if it does not exist
                 Directory.CreateDirectory(Path.GetDirectoryName(downloadPath));

                // Retrieve the storage account from the connection string.
                BlobServiceClient blobServiceClient = new BlobServiceClient(_storagekey.Value.ConnString);
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
                BlobClient blobClient = containerClient.GetBlobClient(blobName);
                blobClient.DownloadTo(downloadPath);
            });
        }
    }
}