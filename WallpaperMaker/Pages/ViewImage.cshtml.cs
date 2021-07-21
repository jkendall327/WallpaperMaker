using System;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WallpaperMaker.Services;

namespace WallpaperMaker.Pages
{
    public class ViewImageModel : PageModel
    {
        public string ImagePath { get; set; }

        private readonly IImageStore _imageStore;

        public ViewImageModel(IImageStore imageStore) => _imageStore = imageStore;

        public void OnGet(Guid id)
        {
            ImagePath = _imageStore.GetPath(id);
        }
    }
}
