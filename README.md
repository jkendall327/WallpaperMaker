# WallpaperMaker

A barebones ASP.NET Core/Razor Pages webapp + API for converting images of arbitrary size to 1920x1080 wallpapers.

Input images are resized as necessary and padded horizontally to reach 1920px. Random snapshots of the image are then used to fill the empty space.

This project is at a very early stage of development, as I'm learning ASP.NET Core while making it.

## Known issues

* The resulting wallpaper isn't good.
    * The subsections have different aspect ratios from the main image, making them look squished or stretched.
    * The blur and tint effect is nowhere near as strong as I want.
* The app's UI isn't good.
    * It's mostly the default Razor pages template.
    * Lots of CSS properties are hardcoded pixel amounts.
* The backend logic isn't good.
    * The entire flow of uploading an image, seeing it, then seeing the converted version is messy. I may try to rewrite the entire project as an Angular app instead.

## Credits

TBD

## License

MIT
