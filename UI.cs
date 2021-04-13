using System.Drawing;

namespace Top_Down_shooter
{
    class HealthBar
    {
        public int Percent { get; }

        private int x, y;
        public readonly Bitmap BackgroundImage;
        public readonly Bitmap BarImage;
        public readonly Bitmap HeartImage;

        public HealthBar(int x, int y)
        {
            this.x = x;
            this.y = y;
            BackgroundImage = new Bitmap(@"Sprites/BackgroundHealthBar.png");
            BarImage = new Bitmap(@"Sprites/HealthBar.png");
            HeartImage = new Bitmap(@"Sprites/HeartHealthBar.png");
        }

        public void Draw(Graphics g)
        {
            g.DrawImage(HeartImage, x, y, new Rectangle(new Point(0, 0), new Size(HeartImage.Width, HeartImage.Height)),
                GraphicsUnit.Pixel);
        }
        
    }
}
