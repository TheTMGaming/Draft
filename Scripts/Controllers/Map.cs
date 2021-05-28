using System;
using System.Drawing;
using Top_Down_shooter.Scripts.GameObjects;
using Top_Down_shooter.Scripts.Source;
using Top_Down_shooter.Properties;
using System.Collections.Generic;
using System.Linq;

namespace Top_Down_shooter.Scripts.Controllers
{
    class TileData
    {
        public Point Point;
        public int Level;

        public TileData(Point point, int level)
        {
            Point = point;
            Level = level;
        }
    }

    class Map
    {
        public readonly GameObject[,] Tiles;

        private readonly int width;
        private readonly int height;

        private readonly Random randGenerator = new Random();

        private readonly int maxCountBoxStack = 2;
        private readonly int sizeBossZone = 3;
        private readonly int sizeViewedZoneTile = 3;
        private readonly float initialProbabilitySpawnBox = .8f;
        private readonly float increasingProbabilityLevels = .012f;

        public Map()
        {
            width = GameSettings.MapWidth / GameSettings.TileSize;
            height = GameSettings.MapHeight / GameSettings.TileSize;
            Tiles = new GameObject[width, height];

            CreateMap();
        }

        private void CreateMap()
        {
            foreach (var x in Enumerable.Range(0, width))
            {
                Tiles[x, 0] = new Block(x * GameSettings.TileSize + GameSettings.TileSize / 2, GameSettings.TileSize / 2);
                Tiles[x, height - 1] = new Block(x * GameSettings.TileSize + GameSettings.TileSize / 2, (height - 1) * GameSettings.TileSize + GameSettings.TileSize / 2);

                Physics.AddToTrackingCollisions(Tiles[x, 0].Collider);
                Physics.AddToTrackingCollisions(Tiles[x, height - 1].Collider);
            }
            foreach (var y in Enumerable.Range(1, height - 1))
            {
                Tiles[0, y] = new Block(GameSettings.TileSize / 2, y * GameSettings.TileSize + GameSettings.TileSize / 2);
                Tiles[width - 1, y] = new Block((width - 1) * GameSettings.TileSize + GameSettings.TileSize / 2, y * GameSettings.TileSize + GameSettings.TileSize / 2);

                Physics.AddToTrackingCollisions(Tiles[0, y].Collider);
                Physics.AddToTrackingCollisions(Tiles[width - 1, y].Collider);
            }
            var rand = new Random();

            var visited = new HashSet<Point>();
            var queue = new Queue<TileData>();

            queue.Enqueue(new TileData(new Point(width / 2, height / 2), 0));

            while (queue.Count > 0)
            {
                var tile = queue.Dequeue();
                visited.Add(tile.Point);

                var zone = GetTileZone(tile.Point);

                if (zone.Count(a => Tiles[a.X, a.Y] is Box) < maxCountBoxStack
                    && tile.Level > sizeBossZone
                    && randGenerator.NextDouble() > initialProbabilitySpawnBox + (tile.Level - sizeBossZone) * increasingProbabilityLevels
                    && !new Rectangle(tile.Point.X * GameSettings.TileSize, tile.Point.Y * GameSettings.TileSize, GameSettings.TileSize, GameSettings.TileSize).IntersectsWith(GameModel.Player.Collider.Transform))
                {
                    var box = new Box(tile.Point.X * GameSettings.TileSize + GameSettings.TileSize / 2, tile.Point.Y * GameSettings.TileSize + GameSettings.TileSize / 2);

                    Tiles[tile.Point.X, tile.Point.Y] = box;
                    Physics.AddToTrackingCollisions(box.Collider);
                }
                else
                {
                   
                    Tiles[tile.Point.X, tile.Point.Y] = new Grass(tile.Point.X * GameSettings.TileSize + GameSettings.TileSize / 2, tile.Point.Y * GameSettings.TileSize + GameSettings.TileSize / 2);
                }

                foreach (var neighbour in GetNeighbors(tile.Point, zone, visited))
                {                  
                    queue.Enqueue(new TileData(neighbour, tile.Level + 1));
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
                .Where(p => p.X > 0 && p.X < width - 1 && p.Y > 0 && p.Y < height - 1 && p != point);
        }


        private IEnumerable<Point> GetNeighbors(Point point, IEnumerable<Point> tileZone, HashSet<Point> visited)
        {
            return tileZone
                .Where(p => Math.Abs(p.X - point.X) < 2 && Math.Abs(p.Y - point.Y) < 2
                    && !visited.Contains(p));
        }

    }
}
