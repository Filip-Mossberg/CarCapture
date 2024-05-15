using BLL.IService;
using Microsoft.Extensions.ML;
using Microsoft.ML.Data;
using System.Drawing;

namespace BLL.Service
{
    public class CarDetectorService : ICarDetectorService
    {
        private readonly PredictionEnginePool<CarDetectorModel.ModelInput, CarDetectorModel.ModelOutput> _predictionEnginePool;
        public CarDetectorService(PredictionEnginePool<CarDetectorModel.ModelInput, CarDetectorModel.ModelOutput> predictionEnginePool)
        {
            _predictionEnginePool = predictionEnginePool;
        }
        public async Task<List<Rectangle>> CarDetectorModel(string imagePath)
        {
            var image = MLImage.CreateFromFile(imagePath);
            var input = new CarDetectorModel.ModelInput()
            {
                Image = image,
            };

            var modelResult = await Task.FromResult(_predictionEnginePool.Predict(input));

            return await CreateBoundingBoxList(modelResult);
        }

        private async Task<List<Rectangle>> CreateBoundingBoxList(CarDetectorModel.ModelOutput modelResult)
        {
            var boxList = new List<Rectangle>();

            for (int i = 0; i < modelResult.PredictedBoundingBoxes.Length; i += 4)
            {
                Rectangle rectangle = new Rectangle()
                {
                    X = (int)modelResult.PredictedBoundingBoxes[i],
                    Y = (int)modelResult.PredictedBoundingBoxes[i + 1],
                    Width = (int)modelResult.PredictedBoundingBoxes[i + 2],
                    Height = (int)modelResult.PredictedBoundingBoxes[i + 3]
                };

                boxList.Add(rectangle);
            }

            return boxList;
        }
    }
}
