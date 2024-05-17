using BLL.IService;
using Models;
using System.Drawing;
namespace BLL.Service
{
    public class ImageService : IImageService
    {
        public async Task<Image> DrawAndLabelDetections(Image image, CarDetectorModel.ModelOutput modelResult)
        {
            var modelFiltrationResult = await ModelResultFiltering(modelResult);

            // Adding rectangle on given coordinates
            using (Graphics graphics = Graphics.FromImage(image))
            {
                for (int i = 0; i < modelFiltrationResult.BoxList.Count; i++)
                {
                    if (modelFiltrationResult.LabelList[i] == "Car")
                    {
                        graphics.DrawRectangle(new Pen(Color.Blue, 2), modelFiltrationResult.BoxList[i]);
                    }
                    else
                    {
                        graphics.DrawRectangle(new Pen(Color.Orange, 2), modelFiltrationResult.BoxList[i]);
                    }

                    graphics.DrawString(modelFiltrationResult.LabelList[i] + ": " + modelFiltrationResult.ScoreList[i],
                        new Font("Arial", 14), new SolidBrush(Color.HotPink),
                        modelFiltrationResult.TopLeftCoordinatesList[i + i],
                        modelFiltrationResult.TopLeftCoordinatesList[i + i + 1] - 22);
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

        private async Task<ModelFiltrationResult> ModelResultFiltering(CarDetectorModel.ModelOutput modelResult)
        {
            var boxList = new List<Rectangle>();
            var labelList = new List<string>();
            var scoreList = new List<string>();
            var topLeftCoordinatesList = new List<int>();

            // Loops through the predicted lables and creates a list of Rectangle objects for adding boundingboxes
            for (int i = 0; i < modelResult.PredictedLabel.Count() * 4; i += 4)
            {
                // Filter by score
                if (modelResult.Score[i / 4] > 0.6F)
                {
                    // Coordinates for top left and bottom right corner of the rectangle
                    int x1 = (int)modelResult.PredictedBoundingBoxes[i];
                    int y1 = (int)modelResult.PredictedBoundingBoxes[i + 1];
                    int x2 = (int)modelResult.PredictedBoundingBoxes[i + 2];
                    int y2 = (int)modelResult.PredictedBoundingBoxes[i + 3];

                    Rectangle rectangle = new Rectangle()
                    {
                        // Sets the top left coordinate (x, y) and then calculates the distance along the x and y axis to get the width and height
                        X = (int)modelResult.PredictedBoundingBoxes[i],
                        Y = (int)modelResult.PredictedBoundingBoxes[i + 1],
                        Width = x2 - x1,
                        Height = y2 - y1
                    };

                    boxList.Add(rectangle);
                    labelList.Add(modelResult.PredictedLabel[i / 4].ToString());
                    scoreList.Add(modelResult.Score[i / 4].ToString());
                    topLeftCoordinatesList.Add(x1);
                    topLeftCoordinatesList.Add(y1);
                }
            }

            return new ModelFiltrationResult()
            {
                BoxList = boxList,
                LabelList = labelList,
                ScoreList = scoreList,
                TopLeftCoordinatesList = topLeftCoordinatesList
            };
        }
    }
}
