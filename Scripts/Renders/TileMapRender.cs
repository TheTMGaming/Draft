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
        private readonly SpriteRender[,] tiles;
        private readonly int sizeTile = 64;

        private readonly Bitmap grassImages = new Bitmap("Sprites/Grass.png");
        private readonly Bitmap boxImage = new Bitmap("Sprites/Box.png");

        public TileMapRender(TileMapController map)
        {
            this.map = map;
            width = map.Width * sizeTile;
            height = map.Height * sizeTile;
            tiles = new SpriteRender[map.Width, map.Height];

            CreateTile();
        }

        public void CreateTile()
        {
            var rand = new Random();

            for (var x = 0; x < width; x += sizeTile)
            {
                for (var y = 0; y < height; y += sizeTile)
                {
                    var image = new Bitmap("Sprites/Grass.png");

                    if (map.Tiles[x / sizeTile, y / sizeTile] == TileTypes.Grass)
                    {                      
                        image = image.Extract(new Rectangle(64 * rand.Next(0, 4), 0, 64, 64));
                    }
                    else
                        image = new Bitmap("Sprites/Box.png");

                    tiles[x / sizeTile, y / sizeTile] = new SpriteRender(x, y, image);
                }
            }
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
