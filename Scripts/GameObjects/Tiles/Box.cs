using System.Drawing;
using Top_Down_shooter.Properties;

namespace Top_Down_shooter.Scripts.GameObjects
{
    class Box : GameObject
    { 
        public int Health { get; set; }

        public Box(int x, int y)
        {
            Health = 10;
            Size = new Size(int.Parse(Resources.TileSize), int.Parse(Resources.TileSize));
            X = x;
            Y = y;
        }
    }
}
