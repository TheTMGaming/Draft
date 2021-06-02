using System.Drawing;
using Top_Down_shooter.Scripts.Source;
using unvell.D2DLib;

namespace Top_Down_shooter.Scripts.Renders
{
    class ImageRender : IRender
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Size Size => image.Size;

        private readonly Bitmap image;
        private readonly bool followCamera;

        public ImageRender(int xLeft, int yTop, Bitmap image, bool followCamera = false)
        {
            X = xLeft;
            Y = yTop;
            this.image = image;
            this.followCamera = followCamera;
        }

        public void Draw(D2DGraphicsDevice device)
        {
            Draw(device, new Point(0, 0), new Size(image.Width, image.Height));
        }

        public void Draw(D2DGraphicsDevice device, Point startSlice, Size sizeSlice)
        {
            device.Graphics.DrawBitmap(device.CreateBitmap(image),
                new D2DRect(X + (followCamera ? GameRender.Camera.X : 0), Y + (followCamera ? GameRender.Camera.Y : 0), sizeSlice.Width, sizeSlice.Height),
                new D2DRect(startSlice.X, startSlice.Y, sizeSlice.Width, sizeSlice.Height));
        }
    }

}
