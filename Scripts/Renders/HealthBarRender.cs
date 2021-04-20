using System.Drawing;
using Top_Down_shooter.Scripts.UI;

namespace Top_Down_shooter.Scripts.Renders
{
    class HealthBarRender : IRender
    {
        private readonly int x;
        private readonly int y;

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
            x = xLeft;
            y = yTop;

            heart = new SpriteRender(x, y, new Bitmap(@"Sprites/Heart.png"));

            background = new SpriteRender(
                x + offsetBackground.X, y + offsetBackground.Y, new Bitmap(@"Sprites/BackgroundHealthBar.png")
                );

            var imageBar = new Bitmap(@"Sprites/HealthBar.png");
            bar = new SpriteRender(x + offsetBar.X, y + offsetBar.Y, imageBar);
        }

        public void Draw(Graphics g)
        {
            heart.Draw(g);

            background.Draw(g);

            bar.Draw(g, new Point(0, 0), new Size(sizeBar.Width * healthBar.Percent / 100, sizeBar.Height));
        }
    }
}
