using System;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;

namespace WallpaperMaker
{
    public static class Extensions
    {
        public static bool IsImage(this IFormFile file)
        {
            return file.FileName.EndsWith(".jpg") || file.FileName.EndsWith(".jpeg") || file.FileName.EndsWith(".png");
        }

        public static decimal AspectRatio(this Rectangle rect)
        {
            var result = rect.Width / (decimal)rect.Height;

            return Math.Round(result, 2);
        }
    }
}
