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

    class Map
    {
        public readonly GameObject[,] Tiles;

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
            Tiles = new GameObject[width, height];

            CreateMap();
        }

        private void CreateMap()
        {
            var rand = new Random();

            var visited = new HashSet<Point>();
            var queue = new Queue<A>();

            queue.Enqueue(new A(new Point(width / 2, height / 2), 0));

            while (queue.Count > 0)
            {
                var tile = queue.Dequeue();
                visited.Add(tile.point);

                var zone = GetTileZone(tile.point).ToList();

                if (zone.Count(a => Tiles[a.X, a.Y] is Box) < maxCountBoxStack
                    && tile.level > sizeBossZone
                    && randGenerator.NextDouble() > initialProbabilitySpawnBox + (tile.level - sizeBossZone) * increasingProbabilityLevels)
                {
                    var box = new Box(tile.point.X * sizeTile, tile.point.Y * sizeTile);

                    Tiles[tile.point.X, tile.point.Y] = box;
                    Physics.AddToTrackingCollisions(box);
                }
                else
                {
                   
                    Tiles[tile.point.X, tile.point.Y] = new Grass(tile.point.X * sizeTile, tile.point.Y * sizeTile);
                }

                foreach (var neighbour in GetNeighbors(tile.point, zone, visited))
                {                  
                    queue.Enqueue(new A(neighbour, tile.level + 1));
                    visited.Add(neighbour);                   
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
