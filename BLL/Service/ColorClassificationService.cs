using BLL.IService;
using Microsoft.Extensions.ML;
using Microsoft.ML.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Service
{
    public class ColorClassificationService : IColorClassificationService
    {
        private readonly PredictionEnginePool<CarColorClassificationModel.ModelInput, CarColorClassificationModel.ModelOutput> _predictionEnginePool;
        public ColorClassificationService(PredictionEnginePool<CarColorClassificationModel.ModelInput, CarColorClassificationModel.ModelOutput> predictionEnginePool)
        {
            _predictionEnginePool = predictionEnginePool;
        }

        public async Task<CarColorClassificationModel.ModelOutput> ColorClassificationModel(string imagePath)
        {
            var input = new CarColorClassificationModel.ModelInput()
            {
                ImageSource = File.ReadAllBytes(imagePath),
            };

            return await Task.FromResult(_predictionEnginePool.Predict(input));
        }
    }
}
