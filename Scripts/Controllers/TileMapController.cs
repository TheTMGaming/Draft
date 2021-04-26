using System;
using System.Drawing;
using Top_Down_shooter.Scripts.Renders;
using Top_Down_shooter.Properties;

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

        private static int sizeTile = int.Parse(Resources.TileSize);

        static TileMapController()
        {
            Width = int.Parse(Resources.MapWidth);
            Height = int.Parse(Resources.MapHeight);
            Tiles = new SpriteRender[Width / sizeTile, Height / sizeTile];          
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
                        var box = new SpriteRender(x * sizeTile, y * sizeTile, Resources.Box);
                        Tiles[x, y] = box;
                        PhysicsController.AddCollider(box);
                    }
                    else
                    {
                        Tiles[x, y] = new SpriteRender(x * sizeTile, y * sizeTile,
                            Resources.Grass.Extract(new Rectangle(sizeTile * rand.Next(0, 4), 0, sizeTile, sizeTile)));
                    }
                }
            }
        }

        public static void DrawTile(Graphics g)
        {
            foreach (var tile in Tiles)
            {
                tile.Draw(g);
            }
        }

    }
}
