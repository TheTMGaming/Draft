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

        private readonly HealthBar healthBar;

        private readonly ImageRender background;
        private readonly ImageRender bar;
        private readonly ImageRender cross;

        private readonly float multiSize;
        private readonly Point offsetBackground = new Point(45, 50);
        private readonly Point offsetBar = new Point(48, 53);

        private static readonly Bitmap crossImage;
        private static readonly Bitmap barImage;
        private static readonly Bitmap backgroundImage;

        static HealthBarRender()
        {
            crossImage = Resources.Cross;
            barImage = Resources.HealthBar;
            backgroundImage = Resources.BackgroundHealthBar;
        }

        public HealthBarRender(HealthBar healthBar, int xLeft, int yTop, int percentSize = 100, bool followCamera = false)
        {
            this.healthBar = healthBar;
            X = xLeft;
            Y = yTop;
            multiSize = percentSize / 100f;

            offsetBackground.X = (int)(offsetBackground.X * multiSize);
            offsetBackground.Y = (int)(offsetBackground.Y * multiSize);
            offsetBar.X = (int)(offsetBar.X * multiSize);
            offsetBar.Y = (int)(offsetBar.Y * multiSize);


            cross = new ImageRender(X, Y, new Bitmap(crossImage, new Size(
                (int)(Resources.Cross.Width * multiSize), (int)(Resources.Cross.Height * multiSize))), followCamera);

            background = new ImageRender(
                X + offsetBackground.X, Y + offsetBackground.Y, new Bitmap(backgroundImage, new Size(
                (int)(Resources.BackgroundHealthBar.Width * multiSize), (int)(Resources.BackgroundHealthBar.Height * multiSize))), followCamera);

            bar = new ImageRender(X + offsetBar.X, Y + offsetBar.Y, new Bitmap(barImage, new Size(
                (int)(Resources.HealthBar.Width * multiSize), (int)(Resources.HealthBar.Height * multiSize))), followCamera);
            
            Size = new Size(X + offsetBackground.X + background.Size.Width, cross.Size.Height);
        }

        public HealthBarRender(HealthBar healthBar, Character character, Point offset, int percentSize = 100)
        {
            this.healthBar = healthBar;
            multiSize = percentSize / 100f;
            offsetBar = new Point((int)(3 * multiSize), (int)(4 * multiSize));

            var backgroundSize = new Size(
                (int)(Resources.BackgroundHealthBar.Width * multiSize), (int)(Resources.BackgroundHealthBar.Height * multiSize));
            var barSize = new Size(
                (int)(Resources.HealthBar.Width * multiSize), (int)(Resources.HealthBar.Height * multiSize));

            X = character.X - backgroundSize.Width / 2 + offset.X;
            Y = character.Y - backgroundSize.Height / 2 + offset.Y;

            background = new ImageRender(
                X, Y, new Bitmap(Resources.BackgroundHealthBar, backgroundSize));

            bar = new ImageRender(X + offsetBar.X, Y + offsetBar.Y, new Bitmap(Resources.HealthBar, barSize));
        }

        public void Draw(D2DGraphicsDevice device)
        {
            background?.Draw(device);

            bar.Draw(device, new Point(0, 0), new Size(bar.Size.Width * healthBar.Percent / 100, bar.Size.Height));

            cross?.Draw(device);
        }
    }
}
