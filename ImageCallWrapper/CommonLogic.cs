using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.CompilerServices;

namespace ImageCallWrapper
{
    public static class CommonLogic
    {
        public static void CheckAndGetResizeDimensions(Image imageToResize, int maxHeightOrWidthInPixels,
            out int newHeight, out int newWidth)
        {
            var width = imageToResize.Width;
            var height = imageToResize.Height;

            if (width < 0)
                throw new ArgumentOutOfRangeException(nameof(imageToResize), "Image width must be at least one pixel.");
            if (height < 0)
                throw new ArgumentOutOfRangeException(nameof(imageToResize),
                    "Image height must be at least one pixel.");

            var largerOfTwo = width > height ? width : height;
            var scale = maxHeightOrWidthInPixels / (double) largerOfTwo;
            newWidth = (int) (width * scale);
            newHeight = (int) (height * scale);
            if (newWidth == 0)
                throw new ArgumentOutOfRangeException(nameof(newWidth),
                    "New image width was calculated to be zero and will be worthless.");
            if (newHeight == 0)
                throw new ArgumentOutOfRangeException(nameof(newHeight),
                    "New image height was calculated to be zero and will be worthless.");
        }

        public static string GetStandardFileExtensionForImageFormat(ImageFormat imageFormat) => imageFormat.ToString().ToLowerInvariant().Replace("jpeg","jpg").Replace("memorybmp", "bmp");

        public static string GetTempFileName(ImageFormat imageFormat)
        {
            return Path.GetTempPath() + Guid.NewGuid() + GetStandardFileExtensionForImageFormat(imageFormat);
        }
        public static string GetTempFileName(Image image)
        {
            return GetTempFileName(image.RawFormat);
        }
        public static Image ConvertToFormat(this Image image, ImageFormat imageFormat) => image.RawFormat.Equals(imageFormat) ? image : Image.FromStream(image.ToMemoryStream(imageFormat));
    
        //<remarks>uses default format of image if not specified.
        public static MemoryStream ToMemoryStream(this Image image, ImageFormat imageFormat = null)
        {
            using var imageToResizeStream = new MemoryStream(); //will auto-dispose at end.
            image.Save(imageToResizeStream, imageFormat ?? (image.RawFormat));
            return imageToResizeStream;
        }

        public static MemoryStream ImageMemoryStreamFromFile(string fileName)
        {
            if (!File.Exists(fileName)) throw new FileNotFoundException($"{fileName} was not found.");
            Image image = null;
            try
            {
                image = Image.FromFile(fileName);
            }
            catch (OutOfMemoryException)
            {
                throw new ArgumentOutOfRangeException(nameof(fileName),
                    $"\"{fileName}\" must be a valid graphics file.");
            }

            return image.ToMemoryStream();
        }
    }
}