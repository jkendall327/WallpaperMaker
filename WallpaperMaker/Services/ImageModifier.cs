using SixLabors.ImageSharp.Processing;
using System;
using System.Numerics;
using Color = SixLabors.ImageSharp.Color;
using Image = SixLabors.ImageSharp.Image;
using Rectangle = SixLabors.ImageSharp.Rectangle;

namespace WallpaperMaker.Pages
{
    public class ImageModifier
    {
        internal Image Convert(Image image)
        {
            image.Mutate(x =>
            {
                x.Resize(image.Width, 1080);
                x.Flip(FlipMode.Horizontal);
            });

            return image;
        }

        internal Image GetSlide(Image image, Rectangle outputSize, double panelFade = 0.2)
        {
            int newWidth = GetNewDimensions(image.Width, image, outputSize);
            int newHeight = GetNewDimensions(image.Height, image, outputSize);

            bool imageAlreadyFillsScreen = (outputSize.Width - newWidth) <= 0;

            if (!imageAlreadyFillsScreen)
            {
                CreateDetails(image, outputSize, panelFade, newWidth, newHeight);
            }

            var newImagePosition = CenterImage(outputSize, newWidth, newHeight);

            // DrawImage(image, newImagePosition);

            return image;
        }

        private Rectangle CenterImage(Rectangle canvas, int newWidth, int newHeight)
        {
            int relativeHalfwayPoint = (canvas.Width / 2) - (newWidth / 2);
            return new Rectangle(relativeHalfwayPoint, 0, newWidth, newHeight);
        }

        private void CreateDetails(Image image, Rectangle outputSize, double panelFade, int newWidth, int newHeight)
        {
            var renderTargetWidth = (outputSize.Width - newWidth) / 2;

            var detailLeft = new Rectangle(0, 0, renderTargetWidth + 1, newHeight);
            var detailRight = new Rectangle((outputSize.Width / 2) + (newWidth / 2), 0, renderTargetWidth, newHeight);

            var cutSize = new Vector2((int)((outputSize.Width - newWidth) / 2f), newHeight);
            var imageSize = new Vector2(image.Width, image.Height);

            var detailCutLeft = GetDetail(imageSize, cutSize);
            var detailCutRight = GetDetail(imageSize, cutSize);

            int levelOfTransparency = (int)(255 * panelFade);
            var transparentBlack = new Color(new Vector4(levelOfTransparency, 255, 255, 255));

            // SpriteBatch.Draw(image, detailLeft, detailCutLeft, transparentBlack);
            // SpriteBatch.Draw(image, detailRight, detailCutRight, transparentBlack);

            //Filter.BoxBlur(RenderTarget, detailLeft, detailRight);
        }

        private int GetNewDimensions(int baseValue, Image image, Rectangle canvas)
        {
            float aspectRatio = (float)canvas.Height / image.Height;
            return (int)(baseValue * aspectRatio);
        }

        readonly Random Random = new Random();

        const int minCutPercent = 40;
        const int maxCutPercent = 75;

        private Rectangle GetDetail(Vector2 imageSize, Vector2 cutSize)
        {
            // Generate Cut Size

            var cutAspect = cutSize.X / cutSize.Y;
            var cutImgAspect = (imageSize.X * (maxCutPercent / 100f)) / imageSize.Y;

            int cutWidth;
            int cutHeight;

            if (cutAspect <= cutImgAspect) // cut fits inside image at maxCutPercent
            {
                var result = CutsFitsInsideImageAtMaxCutPercent(imageSize, cutSize);
                cutWidth = (int)result.X;
                cutHeight = (int)result.Y;
            }
            else // cut doesn't fit inside image at maxCutPercent
            {
                var result = CutsDoesNotFitInsideImageAtMaxCutPercent(imageSize, cutSize);
                cutWidth = (int)result.X;
                cutHeight = (int)result.Y;
            }

            // Place Cut

            var cutWidthRange = (int)imageSize.X - cutWidth;
            var cutHeightRange = (int)imageSize.Y - cutHeight;

            var cutX = cutWidthRange > 0 ? Random.Next(0, cutWidthRange) : 0;
            var cutY = cutHeightRange > 0 ? Random.Next(0, cutHeightRange) : 0;

            return new Rectangle(cutX, cutY, cutWidth, cutHeight);
        }

        private Vector2 CutsFitsInsideImageAtMaxCutPercent(Vector2 imageSize, Vector2 cutSize)
        {
            var cutAspect = cutSize.X / cutSize.Y;

            // use the min/max cut ratio parameters
            var roll = Random.Next(1, 1001);
            var rollspan = (roll / 1000f) * (maxCutPercent - minCutPercent);
            var rollCutRatio = (minCutPercent + rollspan) * 0.01;

            // take slice of full image area
            var cutHeight = (int)(imageSize.Y * rollCutRatio);
            var cutWidth = (int)(cutHeight * cutAspect);

            return new Vector2(cutWidth, cutHeight);
        }

        private Vector2 CutsDoesNotFitInsideImageAtMaxCutPercent(Vector2 imageSize, Vector2 cutSize)
        {
            int cutWidth;
            int cutHeight;

            var cutAspect = cutSize.X / cutSize.Y;

            // bypass the min/max cut ratio parameters
            var roll = Random.Next(1, 1001);
            var rollspan = (roll / 1000f) * 15;
            var rollCutRatio = (75 + rollspan) * 0.01;

            // take slice of maximum possible cut area
            cutHeight = (int)((imageSize.Y * cutAspect) * rollCutRatio);
            cutWidth = (int)(cutHeight * cutAspect);

            if (cutWidth > imageSize.X)
            {
                var ratio = imageSize.X / cutWidth;

                cutWidth = (int)imageSize.X;
                cutHeight = (int)(cutHeight * ratio);
            }

            return new Vector2(cutWidth, cutHeight);
        }
    }
}