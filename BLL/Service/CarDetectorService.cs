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

            for (int i = 0; i < modelResult.PredictedLabel.Count() * 4; i += 4)
            {
                // Coordinates for top left and bottom right corner of the rectangle
                int x1 = (int)modelResult.PredictedBoundingBoxes[i];
                int y1 = (int)modelResult.PredictedBoundingBoxes[i + 1];
                int x2 = (int)modelResult.PredictedBoundingBoxes[i + 2];
                int y2 = (int)modelResult.PredictedBoundingBoxes[i + 3];

                Rectangle rectangle = new Rectangle()
                {
                    // Sets the top left coordinate (x, y) and then calculates the distance along the x and y axis to get the width and height. 
                    X = (int)modelResult.PredictedBoundingBoxes[i],
                    Y = (int)modelResult.PredictedBoundingBoxes[i + 1],
                    Width = x2 - x1,
                    Height = y2 - y1
                };

                boxList.Add(rectangle);
            }

            return boxList;
        }
    }
}
