using BLL.IService;
using Microsoft.AspNetCore.Mvc;
using Models;
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
        public async Task<CarDetectorResult> CarDetector(string imagePath)
        {
            try
            {
                var image = await _imageService.ResizeAndPadImage(imagePath);
                var modelResult = await _carDetectorService.CarDetectorModel(image);
                var imageResult = await _imageService.DrawAndLabelDetections(image, modelResult);

                return imageResult;
            }
            catch (Exception)
            {
                throw new Exception("Something went wrong!");
            }
        }
    }
}
