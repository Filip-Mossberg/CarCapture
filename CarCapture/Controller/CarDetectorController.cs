using BLL.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
            var modelResult = await _carDetectorService.CarDetectorModel(imagePath);
            var imageResult = await _imageService.DrawBoundingBoxes(imagePath, modelResult);

            return JsonConvert.SerializeObject(modelResult);
        }
    }
}
