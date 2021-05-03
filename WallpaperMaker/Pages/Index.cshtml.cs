﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.IO;
using WallpaperMaker.Services;

namespace WallpaperMaker.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly SqliteAccess _access;

        public IndexModel(ILogger<IndexModel> logger, SqliteAccess access)
        {
            _logger = logger;
            _access = access;
        }

        public void OnGet()
        {

        }

        public void OnPost(IFormFile file)
        {
            _logger.LogDebug($"File loaded: {file.FileName}");

            // check if image

            var info = new FileInfo(file.FileName);

            var isImage = false;

            if (info.Extension == ".jpg")
            {
                isImage = true;
            }

            if (!isImage)
            {
                throw new System.Exception();

            }

            // save to database
            _access.Save(info);
        }

    }
}
