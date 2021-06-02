using System;
using System.Collections.Generic;
using System.Drawing;
using Top_Down_shooter.Properties;
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

        private static readonly List<Bitmap> grassImage = new List<Bitmap>();
        private static readonly Bitmap boxImage = Resources.Box;
        private static readonly Bitmap blockImage = Resources.Block;
        
        static MapRender()
        {
            for (var i = 0; i < 4; i++)
            {
                grassImage.Add(Resources.Grass.Extract(new Rectangle(64 * i, 0, 64, 64)));
            }
        }

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
                    g.DrawBitmap(device.CreateBitmap(boxImage),
                        new D2DRect(box.X - boxImage.Width / 2, box.Y - boxImage.Height / 2, boxImage.Width, boxImage.Height));
                    continue;
                }

                if (tile is Grass grass)
                {
                    g.DrawBitmap(device.CreateBitmap(grassImage[grass.ID]),
                       new D2DRect(grass.X - grassImage[grass.ID].Width / 2, grass.Y - grassImage[grass.ID].Height / 2, grassImage[grass.ID].Width, grassImage[grass.ID].Height));
                    continue;
                }

                if (tile is Block block)
                {
                    g.DrawBitmap(device.CreateBitmap(blockImage),
                        new D2DRect(block.X - blockImage.Width / 2, block.Y - blockImage.Height / 2, blockImage.Width, blockImage.Height));
                }
            }
        }
    }
}
