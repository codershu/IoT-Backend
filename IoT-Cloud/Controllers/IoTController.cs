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
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<IoTController> _logger;
        private readonly IBlobService _blobService;

        public IoTController(ILogger<IoTController> logger, IBlobService blobService)
        {
            _logger = logger;
            _blobService = blobService;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
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
