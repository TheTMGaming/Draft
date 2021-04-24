using System;
using System.Drawing;
using Top_Down_shooter.Scripts.Renders;

namespace Top_Down_shooter.Scripts.Controllers
{
    enum TileTypes
    {
        Grass, Box
    }

    class TileMapController
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public readonly SpriteRender[,] Tiles;

        private readonly int sizeTile = 64;
        private readonly Bitmap grassImages = new Bitmap("Sprites/Grass.png");
        private readonly Bitmap boxImage = new Bitmap("Sprites/Box.png");

        public TileMapController(int width, int height)
        {
            Width = width;
            Height = height;
            Tiles = new SpriteRender[Width / sizeTile, Height / sizeTile];

        }

       
        public void CreateTile()
        {
            var rand = new Random();

            for (var x = 0; x < Width; x += sizeTile)
            {
                for (var y = 0; y < Height; y += sizeTile)
                {
                    var image = grassImages.Extract(new Rectangle(sizeTile * rand.Next(0, 4), 0, sizeTile, sizeTile));

                    if (rand.NextDouble() > .95)
                    {
                        image = new Bitmap("Sprites/Box.png");

                    }

                    tiles[x / sizeTile, y / sizeTile] = new SpriteRender(x, y, image);
                }
            }
        }

    }
}
