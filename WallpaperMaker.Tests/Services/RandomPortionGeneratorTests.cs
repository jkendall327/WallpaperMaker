using System;
using FluentAssertions;
using SixLabors.ImageSharp;
using WallpaperMaker.Services;
using Xunit;

namespace WallpaperMaker.Tests.Services
{
    public class RandomPortionGeneratorTests
    {
        private readonly RandomPortionGenerator _sut = new(new Random(0));

        private static int Area(Rectangle rect) => rect.Width * rect.Height;

        private static Rectangle TestRectangle => new(100, 100, 100, 100);

        [Fact]
        public void ShouldReturnDifferentRectangleThanOriginal()
        {
            var actual = _sut.GetRandomSubsection(TestRectangle);

            actual.Should().NotBe(TestRectangle);
        }

        [Fact]
        public void ShouldReturnSmallerRectangleThanOriginal()
        {
            var actual = _sut.GetRandomSubsection(TestRectangle);

            Area(TestRectangle).Should().BeGreaterThan(Area(actual));
        }

        [Fact]
        public void ShouldReturnRectangleWithSameAspectRatioAsOriginal()
        {
            // 16:9
            var test = new Rectangle(0, 0, 1920, 1080);

            var actual = _sut.GetRandomSubsection(test);

            actual.AspectRatio().Should().Be(test.AspectRatio());
        }
    }
}
