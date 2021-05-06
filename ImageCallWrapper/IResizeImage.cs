using System.Drawing;

namespace ImageCallWrapper
{
    public interface IResizeImage
    {
        public Image Resize(Image imageToResize, int maxHeightOrWidthInPixels);
    }
}