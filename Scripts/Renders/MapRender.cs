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
        private readonly List<IRender> tileRenders = new List<IRender>();

        public MapRender(Map map)
        {
            this.map = map;

            foreach (var tile in map.Tiles)
            {
                if (tile is Box)
                {
                    tileRenders.Add(new BoxRender(tile as Box));
                    continue;
                }

                if (tile is Grass)
                    tileRenders.Add(new GrassRender(tile as Grass));
            }
        }


        public void Draw(Graphics g)
        {
            foreach (var render in tileRenders)
                render.Draw(g);
        }
    }
}
