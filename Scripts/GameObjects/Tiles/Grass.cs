using System;
using System.Drawing;
using Top_Down_shooter.Properties;

namespace Top_Down_shooter.Scripts.GameObjects
{
    class Grass : GameObject
    {
        public readonly Bitmap Image;

        private readonly static Random randGenerator = new Random();

        public Grass(int x, int y)
        {
            Image = Resources.Grass.Extract(new Rectangle(64 * randGenerator.Next(0, 4), 0, 64, 64));

            IsTrigger = true;
            Size = new Size(int.Parse(Resources.TileSize), int.Parse(Resources.TileSize));

            X = x;
            Y = y;
        }
    }
}
