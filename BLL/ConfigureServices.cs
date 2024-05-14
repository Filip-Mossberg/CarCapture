using BLL.IService;
using Microsoft.Extensions.DependencyInjection;

namespace BLL
{
    public static class ConfigureServices
    {
        public static IServiceCollection Configure(
            this IServiceCollection service)
        {
            service.AddScoped<IImageService, IImageService>();

            return service;
        }
    }
}
