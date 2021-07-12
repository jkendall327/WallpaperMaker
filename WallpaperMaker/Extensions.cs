using System;
using Microsoft.AspNetCore.Http;

namespace WallpaperMaker
{
    public static class Extensions
    {
        public static bool IsImage(this IFormFile file)
        {
            return file.FileName.EndsWith(".jpg") || file.FileName.EndsWith(".jpeg") || file.FileName.EndsWith(".png");
        }
    }
}
