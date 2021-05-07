using System.Drawing;
using Top_Down_shooter.Properties;
using Top_Down_shooter.Scripts.Source;

namespace Top_Down_shooter.Scripts.GameObjects
{
    class Block : GameObject
    {
        public readonly Bitmap Image = Resources.Block;

        public Block(int x, int y)
        {
            Size = new Size(GameSettings.TileSize, GameSettings.TileSize);
            X = x;
            Y = y;
        }
    }
}
