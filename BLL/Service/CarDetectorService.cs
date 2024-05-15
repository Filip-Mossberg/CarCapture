using BLL.IService;
using Microsoft.Extensions.ML;
using Microsoft.ML.Data;

namespace BLL.Service
{
    public class CarDetectorService : ICarDetectorService
    {
        private readonly PredictionEnginePool<CarDetectorModel.ModelInput, CarDetectorModel.ModelOutput> _predictionEnginePool;
        public CarDetectorService(PredictionEnginePool<CarDetectorModel.ModelInput, CarDetectorModel.ModelOutput> predictionEnginePool)
        {
            _predictionEnginePool = predictionEnginePool;
        }
        public async Task<CarDetectorModel.ModelOutput> CarDetectorModel(string imagePath)
        {
            var image = MLImage.CreateFromFile(imagePath);
            var input = new CarDetectorModel.ModelInput()
            {
                Image = image,
            };

            return await Task.FromResult(_predictionEnginePool.Predict(input));
        }
    }
}
