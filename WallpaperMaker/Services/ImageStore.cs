using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;

namespace WallpaperMaker.Services
{
    public interface IImageStore
    {
        Task<Guid> Store(IFormFile file);
        public string GetPath(Guid imageId);
    }

    public class ImageStore : IImageStore
    {
        private readonly string _tempFolderPath = Path.Combine("wwwroot", "temp");

        private string ImagePath(Guid guid) => _tempFolderPath + Path.DirectorySeparatorChar + guid + ".jpg";

        public ImageStore()
        {
            Directory.CreateDirectory(_tempFolderPath);
        }

        public async Task<Guid> Store(IFormFile file)
        {
            var image = await Image.LoadAsync(file.OpenReadStream());
            
            var guid = Guid.NewGuid();

            await image.SaveAsync(ImagePath(guid));
            
            File.Delete(file.FileName);

            return guid;
        }
        
        public string GetPath(Guid imageId) => "temp" + Path.DirectorySeparatorChar + imageId + ".jpg";
    }
}
