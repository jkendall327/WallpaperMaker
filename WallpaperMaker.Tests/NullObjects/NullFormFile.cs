using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace WallpaperMaker.Tests.NullObjects
{
    public class NullFormFile : IFormFile
    {
        public string ContentType => null;
        public string ContentDisposition => null;
        public IHeaderDictionary Headers => null;
        public long Length => default;
        public string Name => null;
        public string FileName { get; init; }

        public void CopyTo(Stream target) => throw new NotImplementedException();
        public Task CopyToAsync(Stream target, CancellationToken cancellationToken = default) => throw new NotImplementedException();
        public Stream OpenReadStream() => null;
    }
}
