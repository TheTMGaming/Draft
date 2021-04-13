using System.Drawing;

namespace Top_Down_shooter
{
    class HealthBar
    {
        public int Percent { get; }

        private readonly int x, y;
        private readonly Bitmap backgroundImage;
        private readonly Bitmap barImage;
        private readonly Bitmap heartImage;

        private readonly Point offsetBackground;
        private readonly Point offsetBar;

        public HealthBar(int x, int y)
        {
            this.x = x;
            this.y = y;
            backgroundImage = new Bitmap(@"Sprites/BackgroundHealthBar.png");
            barImage = new Bitmap(@"Sprites/HealthBar.png");
            heartImage = new Bitmap(@"Sprites/Heart.png");

            offsetBackground = new Point(45, 45);
            offsetBar = new Point(offsetBackground.X + 2, offsetBackground.Y + 3);
    }

        public void Draw(Graphics g)
        {
            g.DrawImage(
                backgroundImage, x + offsetBackground.X, y + offsetBackground.Y,
                new Rectangle(new Point(0, 0), new Size(backgroundImage.Width, backgroundImage.Height)),
                GraphicsUnit.Pixel);

            g.DrawImage(
                barImage, x + offsetBar.X, y + offsetBar.Y,
                new Rectangle(new Point(0, 0), new Size(barImage.Width, barImage.Height)),
                GraphicsUnit.Pixel);

            g.DrawImage(
                heartImage, x, y,
                new Rectangle(new Point(0, 0), new Size(heartImage.Width, heartImage.Height)),
                GraphicsUnit.Pixel);
        }
        
    }
}
