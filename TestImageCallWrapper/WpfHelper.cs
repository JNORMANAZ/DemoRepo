using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;

namespace TestImageCallWrapper
{
    public static class WpfHelper
    {
        //https://stackoverflow.com/questions/6364524/pngbitmapdecoder-stream-question
        //but adapted to use a System.Drawing.Image
        public static BitmapSource ConvertImageToBitmapSource(Image image)
        {
            using MemoryStream imageAsStream = new();
            image.Save(imageAsStream, ImageFormat.Bmp);
            imageAsStream.Seek(0, SeekOrigin.Begin);
            var buffer = imageAsStream.ToArray();
            using MemoryStream stream = new(buffer);
            var bitmapDecoder =
                BitmapDecoder.Create(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
            var frame = bitmapDecoder.Frames.First();
            frame.Freeze();
            return frame;
        }
    }
}