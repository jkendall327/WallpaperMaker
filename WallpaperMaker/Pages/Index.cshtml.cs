using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace WallpaperMaker.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ImageModifier _converter;

        public IndexModel(ILogger<IndexModel> logger, ImageModifier converter)
        {
            _logger = logger;
            _converter = converter;
        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync(IFormFile file)
        {
            _logger.LogDebug($"File loaded: {file.FileName}");

            var info = new FileInfo(file.FileName);

            if (info.Extension.ToLower() == ".jpg")
            {
                Image image;

                using (Stream fileStream = new FileStream(file.FileName, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);

                    image = Image.FromStream(fileStream);
                }

                _converter.Convert(image);

                return RedirectToPage("viewImage");
            }

            return RedirectToPage("Privacy");
        }

    }
}
