using System.Drawing;
using Top_Down_shooter.Properties;
using Top_Down_shooter.Scripts.Components;
using Top_Down_shooter.Scripts.Source;

namespace Top_Down_shooter.Scripts.GameObjects
{
    class Box : GameObject
    {
        public int Health { get; set; }

        public Box(int x, int y)
        {
            Health = GameSettings.BoxHealth;
            Collider = new Collider(this, 0, 0, GameSettings.TileSize, GameSettings.TileSize);
            X = x;
            Y = y;
        }
    }
}
