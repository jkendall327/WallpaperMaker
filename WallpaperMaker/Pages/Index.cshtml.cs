using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;
using WallpaperMaker.Services;

namespace WallpaperMaker.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IImageStore _imageStore;

        public bool ConvertButtonDisabled { get; private set; } = true;
        public string? UploadedImageFilepath { get; private set; }

        [BindProperty(SupportsGet = true)]
        public Guid UploadedImageId { get; set; }

        public IndexModel(IImageStore imageStore)
        {
            _imageStore = imageStore;
        }

        public IActionResult OnGet()
        {
            if (UploadedImageId == default) return Page();

            ConvertButtonDisabled = false;

            UploadedImageFilepath = _imageStore.GetPath(UploadedImageId);

            return Page();
        }

        public async Task<IActionResult> OnPostUpload(IFormFile file)
        {
            if (!file.IsImage()) return RedirectToPage();

            var imageId = await _imageStore.Store(file);

            return RedirectToPage(new { UploadedImageId = imageId });
        }
    }
}
