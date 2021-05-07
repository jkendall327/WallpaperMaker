using System.Drawing;

namespace WallpaperMaker.Pages
{
    public class ImageModifier
    {
        internal Image Convert(Image image)
        {
            image.RotateFlip(RotateFlipType.Rotate180FlipX);
            return image;
        }
    }
}