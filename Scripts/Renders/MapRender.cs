using System.Drawing;
using Top_Down_shooter.Scripts.Controllers;
using Top_Down_shooter.Scripts.GameObjects;
using Top_Down_shooter.Scripts.Source;
using unvell.D2DLib;

namespace Top_Down_shooter.Scripts.Renders
{
    class MapRender : IRender
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Size Size => new Size(GameSettings.MapWidth, GameSettings.MapHeight);

        private readonly Map map;
        

        public MapRender(Map map)
        {
            this.map = map;
        }


        public void Draw(D2DGraphicsDevice device)
        {
            var g = device.Graphics;

            foreach (var tile in map.Tiles)
            {
                if (tile is Box box)
                {
                    // box.Image.Blackout((1 - (float)box.Health / Box.MaxHealth) / 2
                    g.DrawBitmap(device.CreateBitmap(box.Image),
                        new D2DRect(box.X - box.Image.Width / 2, box.Y - box.Image.Height / 2, box.Image.Width, box.Image.Height));
                    continue;
                }

                if (tile is Grass grass)
                {
                    g.DrawBitmap(device.CreateBitmap(grass.Image),
                       new D2DRect(grass.X - grass.Image.Width / 2, grass.Y - grass.Image.Height / 2, grass.Image.Width, grass.Image.Height));
                    continue;
                }

                if (tile is Block block)
                {
                    g.DrawBitmap(device.CreateBitmap(block.Image),
                        new D2DRect(block.X - block.Image.Width / 2, block.Y - block.Image.Height / 2, block.Image.Width, block.Image.Height));
                }
            }
        }
    }
}
