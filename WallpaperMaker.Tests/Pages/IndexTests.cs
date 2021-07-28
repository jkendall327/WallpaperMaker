using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using WallpaperMaker.Pages;
using WallpaperMaker.Services;
using Xunit;

namespace WallpaperMaker.Tests
{
    public class IndexTests
    {
        private IndexModel _sut;

        public IndexTests()
        {
            _sut = new IndexModel(new NullImageStore());
        }

        [Fact]
        public async Task OnPostUpload_ShouldHaveNoRouteValues_IfUploadedFileIsNotImage()
        {
            var routeValues = await CheckFilename("failingExampleFilename");

            routeValues.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task OnPostUpload_ShouldHaveRouteValues_IfUploadedFileIsImage()
        {
            var routeValues = await CheckFilename("succesfulFilename.jpg");

            routeValues.Should().NotBeNullOrEmpty();
        }

        private async Task<RouteValueDictionary> CheckFilename(string filename)
        {
            var failingFile = new NullFormFile() { FileName = filename };

            var result = await _sut.OnPostUpload(failingFile);

            return (result as RedirectToPageResult).RouteValues;
        }

        [Fact]
        public void OnGet_ShouldDoNothing_WhenUploadedImageId_IsDefaultValue()
        {
            _sut.UploadedImageId = default;

            _sut.OnGet();

            _sut.UploadedImageFilepath.Should().BeNullOrEmpty();
            _sut.ConvertButtonDisabled.Should().BeTrue();
        }
    }
}
