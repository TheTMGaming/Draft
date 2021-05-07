using System.Drawing;
using Top_Down_shooter.Properties;

namespace Top_Down_shooter.Scripts.GameObjects
{
    class Box : GameObject
    {
        public readonly Bitmap Image = new Bitmap(Resources.Box);

        public int Health { get; set; }

        public static readonly int MaxHealth = 10;

        public Box(int x, int y)
        {
            Health = 10;
            Size = new Size(int.Parse(Resources.TileSize), int.Parse(Resources.TileSize));
            X = x;
            Y = y;
        }

        public void DoDamage(int force)
        {
            Health -= force;

            if (Health < 0) Health = 0;
        }
    }
}
