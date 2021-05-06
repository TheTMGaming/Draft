using System.Drawing;
using System;
using Top_Down_shooter.Scripts.Controllers;
using System.Collections.Generic;
using Top_Down_shooter.Scripts.GameObjects;
using Top_Down_shooter.Properties;

namespace Top_Down_shooter.Scripts.Renders
{
    class MapRender : IRender
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Size Size => new Size(int.Parse(Resources.MapWidth), int.Parse(Resources.MapHeight));

        private readonly Map map;
        

        public MapRender(Map map)
        {
            this.map = map;
        }


        public void Draw(Graphics g)
        {
            foreach (var tile in map.Tiles)
            {
                if (tile is Box box)
                {
                    g.DrawImage(box.Image.Blackout((1 - (float)box.Health / Box.MaxHealth) / 2),
                        box.X - box.Image.Width / 2, box.Y - box.Image.Height / 2,
                        new Rectangle(0, 0, box.Image.Width, box.Image.Height),
                        GraphicsUnit.Pixel);
                    continue;
                }

                if (tile is Grass grass)
                {
                    g.DrawImage(grass.Image,
                        grass.X - grass.Image.Width / 2, grass.Y - grass.Image.Height / 2,
                        new Rectangle(0, 0, grass.Image.Width, grass.Image.Height),
                        GraphicsUnit.Pixel);
                }
            }
        }
    }
}
