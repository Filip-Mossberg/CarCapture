using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.ML;
using Microsoft.ML.Data;

namespace CarCapture2.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarDetectorController
    {
        [HttpPost("detect")]
        public async Task<CarDetectorModel.ModelOutput> CarDetector(PredictionEnginePool<CarDetectorModel.ModelInput, 
            CarDetectorModel.ModelOutput> predictionEnginePool, string imagePath)
        {
            var image = MLImage.CreateFromFile(imagePath);
            var input = new CarDetectorModel.ModelInput()
            {
                Image = image,
            };

            return await Task.FromResult(predictionEnginePool.Predict(input));
        }
    }
}
