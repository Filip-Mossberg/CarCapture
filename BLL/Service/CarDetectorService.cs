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

        public async Task<CarDetectorModel.ModelOutput> CarDetectorModel(Image image)
        {
            var mlImage = ConvertToMlImage(image);

            var input = new CarDetectorModel.ModelInput()
            {
                Image = mlImage,
            };

            var modelResult = await Task.FromResult(_predictionEnginePool.Predict(input));

            return modelResult;
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
