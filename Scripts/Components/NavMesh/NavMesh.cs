using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Top_Down_shooter.Scripts.Controllers;
using Top_Down_shooter.Scripts.GameObjects;

namespace Top_Down_shooter.Scripts.Components
{
    class NavMesh
    {
        private readonly GameObject[,] navMesh;

        private readonly int width;
        private readonly int height;

        public NavMesh(Map map)
        {
            navMesh = map.Tiles;
            width = map.Tiles.GetLength(0);
            height = map.Tiles.GetLength(1);
        }

        public Queue<Point> GetPath(Point start, Point target)
        {
            var startTilePosition = GetPositionInNavMesh(start);
            var endTilePosition = GetPositionInNavMesh(target);

            if (startTilePosition is null)
                return new Queue<Point>();
            
            var closed = new HashSet<Point>();
            var opened = new HashSet<Point>() { startTilePosition.Value };
            var track = new Dictionary<Point, NodeData>()
            {
                [startTilePosition.Value] = new NodeData(null, 0, GetH(startTilePosition.Value, endTilePosition.Value))
            };

            while (opened.Count > 0)
            {
                var currPoint = opened
                    .OrderBy(p => track[p].F)
                    .Where(p => !closed.Contains(p))
                    .FirstOrDefault();

                if (currPoint == endTilePosition)
                    return BuildPath();

                closed.Add(currPoint);
                foreach (var neighbourPosition in GetUnclosedNeighbours(currPoint, closed))
                {
                    var g = track[currPoint].G + GetDistance(currPoint, neighbourPosition);
                    if (!opened.Contains(neighbourPosition) || g < track[neighbourPosition].G)
                    {
                        track[neighbourPosition] = new NodeData(track[currPoint], g, GetH(neighbourPosition, endTilePosition.Value));
                        opened.Add(neighbourPosition);
                    }
                }
            }
        }

        private Queue<Point> BuildPath()
        {
            throw new NotImplementedException();
        }

        private int GetDistance(Point position1, Point position2)
        {
            throw new NotImplementedException();
        }

        private int GetH(Point point, Point target)
        {
            throw new NotImplementedException();
        }

        private Point? GetPositionInNavMesh(Point point)
        {
            var left = 0;
            var right = width - 1;
            var top = 0;
            var bottom = height - 1;

            while (left < right && bottom < top)
            {
                var midX = (right + left) / 2;
                var midY = (bottom + top) / 2;

                if (point.X <= navMesh[midX, midY].X)
                    right = midX;
                else left = midX + 1;

                if (point.Y <= navMesh[midX, midY].Y)
                    bottom = midX;
                else top = midX + 1;
            }

            if (navMesh[right, top].Collider.Transform.Contains(point))
                return new Point(right, top);

            return null;
        }

        private IEnumerable<Point> GetUnclosedNeighbours(Point point, HashSet<Point> closed)
        {
            return Enumerable
                .Range(-1, 3)
                .SelectMany(dx => Enumerable
                                    .Range(-1, 3),
                            (dx, dy) => new Point(point.X + dx, point.Y + dy))
                .Where(neighbor =>
                    neighbor.X > -1 && neighbor.X < width && neighbor.Y > -1 && neighbor.Y < height
                    && neighbor.X != point.X && neighbor.Y != point.Y
                    && !closed.Contains(neighbor));
        }
    }
}
