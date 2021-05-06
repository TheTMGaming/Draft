using System;
using System.Drawing;
using Top_Down_shooter.Scripts.GameObjects;
using Top_Down_shooter.Scripts.Renders;
using Top_Down_shooter.Properties;
using System.Collections.Generic;
using System.Linq;

namespace Top_Down_shooter.Scripts.Controllers
{
    class A
    {
        public Point point;
        public int level;

        public A(Point p, int l)
        {
            point = p;
            level = l;
        }
    }

    enum TileTypes
    {
        Grass, Box
    }

    class Map
    {
        public readonly TileTypes[,] Cells;
        public readonly SpriteRender[,] Tiles;

        private readonly int width;
        private readonly int height;

        private readonly int sizeTile = int.Parse(Resources.TileSize);
        private readonly Random randGenerator = new Random();

        private readonly int maxCountBoxStack = 2;
        private readonly int sizeBossZone = 3;
        private readonly int sizeViewedZoneTile = 3;
        private readonly float initialProbabilitySpawnBox = .8f;
        private readonly float increasingProbabilityLevels = .012f;

        public Map()
        {
            width = int.Parse(Resources.MapWidth) / sizeTile;
            height = int.Parse(Resources.MapHeight) / sizeTile;
            Cells = new TileTypes[width, height];
            Tiles = new SpriteRender[width, height];


            CreateMap();
            CreateTiles();
        }
        public void Draw(Graphics g)
        {
            foreach (var s in Tiles)
                s.Draw(g);
        }
        private void CreateMap()
        {
            var visited = new HashSet<Point>();
            var queue = new Queue<A>();

            queue.Enqueue(new A(new Point(width / 2, height / 2), 0));
            visited.Add(new Point(width / 2, height / 2));


            while (queue.Count > 0)
            {
                var tile = queue.Dequeue();

                var zone = GetTileZone(tile.point).ToList();

                if (zone.Count(a => Cells[a.X, a.Y] == TileTypes.Box) < maxCountBoxStack
                    && tile.level > sizeBossZone
                    && randGenerator.NextDouble() > initialProbabilitySpawnBox + (tile.level - sizeBossZone) * increasingProbabilityLevels)
                {
                    Cells[tile.point.X, tile.point.Y] = TileTypes.Box;
                }

                foreach (var neighbour in GetNeighbors(tile.point, zone, visited))
                {                  
                    queue.Enqueue(new A(neighbour, tile.level + 1));
                    visited.Add(neighbour);                   
                }

                visited.Add(tile.point);
            }
        }

        private void CreateTiles()
        {
            var rand = new Random();


            for (var x = 0; x < width; x += 1)
            {
                for (var y = 0; y < height; y += 1)
                {

                    if (Cells[x, y] == TileTypes.Box)
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

        private IEnumerable<Point> GetTileZone(Point point)
        {
            return Enumerable
                .Range(-sizeViewedZoneTile, 1 + sizeViewedZoneTile * 2)
                .SelectMany(dx => Enumerable.Range(-sizeViewedZoneTile, 1 + sizeViewedZoneTile * 2),
                            (dx, dy) => new Point(point.X + dx, point.Y + dy))
                .Where(p => p.X > -1 && p.X < width && p.Y > -1 && p.Y < height && p != point);
        }


        private IEnumerable<Point> GetNeighbors(Point point, IEnumerable<Point> tileZone, HashSet<Point> visited)
        {
            return tileZone
                .Where(p => Math.Abs(p.X - point.X) < 2 && Math.Abs(p.Y - point.Y) < 2
                    && !visited.Contains(p));
        }

    }
}
