using System.Drawing;

namespace Top_Down_shooter
{
    interface IUserInterface
    {
        void Draw(Graphics g);
    }

    class HealthBar : IUserInterface
    {
        public int Percent
        {
            get { return percent; }
            set
            {
                percent = value;
                if (percent < 0) percent = 0;
                if (percent > 100) percent = 100;
            }
        }

        private int percent;

        private readonly Sprite background;
        private readonly Sprite bar;
        private readonly Sprite heart;

        private readonly Point offsetBackground = new Point(140, 20);
        private readonly Point offsetBar = new Point(140, 20);

        public HealthBar(int x, int y, int percent)
        {
            Percent = percent;
            
            heart = new Sprite
            {
                X = x,
                Y = y,
                Image = new Bitmap(@"Sprites/Heart.png")
            };

            background = new Sprite
            {
                X = x + offsetBackground.X,
                Y = y + offsetBackground.Y,
                Image = new Bitmap(@"Sprites/BackgroundHealthBar.png")
            };

            bar = new Sprite
            {
                X = x + offsetBar.X,
                Y = y + offsetBar.Y,
                Image = new Bitmap(@"Sprites/HealthBar.png")
            };
        }

        public void Draw(Graphics g)
        {
            background.Draw(g);

            bar.Draw(g, new Point(0, 0), new Size(bar.Image.Width * Percent / 100, bar.Image.Height));

            heart.Draw(g);
        }       
    }
}
