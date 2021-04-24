using System.Drawing;
using System;
using Top_Down_shooter.Scripts.Controllers;

namespace Top_Down_shooter.Scripts.Renders
{
    class TileMapRender : IRender
    {
        private readonly int width;
        private readonly int height;

        private readonly TileMapController map;
        
        private readonly int sizeTile = 64;


        public TileMapRender(TileMapController map)
        {
            this.map = map;
            width = map.Width * sizeTile;
            height = map.Height * sizeTile;
            tiles = new SpriteRender[map.Width, map.Height];
        }

        
        public void Draw(Graphics g)
        {
            foreach (var tile in tiles)
            {
                tile.Draw(g);
            }
        }
    }
}
