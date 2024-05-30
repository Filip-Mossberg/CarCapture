﻿using BLL.IService;
using Microsoft.ML.Data;
using Models;
using System.Drawing;
using System.Drawing.Imaging;
namespace BLL.Service
{
    public class ImageService : IImageService
    {
        /// <summary>
        /// Method for drawing detections and adding lable text such as score to the image. For now it also implements methods
        /// ModelResultFiltering for filtering the incoming ModelOutput data and ImageToBase64. 
        /// </summary>
        public async Task<Image> DrawAndLabelDetections(Image image, ModelFiltrationResult modelFiltrationResult)
        {
            // Adding label and rectangle boundingbox on given coordinates
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

            return image;
        }

        /// <summary>
        /// Method for Resizing the incoming image before it is used inside the CarDetectorModel.
        /// We Resize so its easier to draw out borderboxes afterwards. 
        /// </summary>
        public async Task<Image> ResizeAndPadImage(Bitmap originalImage, int targetWidth = 800, int targetHeight = 600)
        {
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
                graphics.FillRectangle(Brushes.White, 0, 0, targetWidth, targetHeight);
                graphics.DrawImage(originalImage, padX, padY, scaledWidth, scaledHeight);
            }
            return resizedImage;
        }

        /// <summary>
        /// Method for filtering the ModelOutput result to remove unwanted data.
        /// </summary>
        public async Task<ModelFiltrationResult> ModelResultFiltering(CarDetectorModel.ModelOutput modelResult)
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
                    var x1 = (int)modelResult.PredictedBoundingBoxes[i];
                    var y1 = (int)modelResult.PredictedBoundingBoxes[i + 1];
                    var x2 = (int)modelResult.PredictedBoundingBoxes[i + 2];
                    var y2 = (int)modelResult.PredictedBoundingBoxes[i + 3];

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

        /// <summary>
        /// Method for creating new images for each detected car area.
        /// </summary>
        public async Task<List<ColorClassificationInput>> CreateImagesOfDetectedCars(Image image, ModelFiltrationResult modelFiltrationResult)
        {
            var result = new List<ColorClassificationInput>();

            for (int i = 0; i < modelFiltrationResult.BoxList.Count; i++)
            {
                if (modelFiltrationResult.LabelList[i] == "Car")
                {
                    var bitMap = new Bitmap(modelFiltrationResult.BoxList[i].Width, modelFiltrationResult.BoxList[i].Height);

                    using (Graphics graphics = Graphics.FromImage(bitMap))
                    {
                        graphics.DrawImage(image,
                            new Rectangle(0, 0, bitMap.Width, bitMap.Height),
                            new Rectangle(modelFiltrationResult.BoxList[i].X, modelFiltrationResult.BoxList[i].Y, modelFiltrationResult.BoxList[i].Width, modelFiltrationResult.BoxList[i].Height),
                            GraphicsUnit.Pixel);
                    }

                    var colorClassificationInput = new ColorClassificationInput()
                    {
                        Image = ImageToByte(bitMap),
                        Car = modelFiltrationResult.ScoreList[i]
                    };

                    result.Add(colorClassificationInput);
                }
            }

            return result;
        }

        public static byte[] ImageToByte(Image image)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(image, typeof(byte[]));
        }

        public MLImage ConvertToMlImage(Image image)
        {
            MemoryStream stream = new MemoryStream();

            image.Save(stream, ImageFormat.Jpeg);

            stream.Position = 0;

            return MLImage.CreateFromStream(stream);
        }

        private static string ImageToBase64(Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Jpeg);
                byte[] imageBytes = ms.ToArray();

                return Convert.ToBase64String(imageBytes);
            }
        }

        public async Task<Bitmap> BytearrayToBitmap(byte[] imageBytes)
        {
            Bitmap bitmap;
            using (var ms = new MemoryStream(imageBytes))
            {
                bitmap = new Bitmap(ms);
            }

            return bitmap;
        }

        public async Task<Stream> ImageToStream(Image image)
        {
            MemoryStream ms = new MemoryStream();
            image.Save(ms, ImageFormat.Jpeg);
            ms.Position = 0;

            return ms;
        }
    }
}
