using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;

namespace WallpaperMaker.Services
{
    public interface IImageStore
    {
        Guid Store(Image image, IFormFile originalFile);
        Image Get(Guid imageID);
        public string GetPath(Guid imageID);
    }

    public class ImageStore : IImageStore
    {
        private string ImagePath(Guid guid) => Path.Combine("wwwroot", "temp") + Path.DirectorySeparatorChar + guid + ".jpg";

        public Guid Store(Image image, IFormFile originalFile)
        {
            var guid = Guid.NewGuid();

            image.Save(ImagePath(guid));
            File.Delete(originalFile.FileName);

            return guid;
        }

        public Image Get(Guid imageID) => Image.Load(ImagePath(imageID));

        public string GetPath(Guid imageID) => "temp" + Path.DirectorySeparatorChar + imageID + ".jpg";
    }
}
