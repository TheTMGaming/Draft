using System.Drawing;
using System;
using Top_Down_shooter.Scripts.Controllers;
using System.Collections.Generic;
using Top_Down_shooter.Scripts.GameObjects;
using Top_Down_shooter.Properties;
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


        public void Draw(D2DGraphics g)
        {
            foreach (var tile in map.Tiles)
            {
                if (tile is Box box)
                {
                    g.DrawBitmap(g.Device.CreateBitmapFromGDIBitmap(box.Image.Blackout((1 - (float)box.Health / Box.MaxHealth) / 2)),
                        new D2DRect(box.X - box.Image.Width / 2, box.Y - box.Image.Height / 2, box.Image.Width, box.Image.Height));
                    continue;
                }

                if (tile is Grass grass)
                {
                    g.DrawBitmap(g.Device.CreateBitmapFromGDIBitmap(grass.Image),
                       new D2DRect(grass.X - grass.Image.Width / 2, grass.Y - grass.Image.Height / 2, grass.Image.Width, grass.Image.Height));
                    continue;
                }

                if (tile is Block block)
                {
                    g.DrawBitmap(g.Device.CreateBitmapFromGDIBitmap(block.Image),
                        new D2DRect(block.X - block.Image.Width / 2, block.Y - block.Image.Height / 2, block.Image.Width, block.Image.Height));
                }
            }
        }
    }
}
