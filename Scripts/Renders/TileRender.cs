using System;
using System.Collections.Generic;
using System.Drawing;
using Top_Down_shooter.Properties;
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
        private Bitmap image;

        private static Dictionary<int, Bitmap> statesBox = new Dictionary<int, Bitmap>();

        static TileRender()
        {
            var image = Resources.Box;

            for (var hp = 1; hp < GameSettings.BoxHealth + 1; hp++)
            {
                statesBox[hp] = image.Blackout((1 - (float)hp / GameSettings.BoxHealth) / 2);
            }
        }

        public TileRender(GameObject tile, Bitmap image)
        {
            this.tile = tile;
            this.image = image;
        }

        public void Draw(D2DGraphicsDevice device)
        {
            if (tile is Box box)
                image = statesBox[box.Health];

            device.Graphics.DrawBitmap(device.CreateBitmap(image),
                new D2DRect(X, Y, Size.Width, Size.Height));
        }
    }
}
