using System.Drawing;
using Top_Down_shooter.Scripts.GameObjects;
using Top_Down_shooter.Scripts.Source;
using unvell.D2DLib;

namespace Top_Down_shooter.Scripts.Renders
{
    class TileRender : IRender
    {
        public int X => tile.X - Size.Width / 2;

        public int Y => tile.Y - Size.Height / 2;

        public Size Size => image.Size;

        public GameObject Parent => tile;

        private readonly GameObject tile;
        private readonly Bitmap image;

        public TileRender(GameObject tile, Bitmap image)
        {
            this.tile = tile;
            this.image = image;
        }

        public void Draw(D2DGraphicsDevice device)
        {
            device.Graphics.DrawBitmap(device.CreateBitmap(image),
                new D2DRect(X, Y, Size.Width, Size.Height));
        }
    }
}
