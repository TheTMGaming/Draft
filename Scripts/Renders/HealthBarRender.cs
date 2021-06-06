using System.Drawing;
using Top_Down_shooter.Properties;
using Top_Down_shooter.Scripts.GameObjects;
using Top_Down_shooter.Scripts.Source;
using Top_Down_shooter.Scripts.UI;

namespace Top_Down_shooter.Scripts.Renders
{
    class HealthBarRender : IRender
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Size Size { get; set; }

        public GameObject Parent => healthBar;

        private readonly HealthBar healthBar;

        private readonly ImageRender background;
        private readonly ImageRender bar;
        private readonly ImageRender cross;

        private readonly Point offsetBackground = new Point(45, 50);
        private readonly Point offsetBar = new Point(48, 53);

        public HealthBarRender(HealthBar healthBar, int xLeft, int yTop)
        {
            this.healthBar = healthBar;
            X = xLeft;
            Y = yTop;

            cross = new ImageRender(X, Y, GameImages.HealthBarCross, followCamera: true);

            background = new ImageRender(
                X + offsetBackground.X, Y + offsetBackground.Y, GameImages.HealthBarBackgroundPlayer, followCamera: true);

            bar = new ImageRender(X + offsetBar.X, Y + offsetBar.Y, GameImages.HealthBarPlayer, followCamera: true);
            
            Size = new Size(X + offsetBackground.X + background.Size.Width, cross.Size.Height);
        }

        public void Draw(D2DGraphicsDevice device)
        {
            background.Draw(device);

            bar.Draw(device, new Point(0, 0), new Size(bar.Size.Width * healthBar.Percent / 100, bar.Size.Height));

            cross.Draw(device);
        }
    }
}
