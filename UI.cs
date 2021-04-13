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

        private readonly Point offsetBackground = new Point(45, 45);
        private readonly Point offsetBarRelativeBack = new Point(2, 3);

        public HealthBar(int xLeft, int yTop, int percent)
        {
            Percent = percent;
            
            heart = new Sprite
            {
                X = xLeft,
                Y = yTop,
                Image = new Bitmap(@"Sprites/Heart.png")
            };

            background = new Sprite
            {
                X = xLeft + offsetBackground.X,
                Y = yTop + offsetBackground.Y,
                Image = new Bitmap(@"Sprites/BackgroundHealthBar.png")
            };

            bar = new Sprite
            {
                X = xLeft + offsetBackground.X + offsetBarRelativeBack.X,
                Y = yTop + offsetBackground.Y + offsetBarRelativeBack.Y,
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
