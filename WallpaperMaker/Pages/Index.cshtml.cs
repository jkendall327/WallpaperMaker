using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WallpaperMaker.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ImageModifier _converter;
        readonly string tempImgPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "temp");

        public IndexModel(ILogger<IndexModel> logger, ImageModifier converter)
        {
            _logger = logger;
            _converter = converter;

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

            var info = new FileInfo(file.FileName);

            if (info.Extension.ToLower() == ".jpg")
            {
                Image image;

                using (Stream fileStream = new FileStream(file.FileName, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);

                    image = Image.FromStream(fileStream);
                }

                var result = _converter.Convert(image);

                var imgname = System.Guid.NewGuid() + ".jpg";
                var imgpath = Path.Combine("wwwroot", "temp") + Path.DirectorySeparatorChar + imgname;
                result.Save(imgpath);

                System.IO.File.Delete(file.FileName);

                return RedirectToPage("ViewImage", new { path = "temp" + Path.DirectorySeparatorChar + imgname });
            }

            return RedirectToPage();
        }
    }
}
