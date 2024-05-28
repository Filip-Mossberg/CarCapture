using BLL.IService;
using Microsoft.Extensions.ML;
using Microsoft.ML.Data;
using System.Drawing;
using System.Drawing.Imaging;

namespace BLL.Service
{
    public class CarDetectorService : ICarDetectorService
    {
        private readonly PredictionEnginePool<CarDetectorModel.ModelInput, CarDetectorModel.ModelOutput> _predictionEnginePool;
        private readonly IImageService _imageService;
        public CarDetectorService(PredictionEnginePool<CarDetectorModel.ModelInput, CarDetectorModel.ModelOutput> predictionEnginePool, IImageService imageService)
        {
            _predictionEnginePool = predictionEnginePool;
            _imageService = imageService;
        }

        public async Task<CarDetectorModel.ModelOutput> CarDetectorModel(Image image)
        {
            var mlImage = _imageService.ConvertToMlImage(image);

            var input = new CarDetectorModel.ModelInput()
            {
                Image = mlImage
            };

            var modelResult = await Task.FromResult(_predictionEnginePool.Predict(input));

            return modelResult;
        }
    }
}
