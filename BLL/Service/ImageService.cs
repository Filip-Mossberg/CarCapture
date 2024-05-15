using BLL.IService;
using System.Drawing;
using System.Xml.XPath;
namespace BLL.Service
{
    public class ImageService : IImageService
    {
        public async Task<Image> DrawBoundingBoxes(string imagePath, List<Rectangle> boxes)
        {
            // Loading the image
            Image image = Image.FromFile(imagePath);

            // Adding rectangle on given coordinates
            using (Graphics graphics = Graphics.FromImage(image))
            {
                Pen pen = new Pen(Color.Blue, 2);

                foreach (var box in boxes)
                {
                    var rectangle = ImageCoordinateScaling(image.Width, image.Height, box);
                    graphics.DrawRectangle(pen, rectangle);
                }

            }

            return image;
        }

        // Scales the result coordinates to fit the original image size
        private Rectangle ImageCoordinateScaling(int imageWidth, int imageHeight, Rectangle box)
        {
            var xScale = imageWidth / 800;
            var yScale = imageHeight / 600;

            Rectangle rectangle = new Rectangle()
            {
                X = box.X * xScale,
                Y = box.Y * yScale,
                Width = box.Width * xScale,
                Height = box.Height * yScale
            };

            return rectangle;
        }
    }
}
