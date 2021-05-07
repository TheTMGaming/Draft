using System.Drawing;
using Top_Down_shooter.Properties;

namespace Top_Down_shooter.Scripts.GameObjects
{
    class Block : GameObject
    {
        public readonly Bitmap Image = Resources.Block;

        public Block(int x, int y)
        {
            Size = new Size(int.Parse(Resources.TileSize), int.Parse(Resources.TileSize));
            X = x;
            Y = y;
        }
    }
}
