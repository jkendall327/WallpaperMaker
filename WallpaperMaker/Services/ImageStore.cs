using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;

namespace WallpaperMaker.Services
{
    public interface IImageStore
    {
        Guid Store(Image image, IFormFile originalFile);
        Guid Store(Image image);
        Task<Guid> Store(IFormFile file);
        Image Get(Guid imageID);
        public string GetPath(Guid imageID);
    }

    public class ImageStore : IImageStore
    {
        private readonly string TempFolderPath = Path.Combine("wwwroot", "temp");

        private string ImagePath(Guid guid) => TempFolderPath + Path.DirectorySeparatorChar + guid + ".jpg";

        public ImageStore()
        {
            Directory.CreateDirectory(TempFolderPath);
        }

        public async Task<Guid> Store(IFormFile file)
        {
            var image = await Image.LoadAsync(file.OpenReadStream());
            return Store(image, file);
        }

        public Guid Store(Image image, IFormFile originalFile)
        {
            var guid = Guid.NewGuid();

            image.Save(ImagePath(guid));
            File.Delete(originalFile.FileName);

            return guid;
        }

        public Guid Store(Image image)
        {
            var guid = Guid.NewGuid();

            image.Save(ImagePath(guid));

            return guid;
        }

        public Image Get(Guid imageID) => Image.Load(ImagePath(imageID));

        public string GetPath(Guid imageID) => "temp" + Path.DirectorySeparatorChar + imageID + ".jpg";
    }
}
