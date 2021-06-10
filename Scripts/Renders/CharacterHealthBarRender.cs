using System.Drawing;
using Top_Down_shooter.Scripts.GameObjects;
using Top_Down_shooter.Scripts.Source;

namespace Top_Down_shooter.Scripts.Renders
{
    class CharacterHealthBarRender : IRender
    {
        public int X => character.X - Size.Width / 2 + offset.X;
        public int Y => character.Y - Size.Height / 2 + offset.Y;
        public Size Size => GameImages.HealthBarBackgroundEnemy.Size;

        public GameObject Parent => character;

        private readonly Character character;

        private readonly Point offset;

        private readonly ImageRender background;
        private readonly ImageRender bar;

        private static Point offsetBar = new Point(1, 0);

        public CharacterHealthBarRender(Boss boss, Point offset) : this((Character)boss, offset)
        {
            offsetBar = new Point(0, 0);
            background = new ImageRender(offset.X, offset.Y, GameImages.HealthBarBackgroundBoss, parent: boss);
            bar = new ImageRender(offset.X + offsetBar.X, offset.Y + offsetBar.Y, GameImages.HealthBarBoss, parent: boss);
        }

        public CharacterHealthBarRender(Enemy enemy, Point offset) : this((Character)enemy, offset)
        {
            background = new ImageRender(offset.X, offset.Y, GameImages.HealthBarBackgroundEnemy, parent: enemy);
            bar = new ImageRender(offset.X + offsetBar.X, offset.Y + offsetBar.Y, GameImages.HealthBarEnemy, parent: enemy);
        }

        public CharacterHealthBarRender(Character character, Point offset)
        {
            this.character = character;
            this.offset = offset;
        }

        public void Draw(D2DGraphicsDevice device)
        {
            background.Draw(device);

            bar.Draw(device, new Point(0, 0), new Size(bar.Size.Width * character.HealthBar.Percent / 100, bar.Size.Height));
        }
    }
}
