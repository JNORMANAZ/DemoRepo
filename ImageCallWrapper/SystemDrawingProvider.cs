using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace ImageCallWrapper
{
    public class SystemDrawingProvider : IResizeImage, IRotateImage
    {
        public Image Resize(Image imageToResize, int maxHeightOrWidthInPixels)
        {
            CommonLogic.CheckAndGetResizeDimensions(imageToResize, maxHeightOrWidthInPixels, out var newHeight,
                out var newWidth);
            imageToResize = ResizeImage(imageToResize, newWidth, newHeight);
            using var imageToResizeStream = new MemoryStream();
            imageToResize.Save(imageToResizeStream, ImageFormat.Png);
            return Image.FromStream(imageToResizeStream);
        }

        public Image Rotate(Image imageToRotate)
        {
            imageToRotate.RotateFlip(RotateFlipType.Rotate180FlipNone);
            return imageToRotate;
        }

        //Because I didn't know how to resize https://stackoverflow.com/questions/1922040/how-to-resize-an-image-c-sharp
        private static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using var graphics = Graphics.FromImage(destImage);
            graphics.CompositingMode = CompositingMode.SourceCopy;
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            using var wrapMode = new ImageAttributes();
            wrapMode.SetWrapMode(WrapMode.TileFlipXY);
            graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);

            return destImage;
        }
    }
}