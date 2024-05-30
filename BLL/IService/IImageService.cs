using Microsoft.ML.Data;
using Models;
using System.Drawing;

namespace BLL.IService
{
    public interface IImageService
    {
        public Task<Image> DrawAndLabelDetections(Image image, ModelFiltrationResult modelFiltrationResult);
        public Task<Image> ResizeAndPadImage(Bitmap originalImage, int targetWidth = 800, int targetHeight = 600);
        public Task<ModelFiltrationResult> ModelResultFiltering(CarDetectorModel.ModelOutput modelResult);
        public Task<List<ColorClassificationInput>> CreateImagesOfDetectedCars(Image image, ModelFiltrationResult modelFiltrationResult);
        public MLImage ConvertToMlImage(Image image);
        public Task<Bitmap> BytearrayToBitmap(byte[] imageBytes);
        public Task<Stream> ImageToStream(Image image);
    }
}