using System;
using System.Threading.Tasks;
using IoT_Cloud.Interfaces;
using IoT_Cloud.Models;
using Microsoft.AspNetCore.Http;
using Azure.Storage;
using Azure.Storage.Sas;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Auth;
using Microsoft.Azure.Storage.Blob;
using System.IO;

namespace IoT_Cloud.Service
{
    public class BlobService : IBlobService
    {
        private readonly string _accountName;
        private readonly string _accountKey;

        private CloudStorageAccount _cloudStorageAccount;
        private CloudBlobClient _blockClient;


        public BlobService()
        {
            _accountName = "iotcloud";
            _accountKey = "5SWSOqZtu115Yx5NPBFi/60wRNlC3w6w/F9EDgJWioZoJ52p0L9iAiwhDKt0AXBgVjQTdLHP0dO3RmV2TWao7w==";

            StorageCredentials sc = new StorageCredentials(_accountName, _accountKey);
            _cloudStorageAccount = new CloudStorageAccount(sc, true);
            _blockClient = _cloudStorageAccount.CreateCloudBlobClient();
        }

        public async Task<bool> CreateContainer(string containerName)
        {
            CloudBlobContainer container = _blockClient.GetContainerReference(containerName);
            await container.CreateIfNotExistsAsync();
            return true;
        }

        public Task<Response<bool>> DownloadAllBlobsInContainerToFolder(string containerName, string filePath)
        {
            throw new NotImplementedException();
        }

        public Task<Response<bool>> GetAllFilesNameInContainer(string containerName)
        {
            throw new NotImplementedException();
        }

        public string GetContainerSas(string accountName, string accountKey)
        {
            StorageCredentials sc = new StorageCredentials(accountName, accountKey);
            _cloudStorageAccount = new CloudStorageAccount(sc, true);

            var key = _cloudStorageAccount.GetSharedAccessSignature(new SharedAccessAccountPolicy()
            {
                Permissions = SharedAccessAccountPermissions.Read,
                SharedAccessStartTime = DateTime.UtcNow.AddSeconds(-5),
                SharedAccessExpiryTime = DateTimeOffset.UtcNow.AddDays(1),
                Protocols = SharedAccessProtocol.HttpsOnly,
                Services = SharedAccessAccountServices.Blob | SharedAccessAccountServices.File,
                ResourceTypes = SharedAccessAccountResourceTypes.Container | SharedAccessAccountResourceTypes.Object
            });

            return key;
        }

        public Task<bool> IsExistingContainer(string containerName)
        {
            //var found = _blockClient.GetContainerReference(containerName);
            var containerList = _blockClient.ListContainers();
            foreach (var container in containerList)
            {
                if(container.Name == containerName)
                {
                    return Task.FromResult(true);
                }
            }
            return Task.FromResult(false);
        }

        public async Task<bool> UploadFileToBlob(string fileName, string location, string filePath)
        {
            CloudBlobContainer uploadContainer = _blockClient.GetContainerReference(location);
            CloudBlockBlob uploadBlockBlob = uploadContainer.GetBlockBlobReference(fileName);
            await uploadBlockBlob.UploadFromFileAsync(filePath);

            return true;
        }

        public async Task<bool> UploadFile(string location, IFormFile uploadedFile)
        {
            string tempFolderPath = Path.GetTempPath();
            string newTempFolderPath = tempFolderPath + $"/iot-{Guid.NewGuid()}";

            System.IO.Directory.CreateDirectory(newTempFolderPath);

            var fileName = uploadedFile.FileName;
            var filePath = newTempFolderPath + $"/{fileName}";

            using (Stream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                uploadedFile.CopyTo(fileStream);
            }

            var existed = IsExistingContainer(location);
            if (!existed.Result)
            {
                await CreateContainer(location);
            }
            await UploadFileToBlob(fileName, location, filePath);

            return true;
        }
    }
}
