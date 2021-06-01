using System.Drawing;

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

        public void Draw(Graphics g)
        {
            Draw(g, new Point(0, 0), new Size(image.Width, image.Height));
        }

        public void Draw(Graphics g, Point startSlice, Size sizeSlice)
        {
            g.DrawImage(image,
                X + (followCamera ? GameRender.Camera.X : 0), Y + (followCamera ? GameRender.Camera.Y : 0),
                new Rectangle(startSlice, sizeSlice),
                GraphicsUnit.Pixel);
        }
    }

}
