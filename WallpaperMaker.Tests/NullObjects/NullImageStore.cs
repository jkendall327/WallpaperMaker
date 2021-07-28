using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using WallpaperMaker.Services;

namespace WallpaperMaker.Tests
{
    public class NullImageStore : IImageStore
    {
        public Image Get(Guid imageID) => null;
        public string GetPath(Guid imageID) => null;
        public Guid Store(Image image, IFormFile originalFile) => Guid.Empty;
        public Guid Store(Image image) => Guid.Empty;
        public Task<Guid> Store(IFormFile file) => Task.FromResult(Guid.Empty);
    }
}
