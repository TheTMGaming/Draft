using System;
using System.Drawing;
using Top_Down_shooter.Scripts.GameObjects;
using Top_Down_shooter.Scripts.Renders;
using Top_Down_shooter.Properties;

namespace Top_Down_shooter.Scripts.Controllers
{
    enum TileTypes
    {
        Grass, Box
    }

    class Map
    {
        public readonly Point PositionPlayer;
        public readonly Point PositionBoss;
        public readonly Point[] PositionsEnemies;
        public readonly SpriteRender[,] Tiles;

        private readonly int width;
        private readonly int height;

        private readonly int sizeTile = int.Parse(Resources.TileSize);
        private readonly Random randGenerator = new Random();
       
        public Map()
        {
            width = int.Parse(Resources.MapWidth);
            height = int.Parse(Resources.MapHeight);
            Tiles = new SpriteRender[width / sizeTile, height / sizeTile];

            PositionBoss = new Point(
                randGenerator.Next(width / 2 - 20, width / 2 + 20 + 1),
                randGenerator.Next(height / 2 - 20, height / 2 + 20 + 1));

            PositionPlayer = new Point(
                randGenerator.Next(sizeTile + 20, width / 2 - 20) + width / 2 * randGenerator.Next(0, 2),
                randGenerator.Next(sizeTile + 20, height / 2 - 20) + height / 2 * randGenerator.Next(0, 2));
            

            CreateTiles();

        }

        private void CreateTiles()
        {
            var rand = new Random();

            for (var x = 0; x < width / sizeTile; x += 1)
            {
                for (var y = 0; y < height / sizeTile; y += 1)
                {

                    if (rand.NextDouble() > .95)
                    {
                        var box = new SpriteRender(x * sizeTile, y * sizeTile, Resources.Box);
                        Tiles[x, y] = box;
                        Physics.AddToTrackingCollisions(new GameObject
                        {
                            X = box.X + box.Size.Width / 2,
                            Y = box.Y + box.Size.Height / 2,
                            Size = box.Size
                        });
                    }
                    else
                    {
                        Tiles[x, y] = new SpriteRender(x * sizeTile, y * sizeTile,
                            Resources.Grass.Extract(new Rectangle(sizeTile * rand.Next(0, 4), 0, sizeTile, sizeTile)));
                    }
                }
            }
        }

        public void DrawTiles(Graphics g)
        {
            foreach (var tile in Tiles)
            {
                tile.Draw(g);
            }
        }

    }
}
