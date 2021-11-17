using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IoT_Cloud.Interfaces;
using IoT_Cloud.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Logging;

namespace IoT_Cloud.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IoTController : ControllerBase
    {
        private readonly ILogger<IoTController> _logger;
        private readonly IBlobService _blobService;

        public IoTController(ILogger<IoTController> logger, IBlobService blobService)
        {
            _logger = logger;
            _blobService = blobService;
        }

        [HttpGet]
        public string Get()
        {
            return "Hi! This is IoT Backend Server for course ECE569A";
        }


        [HttpPost("UploadFileToBlob/{location}")]
        public ActionResult<Response<bool>> UploadFileToBlob(string location)
        {
            try
            {
                var httpRequest = HttpContext.Request;
                var uploadFile = httpRequest.Form.Files[0];

                return Ok(_blobService.UploadFile(location, uploadFile).Result);
            }
            catch(Exception ex)
            {
                _logger.LogError($"Failed Upload File: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("GetAllContainers")]
        public ActionResult<Response<List<Container>>> GetAllContainers()
        {
            try
            {
                return Ok(_blobService.GetAllContainers().Result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed Get All Containers: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        
        [HttpGet("GetAllBlobsInContainer/{containerName}")]
        public ActionResult<Response<List<BlobFile>>> GetAllBlobsInContainer(string containerName)
        {
            try
            {
                return Ok(_blobService.GetAllBlobsInContainer(containerName).Result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed Get All Containers: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }
}
