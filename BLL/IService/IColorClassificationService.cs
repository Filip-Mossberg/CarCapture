using Models;

namespace BLL.IService
{
    public interface IColorClassificationService
    {
        public Task<List<CarColorResult>> ColorClassificationModel(List<ColorClassificationInput> classificationInput);
    }
}
