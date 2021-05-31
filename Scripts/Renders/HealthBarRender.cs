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
            DrawPart(g, background, 
                offsetBackground.X + GameRender.Camera.X, offsetBackground.Y + GameRender.Camera.Y,
                background.Size.Width, background.Size.Height);

            DrawPart(g, bar,
                offsetBar.X + GameRender.Camera.X, offsetBar.Y + GameRender.Camera.Y,
                bar.Size.Width * healthBar.Percent / 100, bar.Size.Height);

            DrawPart(g, heart, GameRender.Camera.X, GameRender.Camera.Y, heart.Size.Width, heart.Size.Height);
        }

        private void DrawPart(Graphics g, ImageRender render, int offsetX, int offsetY, int widthSlice, int heighSlice)
        {
            render.X = X + offsetX;
            render.Y = Y + offsetY;
            render.Draw(g, new Point(0, 0), new Size(widthSlice, heighSlice));
        }
    }
}
