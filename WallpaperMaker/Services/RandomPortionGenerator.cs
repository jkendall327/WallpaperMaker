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
            var newHeight = GetRandomPercentageOfValue(original.Height);
            var newWidth = (int)(newHeight * original.AspectRatio());

            var newRectangle = new Rectangle()
            {
                Width = newWidth,
                Height = newHeight,

                // remember that y counts down...
                X = Random.Next(0, original.Width - newWidth),
                Y = Random.Next(0, original.Height - newHeight)
            };

            return newRectangle;
        }

        private readonly double MinPercent = 0.4;
        private readonly double MaxPercent = 0.7;

        private int GetRandomPercentageOfValue(int original)
        {
            // https://stackoverflow.com/a/1064907

            var percentage = Random.NextDouble() * (MaxPercent - MinPercent) + MinPercent;
            return (int)(original * percentage);
        }
    }
}
