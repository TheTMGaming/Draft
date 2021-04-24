using System;
using System.Drawing;
using Top_Down_shooter.Scripts.Renders;

namespace Top_Down_shooter.Scripts.Controllers
{
    enum TileTypes
    {
        Grass, Box
    }

    static class TileMapController
    {
        public static int Width;
        public static int Height;
        public static SpriteRender[,] Tiles;

        private static int sizeTile = 64;
        private static Bitmap grassImages = new Bitmap("Sprites/Grass.png");
        private static Bitmap boxImage = new Bitmap("Sprites/Box.png");

        static TileMapController()
        {
            Tiles = new SpriteRender[1920 / sizeTile, 1080 / sizeTile];
            Width = 1920;
            Height = 1080;
        }

       
        public static void CreateTile()
        {
            var rand = new Random();

            for (var x = 0; x < Width / sizeTile; x += 1)
            {
                for (var y = 0; y < Height / sizeTile; y += 1)
                {
                
                    if (rand.NextDouble() > .95)
                    {
                        var box = new SpriteRender(x * sizeTile, y * sizeTile, boxImage);
                        Tiles[x, y] = box;
                        PhysicsController.AddCollider(box);
                    }
                    else
                    {
                        Tiles[x, y] = new SpriteRender(x * sizeTile, y * sizeTile,
                            grassImages.Extract(new Rectangle(sizeTile * rand.Next(0, 4), 0, sizeTile, sizeTile)));
                    }
                }
            }
        }

        public static void DrawTile(Graphics g)
        {
            foreach (var tile in Tiles)
                tile.Draw(g);
        }

    }
}
