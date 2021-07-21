using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WallpaperMaker.Services;
using static System.Net.WebRequestMethods;

namespace WallpaperMaker.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        private readonly ImageModifier _converter;
        private readonly IImageStore _imageStore;

        public bool ConvertButtonDisabled { get; set; } = true;
        public string UploadedImageFilepath { get; set; }

        [BindProperty(SupportsGet = true)]
        public Guid UploadedImageId { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool Convert { get; set; }

        public IndexModel(ILogger<IndexModel> logger, ImageModifier converter, IImageStore imageStore)
        {
            _logger = logger;
            _converter = converter;
            _imageStore = imageStore;

            Directory.CreateDirectory(Path.Combine("wwwroot", "temp"));
        }

        public IActionResult OnGet()
        {
            if (UploadedImageId == default) return Page();

            ConvertButtonDisabled = false;

            if (Convert)
            {
                UploadedImageId = ConvertImage(UploadedImageId);

                ConvertButtonDisabled = true;
            }

            UploadedImageFilepath = _imageStore.GetPath(UploadedImageId);

            return Page();
        }

        private Guid ConvertImage(Guid id)
        {
            var original = _imageStore.Get(id);

            var result = _converter.Convert(original);

            return _imageStore.Store(result);
        }

        public async Task<IActionResult> OnPostUpload(IFormFile file)
        {
            if (!file.IsImage()) return RedirectToPage();

            Guid imageID = await StoreUploadedImage(file);

            return RedirectToPage(new { UploadedImageId = imageID, Convert = false });
        }

        private async Task<Guid> StoreUploadedImage(IFormFile file)
        {
            var image = await Image.LoadAsync(file.OpenReadStream());
            return _imageStore.Store(image, file);
        }

        public IActionResult OnPostConvert(Guid uploadedId)
        {
            return RedirectToPage(new { UploadedImageId = uploadedId, Convert = true });
        }
    }
}
