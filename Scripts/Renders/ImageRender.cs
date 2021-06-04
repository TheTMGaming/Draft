using System.Drawing;
using Top_Down_shooter.Scripts.GameObjects;
using Top_Down_shooter.Scripts.Source;
using unvell.D2DLib;

namespace Top_Down_shooter.Scripts.Renders
{
    class ImageRender : IRender
    {
        public int X => (!(parent is null) ? parent.X - Size.Width / 2 : 0) + startX + (followCamera ? GameRender.Camera.X : 0);
        public int Y => (!(parent is null) ? parent.Y - Size.Height / 2 : 0) + startY + (followCamera ? GameRender.Camera.Y : 0);
        public Size Size => image.Size;

        public GameObject Parent => parent;

        private readonly Bitmap image;
        private readonly bool followCamera;

        private readonly int startX;
        private readonly int startY;

        private readonly GameObject parent;

        public ImageRender(int xLeft, int yTop, Bitmap image, bool followCamera = false, GameObject parent = null)
        {
            startX = xLeft;
            startY = yTop;
            this.image = image;
            this.followCamera = followCamera;
            this.parent = parent;
        }

        public void Draw(D2DGraphicsDevice device)
        {
            Draw(device, new Point(0, 0), new Size(image.Width, image.Height));
        }

        public void Draw(D2DGraphicsDevice device, Point startSlice, Size sizeSlice)
        {
            device.Graphics.DrawBitmap(device.CreateBitmap(image),
                new D2DRect(X, Y, sizeSlice.Width, sizeSlice.Height),
                new D2DRect(startSlice.X, startSlice.Y, sizeSlice.Width, sizeSlice.Height));
        }
    }

}
