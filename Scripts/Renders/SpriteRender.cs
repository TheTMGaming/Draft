using System.Drawing;

namespace Top_Down_shooter.Scripts.Renders
{
    class SpriteRender : IRender, IAnimationRender
    {
        private readonly int x, y;
        private readonly Bitmap image;

        public SpriteRender(int xLeft, int yTop, Bitmap image)
        {
            x = xLeft;
            y = yTop;
            this.image = image;
        }

        public void Draw(Graphics g)
        {
            Draw(g, new Point(0, 0), new Size(image.Width, image.Height));
        }

        public void Draw(Graphics g, Point startSlice, Size sizeSlice)
        {
            g.DrawImage(image,
                x, y,
                new Rectangle(startSlice, sizeSlice),
                GraphicsUnit.Pixel);
        }
    }

}
