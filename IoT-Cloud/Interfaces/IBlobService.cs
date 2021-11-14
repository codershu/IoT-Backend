using System;
using System.Threading.Tasks;
using IoT_Cloud.Models;
using Microsoft.AspNetCore.Http;

namespace IoT_Cloud.Interfaces
{
    public interface IBlobService
    {
        Task<bool> UploadFileToBlob(string fileName, string location, string filePath);
        Task<bool> CreateContainer(string containerName);
        Task<Response<bool>> DownloadAllBlobsInContainerToFolder(string containerName, string filePath);
        Task<Response<bool>> GetAllFilesNameInContainer(string containerName);
        Task<bool> IsExistingContainer(string containerName);
        string GetContainerSas(string accountName, string accountKey);
        Task<bool> UploadFile(string location, IFormFile uploadedFile);
    }
}
