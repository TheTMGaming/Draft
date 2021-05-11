using System;
using System.Drawing;
using Top_Down_shooter.Properties;
using Top_Down_shooter.Scripts.Source;
using Top_Down_shooter.Scripts.Components;

namespace Top_Down_shooter.Scripts.GameObjects
{
    class Grass : GameObject
    {
        public readonly Bitmap Image;

        private readonly static Random randGenerator = new Random();

        public Grass(int x, int y)
        {
            Image = Resources.Grass.Extract(new Rectangle(64 * randGenerator.Next(0, 4), 0, 64, 64));
            Collider = new Collider(this, 0, 0, GameSettings.TileSize, GameSettings.TileSize);

            X = x;
            Y = y;
        }
    }
}
