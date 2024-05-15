using BLL;
using BLL.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.ML;
using Microsoft.ML.Data;
using Newtonsoft.Json;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace CarCapture.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarDetectorController
    {
        private readonly ICarDetectorService _carDetectorService;
        private readonly IImageService _imageService;
        public CarDetectorController(ICarDetectorService carDetectorService, IImageService imageService)
        {
            _carDetectorService = carDetectorService;
            _imageService = imageService;
        }

        [HttpPost("Detect")]
        public async Task<string> CarDetector(string imagePath)
        {
            var result = await _carDetectorService.CarDetectorModel(imagePath);

            return JsonConvert.SerializeObject(result);
        }
    }
}
