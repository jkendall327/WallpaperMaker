using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using WallpaperMaker.Services;

namespace WallpaperMaker.Tests.NullObjects
{
    public class NullImageStore : IImageStore
    {
        public string GetPath(Guid imageId) => null;
        public Task<Guid> Store(IFormFile file) => Task.FromResult(Guid.Empty);
    }
}
