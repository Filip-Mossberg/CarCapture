using System.Drawing;

namespace BLL.IService
{
    public interface IImageService
    {
        public Image DrawBoundingBoxes(string imagePath, List<Rectangle> boxes);
    }
}