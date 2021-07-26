using System;
using SixLabors.ImageSharp;

namespace WallpaperMaker.Services
{
    public class RandomPortionGenerator
    {
        private readonly Random Random;

        public RandomPortionGenerator(Random random)
        {
            Random = random;
        }

        public Rectangle GetRandomSubsection(Rectangle original)
        {
            /*
             * given rectangle of arbitrary size
             * return a random sub-rectangle
             * whose size is above some arbitrary
             * percentage of the size of the original
             */

            var minPercent = 0.4;
            var maxPercent = 0.7;

            // https://stackoverflow.com/a/1064907
            var widthPercentage = Random.NextDouble() * (maxPercent - minPercent) + minPercent;
            var heightPercentage = Random.NextDouble() * (maxPercent - minPercent) + minPercent;

            var originalBounds = original;

            var finalWidth = (int)(originalBounds.Width * widthPercentage);
            var finalHeight = (int)(originalBounds.Height * heightPercentage);

            var newRectangle = new Rectangle()
            {
                Width = finalWidth,
                Height = finalHeight,

                // remember that y counts down...
                X = Random.Next(0, original.Width - finalWidth),
                Y = Random.Next(0, original.Height - finalHeight)
            };

            return newRectangle;
        }
    }
}
