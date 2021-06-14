using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Drawing;

namespace WallpaperMaker.Pages
{
    public class ViewImageModel : PageModel
    {
        public string ImagePath { get; set; }

        public Image Img { get; set; }

        public void OnGet(string path)
        {
            ImagePath = path;
        }
    }
}
