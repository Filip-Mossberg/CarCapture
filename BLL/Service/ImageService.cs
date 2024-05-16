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

            image.Save("C:\\Users\\Joakim\\Desktop\\SaveTest\\CarDetectedImage.Jpeg", System.Drawing.Imaging.ImageFormat.Jpeg);                  

            return image;
        }

        // Scales the result coordinates to fit the original image size
        private Rectangle ImageCoordinateScaling(int imageWidth, int imageHeight, Rectangle box)
        {
            float xScale = (imageWidth / 800.0F);
            float yScale = (imageHeight / 600.0F);

            Rectangle rectangle = new Rectangle()
            {
                X = (int)Math.Ceiling(box.X * xScale),
                Y = (int)Math.Ceiling(box.Y * yScale),
                Width = (int)Math.Ceiling(box.Width * xScale),
                Height = (int)Math.Ceiling(box.Height * yScale)
            };

            return rectangle;
        }
    }
}
