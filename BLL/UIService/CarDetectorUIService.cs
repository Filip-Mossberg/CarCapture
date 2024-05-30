using BLL.IService;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.UIService
{
    public class CarDetectorUIService : ICarDetectorUIService
    {
        private readonly IImageService _imageService;
        private readonly ICarDetectorService _carDetectorService;
        private readonly IColorClassificationService _colorClassificationService;
        public CarDetectorUIService(IImageService imageService, ICarDetectorService carDetectorService, IColorClassificationService colorClassificationService)
        {
            _imageService = imageService;
            _carDetectorService = carDetectorService;
            _colorClassificationService = colorClassificationService;
        }
        public async Task<CarDetectorResult> CarDetector(byte[] imageBytes)
        {
            // Event Pipeline
            var bitmapImage = await _imageService.BytearrayToBitmap(imageBytes);
            var image = await _imageService.ResizeAndPadImage(bitmapImage);
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
    }
}
