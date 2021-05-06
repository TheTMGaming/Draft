using System.Drawing;

namespace Top_Down_shooter.Scripts.Renders
{
    class ImageRender : IAnimationRender
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Size Size => image.Size;

        private readonly Bitmap image;

        public ImageRender(int xLeft, int yTop, Bitmap image)
        {
            X = xLeft;
            Y = yTop;
            this.image = image;
        }

        public void Draw(Graphics g)
        {
            Draw(g, new Point(0, 0), new Size(image.Width, image.Height));
        }

        public void Draw(Graphics g, Point startSlice, Size sizeSlice)
        {
            g.DrawImage(image,
                X, Y,
                new Rectangle(startSlice, sizeSlice),
                GraphicsUnit.Pixel);
        }
    }

}
