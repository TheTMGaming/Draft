using System.Drawing;
using Top_Down_shooter.Properties;

namespace Top_Down_shooter.Scripts.GameObjects
{
    class Grass : GameObject
    {
        public Grass(int x, int y)
        {
            IsTrigger = true;
            Size = new Size(int.Parse(Resources.TileSize), int.Parse(Resources.TileSize));
            X = x;
            Y = y;
        }
    }
}
