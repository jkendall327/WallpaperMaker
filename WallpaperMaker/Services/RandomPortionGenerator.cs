using System;
using SixLabors.ImageSharp;

namespace WallpaperMaker.Services
{
    public class RandomPortionGenerator
    {
        private readonly Random _random;

        public RandomPortionGenerator(Random random)
        {
            _random = random;
        }

        public Rectangle GetRandomSubsection(Rectangle original)
        {
            var newHeight = GetRandomPercentageOfValue(original.Height);
            var newWidth = (int)(newHeight * original.AspectRatio());

            var newRectangle = new Rectangle
            {
                Width = newWidth,
                Height = newHeight,

                // remember that y counts down...
                X = _random.Next(0, original.Width - newWidth),
                Y = _random.Next(0, original.Height - newHeight)
            };

            return newRectangle;
        }

        private readonly double _minPercent = 0.4;
        private readonly double _maxPercent = 0.7;

        private int GetRandomPercentageOfValue(int original)
        {
            // https://stackoverflow.com/a/1064907

            var percentage = _random.NextDouble() * (_maxPercent - _minPercent) + _minPercent;
            return (int)(original * percentage);
        }
    }
}
