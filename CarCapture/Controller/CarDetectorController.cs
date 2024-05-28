using BLL;
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
        private readonly IColorClassificationService _colorClassificationService;
        public CarDetectorController(ICarDetectorService carDetectorService, IImageService imageService, IColorClassificationService colorClassificationService)
        {
            _carDetectorService = carDetectorService;
            _imageService = imageService;
            _colorClassificationService = colorClassificationService;
        }

        [HttpPost("Detect")]
        public async Task<CarDetectorResult> CarDetector(string imagePath)
        {
            try
            {
                var image = await _imageService.ResizeAndPadImage(imagePath);
                var modelResult = await _carDetectorService.CarDetectorModel(image);
                var modelResultFiltered = await _imageService.ModelResultFiltering(modelResult);
                var colorClassificationInput = await _imageService.CreateImagesOfDetectedCars(image, modelResultFiltered);
                var colorClassificationResult = await _colorClassificationService.ColorClassificationModel(colorClassificationInput);
                var imageResult = await _imageService.DrawAndLabelDetections(image, modelResultFiltered);


                var carDetectorResult = new CarDetectorResult()
                {
                    ColorList = colorClassificationResult,
                    Image = imageResult
                };

                return carDetectorResult;
            }
            catch (Exception)
            {
                throw new Exception("Something went wrong!");
            }
        }
    }
}
