using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using WallpaperMaker.Pages;

namespace WallpaperMaker
{
    [Route("[controller]")]
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

            Image image = await GetRequestedImage(url);

            MemoryStream stream = ConvertImage(image);

            return File(stream, "image/jpeg");
        }

        private List<string> AcceptedHeaders = new() { "image/gif", "image/jpeg", "image/png", "image/tiff" };

        private bool IsValid(string url)
        {
            if (string.IsNullOrEmpty(url)) return false;

            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute)) return false;

            if (!_response.IsSuccessStatusCode || _response is null) return false;

            var responseType = _response.Content.Headers.ContentType.MediaType;

            if (!AcceptedHeaders.Contains(responseType)) return false;

            return true;
        }

        private async Task<Image> GetRequestedImage(string url)
        {
            var stream = await _response.Content.ReadAsStreamAsync();

            return await Image.LoadAsync(stream);
        }

        private MemoryStream ConvertImage(Image image)
        {
            var result = _modifier.Convert(image);

            MemoryStream stream = new MemoryStream();

            result.Save(stream, new JpegEncoder());

            // 'using' statements don't work well here
            stream.Flush();
            stream.Position = 0;

            return stream;
        }
    }
}
