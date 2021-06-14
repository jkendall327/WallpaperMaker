using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WallpaperMaker.Pages
{
    public class ViewImageModel : PageModel
    {
        public string ImagePath { get; set; }

        public void OnGet(string path)
        {
            ImagePath = path;
        }
    }
}
