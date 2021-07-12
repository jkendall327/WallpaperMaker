using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WallpaperMaker.Services;

namespace WallpaperMaker.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ImageModifier _converter;

        private readonly IImageStore _imageStore;

        readonly string tempImgPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "temp");

        public IndexModel(ILogger<IndexModel> logger, ImageModifier converter, ImageStore imageStore)
        {
            _logger = logger;
            _converter = converter;
            _imageStore = imageStore;

            Directory.CreateDirectory(Path.Combine("wwwroot", "temp"));
        }

        public void OnGet()
        {
            var temp = Directory.EnumerateFiles(tempImgPath);

            if (temp.Any())
            {
                foreach (var file in temp)
                {
                    System.IO.File.Delete(file);
                }
            }
        }

        public async Task<IActionResult> OnPostAsync(IFormFile file)
        {
            _logger.LogDebug($"File loaded: {file.FileName}");

            if (!file.IsImage()) return RedirectToPage();

            Image image = await Image.LoadAsync(file.OpenReadStream());

            var result = _converter.Convert(image);
            var imageID = _imageStore.Store(result, file);

            return RedirectToPage("ViewImage", new { id = imageID });
        }
    }
}
