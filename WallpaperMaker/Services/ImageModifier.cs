using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Numerics;
using WallpaperMaker.Services;
using Color = SixLabors.ImageSharp.Color;
using Image = SixLabors.ImageSharp.Image;

namespace WallpaperMaker.Pages
{
    public class ImageModifier
    {
        private readonly RandomPortionGenerator _generator;

        public int FinalHeight { get; private set; } = 1080;

        private Size FinalSize = new(1920, 1080);

        public decimal PanelFade { get; set; } = 0.2m;
        public int TransparencyLevel => (int)(255 * PanelFade);
        public Color BlackTint => new(new Vector4(TransparencyLevel, 255, 255, 255));

        private readonly ResizeOptions resizeOptions;

        public ImageModifier(RandomPortionGenerator generator)
        {
            _generator = generator;

            resizeOptions = new ResizeOptions()
            {
                Size = new Size(0, FinalHeight),
                Mode = ResizeMode.Pad
            };
        }

        internal Image Convert(Image image)
        {
            image.Mutate(x =>
            {
                x.Resize(resizeOptions);
            });

            if (image.Width < FinalSize.Width)
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

            return image;
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

                x.Fill(BlackTint);
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