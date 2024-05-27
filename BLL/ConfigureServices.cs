using BLL.IService;
using BLL.Service;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.ML;

namespace BLL
{
    public static class ConfigureServices
    {
        public static IServiceCollection Configure(
            this IServiceCollection services)
        {
            services.AddScoped<ICarDetectorService, CarDetectorService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IColorClassificationService, ColorClassificationService>();

            services.AddPredictionEnginePool<CarDetectorModel.ModelInput, CarDetectorModel.ModelOutput>()
.FromFile("C:\\Users\\Joakim\\source\\repos\\CarCapture\\BLL\\CarDetectorModel.mlnet");

            services.AddPredictionEnginePool<CarColorClassificationModel.ModelInput, CarColorClassificationModel.ModelOutput>()
    .FromFile("CarColorClassificationModel.mlnet");

            return services;
        }
    }
}
