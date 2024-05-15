using System.Drawing;

namespace BLL.IService
{
    public interface IImageService
    {
        public Task<Image> DrawBoundingBoxes(string imagePath, List<Rectangle> boxes);
    }
}