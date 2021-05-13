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

        private readonly int costOrthogonalPoint = 10;

        public NavMesh(Map map)
        {
            navMesh = map.Tiles;
            width = map.Tiles.GetLength(0);
            height = map.Tiles.GetLength(1);
        }

        public Stack<Point> GetPath(Point start, Point target)
        {
            var startTilePosition = GetPositionInNavMesh(start);
            var endTilePosition = GetPositionInNavMesh(target);

            if (startTilePosition is null)
                return new Stack<Point>();
            
            var closed = new HashSet<Point>();
            var opened = new HashSet<Point>() { startTilePosition.Value };
            var track = new Dictionary<Point, PointData>()
            {
                [startTilePosition.Value] = new PointData(null, 0, GetH(startTilePosition.Value, endTilePosition.Value))
            };

            while (opened.Count > 0)
            {
                var currPoint = opened
                    .OrderBy(p => track[p].F)
                    .Where(p => !closed.Contains(p))
                    .FirstOrDefault();

                if (currPoint == endTilePosition)
                    return BuildPath(endTilePosition.Value, track);

                closed.Add(currPoint);
                foreach (var neighbourPosition in GetUnclosedNeighbours(currPoint, closed))
                {
                    var g = track[currPoint].G + GetDistance(currPoint, neighbourPosition);
                    if (!opened.Contains(neighbourPosition) || g < track[neighbourPosition].G)
                    {
                        track[neighbourPosition] = new PointData(currPoint, g, GetH(neighbourPosition, endTilePosition.Value));
                        opened.Add(neighbourPosition);
                    }
                }
            }

            return new Stack<Point>();
        }

        private Stack<Point> BuildPath(Point target, Dictionary<Point, PointData> track)
        {
            var path = new Stack<Point>();
            Point? end = target;

            while (!(end is null))
            {
                path.Push(navMesh[end.Value.X, end.Value.Y].Transform);
                end = track[end.Value].Previous;
            }

            return path;
        }

        private int GetDistance(Point pos1, Point pos2) =>
            (int)Math.Sqrt((pos1.X - pos2.X) * (pos1.X - pos2.X) + (pos1.Y - pos2.Y) * (pos1.Y - pos2.Y)) * costOrthogonalPoint;

        private int GetH(Point point, Point target) =>
            (Math.Abs(target.X - point.X) + Math.Abs(target.Y - point.Y)) * costOrthogonalPoint;

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
