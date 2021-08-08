using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using WallpaperMaker.Services;

namespace WallpaperMaker
{
    [Route("api/{version:apiVersion}/[controller]")]
    [ApiController]
    public class ConvertController : ControllerBase
    {
        private readonly HttpClient _client;
        private readonly ImageModifier _modifier;

        public ConvertController(IHttpClientFactory factory, ImageModifier modifier)
        {
            _client = factory.CreateClient();
            _modifier = modifier;
        }

        private HttpResponseMessage? _response;

        public async Task<IActionResult> Get(string url)
        {
            _response = await _client.GetAsync(url);

            if (!IsValid(url)) return StatusCode(415);

            Image image = await GetRequestedImage();

            MemoryStream stream = ConvertImage(image);

            return File(stream, "image/jpeg");
        }

        private readonly List<string> _acceptedHeaders = new() { "image/gif", "image/jpeg", "image/png", "image/tiff" };

        private bool IsValid(string url)
        {
            if (string.IsNullOrEmpty(url)) return false;

            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute)) return false;

            if (_response is null || !_response.IsSuccessStatusCode) return false;

            var responseType = _response?.Content.Headers.ContentType?.MediaType;

            return responseType is not null && _acceptedHeaders.Contains(responseType);
        }

        private async Task<Image> GetRequestedImage()
        {
            var task = _response?.Content.ReadAsStreamAsync();

            if (task is null)
            {
                throw new ArgumentNullException(nameof(Content));
            }

            var stream = await task;
            return await Image.LoadAsync(stream);
        }

        private MemoryStream ConvertImage(Image image)
        {
            var result = _modifier.Convert(image, new Size(1920, 1080));

            MemoryStream stream = new();

            result.Save(stream, new JpegEncoder());

            // 'using' statements don't work well here
            stream.Flush();
            stream.Position = 0;

            return stream;
        }
    }
}
