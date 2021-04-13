using System.Drawing;

namespace Top_Down_shooter
{
    class Sprite
    {
        public int X { get; set; }
        public int Y { get; set; }
        public float Angle { get; set; }
        public Bitmap Image { get; set; }

        public virtual void Draw(Graphics g)
        {
            Draw(g, new Point(0, 0), new Size(Image.Width, Image.Height));
        }

        public void Draw(Graphics g, Point startSlice, Size sizeSlice)
        {
            g.DrawImage(
               Image, X - Image.Width / 2, Y - Image.Height / 2,
               new Rectangle(startSlice, sizeSlice),
               GraphicsUnit.Pixel);
        }

        public virtual void Move() { }

        public virtual void Move(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
