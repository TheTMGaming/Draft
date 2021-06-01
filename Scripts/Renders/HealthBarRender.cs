using System.Drawing;
using Top_Down_shooter.Properties;
using Top_Down_shooter.Scripts.UI;

namespace Top_Down_shooter.Scripts.Renders
{
    class HealthBarRender : IRender
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Size Size => new Size(X + offsetBackground.X + background.Size.Width, heart.Size.Height);

        private readonly HealthBar healthBar;

        private readonly ImageRender background;
        private readonly ImageRender bar;
        private readonly ImageRender heart;

        private readonly float multiSize;
        private readonly Point offsetBackground = new Point(45, 50);
        private readonly Point offsetBar = new Point(48, 53);

        public HealthBarRender(HealthBar healthBar, int xLeft, int yTop, int percentSize = 100)
        {
            this.healthBar = healthBar;
            X = xLeft;
            Y = yTop;
            multiSize = percentSize / 100;

            offsetBackground.X = (int)(offsetBackground.X * multiSize);
            offsetBackground.Y = (int)(offsetBackground.Y * multiSize);
            offsetBar.X = (int)(offsetBar.X * multiSize);
            offsetBar.Y = (int)(offsetBar.Y * multiSize);

            heart = new ImageRender(X, Y, new Bitmap(Resources.Cross, new Size(
                (int)(Resources.Cross.Width * multiSize), (int)(Resources.Cross.Height * multiSize))));

            background = new ImageRender(
                X + offsetBackground.X, Y + offsetBackground.Y, new Bitmap(Resources.BackgroundHealthBar, new Size(
                (int)(Resources.BackgroundHealthBar.Width * multiSize), (int)(Resources.BackgroundHealthBar.Height * multiSize))));

            bar = new ImageRender(X + offsetBar.X, Y + offsetBar.Y, new Bitmap(Resources.HealthBar, new Size(
                (int)(Resources.HealthBar.Width * multiSize), (int)(Resources.HealthBar.Height * multiSize))));
        }

        public void Draw(Graphics g)
        {
            background.Draw(g);

            bar.Draw(g, new Point(0, 0), new Size(bar.Size.Width * healthBar.Percent / 100, bar.Size.Height));

            heart.Draw(g);
        }
    }
}
