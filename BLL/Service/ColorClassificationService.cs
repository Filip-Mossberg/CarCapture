using BLL.IService;
using Microsoft.Extensions.ML;
using Models;

namespace BLL.Service
{
    public class ColorClassificationService : IColorClassificationService
    {
        private readonly PredictionEnginePool<CarColorClassificationModel.ModelInput, CarColorClassificationModel.ModelOutput> _predictionEnginePool;
        public ColorClassificationService(PredictionEnginePool<CarColorClassificationModel.ModelInput, CarColorClassificationModel.ModelOutput> predictionEnginePool)
        {
            _predictionEnginePool = predictionEnginePool;
        }

        public async Task<List<CarColorResult>> ColorClassificationModel(List<ColorClassificationInput> classificationInput)
        {
            var predictedColorResult = new List<CarColorResult>();

            foreach (var obj in classificationInput)
            {
                var input = new CarColorClassificationModel.ModelInput()
                {
                    ImageSource = obj.Image
                };

                var colorResult = await Task.FromResult(_predictionEnginePool.Predict(input));

                var carColorResult = new CarColorResult()
                {
                    CarScore = obj.Car,
                    Color = colorResult.PredictedLabel
                };

                predictedColorResult.Add(carColorResult);   
            }

            return predictedColorResult;
        }
    }
}
