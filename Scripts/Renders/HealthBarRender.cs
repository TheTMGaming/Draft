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

        private readonly Point offsetBackground = new Point(45, 50);
        private readonly Point offsetBar = new Point(48, 53);

        public HealthBarRender(HealthBar healthBar, int xLeft, int yTop)
        {
            this.healthBar = healthBar;
            X = xLeft;
            Y = yTop;

            heart = new ImageRender(X, Y, Resources.Heart);

            background = new ImageRender(
                X + offsetBackground.X, Y + offsetBackground.Y, Resources.BackgroundHealthBar);

            bar = new ImageRender(X + offsetBar.X, Y + offsetBar.Y, Resources.HealthBar);
        }

        public void Draw(Graphics g)
        {
            background.Draw(g);

            bar.Draw(g, new Point(0, 0), new Size(bar.Size.Width * healthBar.Percent / 100, bar.Size.Height));

            heart.Draw(g);
        }
    }
}
