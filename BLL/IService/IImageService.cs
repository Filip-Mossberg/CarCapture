using Microsoft.ML.Data;
using System.Drawing;

namespace BLL.IService
{
    public interface IImageService
    {
        public Task<Image> DrawAndLabelDetections(Image image, CarDetectorModel.ModelOutput modelResult);
        public Task<Image> ResizeAndPadImage(string imagepath, int targetWidth = 800, int targetHeight = 600);
    }
}