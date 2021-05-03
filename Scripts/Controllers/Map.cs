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
        Grass, Box, None
    }

    class Map
    {
        public readonly TileTypes[,] Cells;
        public readonly SpriteRender[,] Tiles;

        private readonly int width;
        private readonly int height;

        private readonly int sizeTile = int.Parse(Resources.TileSize);
        private readonly Random randGenerator = new Random();
       
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
            

            

            for (var i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Cells[i, j] = TileTypes.None;
                }
            }


            var level = 0;
            var visited = new HashSet<Point>();
            var queue = new Queue<A>();
            queue.Enqueue(new A(new Point(width / 2, height / 2), 0));
            visited.Add(new Point(width / 2, height / 2));


            while (queue.Count > 0)
            {
                var tile = queue.Dequeue();

                var n = GetNeighbors(tile.point, visited);
                if (n.Count(a => Cells[a.X, a.Y] == TileTypes.Box) == 0 && tile.level > 2 && randGenerator.NextDouble() > .75 + tile.level * 0.007)
                    Cells[tile.point.X, tile.point.Y] = TileTypes.Box;

                foreach (var neighbour in n)
                {
                    if (!visited.Contains(neighbour))
                    {
                        queue.Enqueue(new A(neighbour, tile.level + 1));
                        visited.Add(neighbour);
                    }
                }

                visited.Add(tile.point);
                level++;
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


        private IEnumerable<Point> GetNeighbors(Point point, HashSet<Point> visited)
        {
            return Enumerable
                .Range(-2, 4)
                .SelectMany(dx => Enumerable.Range(-2, 4).Where(dy => dx != point.X && dy != point.Y),
                            (dx, dy) => new Point(point.X + dx, point.Y + dy))
                .Where(n => n.X > -1 && n.X < width && n.Y > -1 && n.Y < height
                ) ;
        }

    }
}
