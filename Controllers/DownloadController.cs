using Microsoft.AspNetCore.Mvc;
namespace react_weatherapp.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DownloadApplication : Controller
    {
        private readonly IDownloadApiService _downloadApiService;
        public DownloadApplication(IDownloadApiService downloadApiService)
        {
            _downloadApiService = downloadApiService;
        }
        
        [HttpGet("{containerName}/{blobName}")]
        public async Task<FileStreamResult> DownloadFile(string containerName, string blobName)
        {
            string downloadPath = @"C:\Downloads\" + blobName; // change this to the path where you want to download the file
            await _downloadApiService.DownloadFileAsync(containerName, blobName, downloadPath);

            // return the file as a stream
            var stream = new FileStream(downloadPath, FileMode.Open);
            return new FileStreamResult(stream, "application/octet-stream")
            {
                FileDownloadName = blobName
            };
        }

    }
}