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
        private readonly Size sizeBar;

        private readonly SpriteRender background;
        private readonly SpriteRender bar;
        private readonly SpriteRender heart;

        private readonly Point offsetBackground = new Point(140, 20);
        private readonly Point offsetBar = new Point(140, 20);

        public HealthBarRender(HealthBar healthBar, int xLeft, int yTop)
        {
            this.healthBar = healthBar;
            X = xLeft;
            Y = yTop;

            heart = new SpriteRender(X, Y, Resources.Heart);

            background = new SpriteRender(
                X + offsetBackground.X, Y + offsetBackground.Y, Resources.BackgroundHealthBar
                );

            bar = new SpriteRender(X + offsetBar.X, Y + offsetBar.Y, Resources.HealthBar);
        }

        public void Draw(Graphics g)
        {
            heart.Draw(g);

            background.Draw(g);

            bar.Draw(g, new Point(0, 0), new Size(sizeBar.Width * healthBar.Percent / 100, sizeBar.Height));
        }
    }
}
