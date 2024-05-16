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
        public CarDetectorService(PredictionEnginePool<CarDetectorModel.ModelInput, CarDetectorModel.ModelOutput> predictionEnginePool)
        {
            _predictionEnginePool = predictionEnginePool;
        }
        public async Task<List<Rectangle>> CarDetectorModel(Image image)
        {
            var mlImage = ConvertToMlImage(image);

            var input = new CarDetectorModel.ModelInput()
            {
                Image = mlImage,
            };

            var modelResult = await Task.FromResult(_predictionEnginePool.Predict(input));

            return await CreateBoundingBoxList(modelResult);
        }

        private async Task<List<Rectangle>> CreateBoundingBoxList(CarDetectorModel.ModelOutput modelResult)
        {
            var boxList = new List<Rectangle>();

            // Loops through the predicted lables and creates a list of Rectangle objects for adding boundingboxes
            for (int i = 0; i < modelResult.PredictedLabel.Count() * 4; i += 4)
            {
                // Filter by score
                if (modelResult.Score[i / 4] > 0.6F)
                {
                    // Coordinates for top left and bottom right corner of the rectangle
                    int x1 = (int)modelResult.PredictedBoundingBoxes[i];
                    int y1 = (int)modelResult.PredictedBoundingBoxes[i + 1];
                    int x2 = (int)modelResult.PredictedBoundingBoxes[i + 2];
                    int y2 = (int)modelResult.PredictedBoundingBoxes[i + 3];

                    Rectangle rectangle = new Rectangle()
                    {
                        // Sets the top left coordinate (x, y) and then calculates the distance along the x and y axis to get the width and height
                        X = (int)modelResult.PredictedBoundingBoxes[i],
                        Y = (int)modelResult.PredictedBoundingBoxes[i + 1],
                        Width = x2 - x1,
                        Height = y2 - y1
                    };

                    boxList.Add(rectangle);
                }
            }

            return boxList;
        }

        private static MLImage ConvertToMlImage(Image image)
        {
            MemoryStream stream = new MemoryStream();

            image.Save(stream, ImageFormat.Jpeg);

            stream.Position = 0;

            return MLImage.CreateFromStream(stream);
        }
    }
}
