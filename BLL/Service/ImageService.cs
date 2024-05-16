using BLL.IService;
using System.Drawing;
namespace BLL.Service
{
    public class ImageService : IImageService
    {
        public async Task<Image> DrawBoundingBoxes(Image image, List<Rectangle> boxes)
        {
            // Adding rectangle on given coordinates
            using (Graphics graphics = Graphics.FromImage(image))
            {
                Pen pen = new Pen(Color.Blue, 2);

                foreach (var box in boxes)
                {
                    graphics.DrawRectangle(pen, box);
                }

            }

            image.Save("C:\\Users\\Joakim\\Desktop\\SaveTest\\CarDetectedImage.Jpeg", System.Drawing.Imaging.ImageFormat.Jpeg);

            return image;
        }

        public async Task<Image> ResizeAndPadImage(string imagePath, int targetWidth = 800, int targetHeight = 600)
        {
            Image originalImage = Image.FromFile(imagePath);

            // Calculates scale to fit image within the target size while preserving aspect ratio
            float scale = Math.Min((float)targetWidth / originalImage.Width, (float)targetHeight / originalImage.Height);

            // Calculates the width and height of the scaled image
            var scaledWidth = (int)(originalImage.Width * scale);
            var scaledHeight = (int)(originalImage.Height * scale);

            // Calculates padding
            var padX = (targetWidth - scaledWidth) / 2;
            var padY = (targetHeight - scaledHeight) / 2;

            // Creates a new bitmap with desired size and makes it black
            Bitmap resizedImage = new Bitmap(targetWidth, targetHeight);
            using (Graphics graphics = Graphics.FromImage(resizedImage))
            {
                graphics.FillRectangle(Brushes.Black, 0, 0, targetWidth, targetHeight);
                graphics.DrawImage(originalImage, padX, padY, scaledWidth, scaledHeight);
            }

            return resizedImage;
        }
    }
}
