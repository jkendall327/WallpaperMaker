using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace WallpaperMaker.Tests
{
    public class NullFormFile : IFormFile
    {
        public string ContentType { get; }
        public string ContentDisposition { get; }
        public IHeaderDictionary Headers { get; }
        public long Length { get; }
        public string Name { get; }
        public string FileName { get; set; }

        public void CopyTo(Stream target) => throw new NotImplementedException();
        public Task CopyToAsync(Stream target, CancellationToken cancellationToken = default) => throw new NotImplementedException();
        public Stream OpenReadStream() => null;
    }
}
