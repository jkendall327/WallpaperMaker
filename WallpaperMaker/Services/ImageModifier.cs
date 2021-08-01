using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using WallpaperMaker.Services;

namespace WallpaperMaker.Pages
{
    public class ImageModifier
    {
        private readonly RandomPortionGenerator _generator;

        private Size FinalSize = new(1920, 1080);
        private ResizeOptions resizeOptions;

        public ImageModifier(RandomPortionGenerator generator)
        {
            _generator = generator;
        }

        internal Image Convert(Image image, Size newSize)
        {
            FinalSize = newSize;

            resizeOptions = new ResizeOptions()
            {
                Size = new Size(0, FinalSize.Height),
                Mode = ResizeMode.Pad
            };

            image.Mutate(x =>
            {
                x.Resize(resizeOptions);
            });

            if (image.Width < FinalSize.Width)
            {
                MakeWallpaper(image);
            }

            return image;
        }

        private void MakeWallpaper(Image image)
        {
            var originalwidth = image.Width;

            Image leftDetail = GetDetail(image);
            Image rightDetail = GetDetail(image);

            // expand canvas
            image.Mutate(x =>
            {
                x.Pad(FinalSize.Width, image.Height, Color.Transparent);
            });

            ApplyDetails(image, leftDetail, rightDetail, originalwidth);
        }

        private static void ApplyDetails(Image image, Image leftDetail, Image rightDetail, int originalImageWidth)
        {
            var leftDetailLocation = new Point(0, 0);
            var rightDetailLocation = new Point(leftDetail.Width + originalImageWidth, 0);

            image.Mutate(x =>
            {
                x.DrawImage(leftDetail, leftDetailLocation, 1f);
                x.DrawImage(rightDetail, rightDetailLocation, 1f);
            });
        }

        private Size DetailSize(Image image) => new((FinalSize.Width - image.Width) / 2, FinalSize.Height);

        private Image GetDetail(Image image)
        {
            Image randomPortion = PickRandomPortion(image);

            return randomPortion.Clone(x =>
            {
                // size it back up as necessary
                x.Resize(DetailSize(image));

                x.BoxBlur();

                // add dark tint
                x.SetGraphicsOptions(o =>
                {
                    o.ColorBlendingMode = PixelColorBlendingMode.Multiply;
                });

                x.Fill(new Argb32(0, 0, 0, 180));
            });

        }

        private Image PickRandomPortion(Image image)
        {
            var subsection = _generator.GetRandomSubsection(image.Bounds());

            return image.Clone(x =>
            {
                x.Crop(subsection);
            });
        }
    }
}