using System.Drawing;

namespace ImageCallWrapper
{
    public interface IRotateImage
    {
        public Image Rotate(Image imageToRotate);
    }
}