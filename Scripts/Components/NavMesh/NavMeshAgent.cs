using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Top_Down_shooter.Scripts.Controllers;
using Top_Down_shooter.Scripts.GameObjects;
using Top_Down_shooter.Scripts.Source;

namespace Top_Down_shooter.Scripts.Components
{
    static class NavMeshAgent
    {
        public static readonly Node[,] navMesh;

        private static readonly int width;
        private static readonly int height;
        private static readonly int distanceFromObstacle;
        private static readonly int stepAgent = 32;

        private static readonly int costOrthogonalPoint = 10;

        private static readonly Random randGenerator = new Random();

        static NavMeshAgent()
        {
            width = GameSettings.MapWidth / stepAgent;
            height = GameSettings.MapHeight / stepAgent;

            distanceFromObstacle = Math.Max(
                GameSettings.FiremanSizeCollider, Math.Max(GameSettings.TankSizeCollider, GameSettings.WatermanSizeCollider)) / 2;

            navMesh = new Node[width, height];
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    navMesh[x, y] = new Node(new Point(x * stepAgent, y * stepAgent));
                }
            }
        }

        public static void Bake(Map map)
        {
            foreach (var tile in map.Tiles)
            {
                if (tile is Grass)
                    continue;

                var rect = tile.Collider.Transform;

                var offset = distanceFromObstacle - distanceFromObstacle % stepAgent;

                var xLeft = rect.X - offset;
                if (xLeft < 0) xLeft = 0;

                var yTop = rect.Y - offset;
                if (yTop < 0) yTop = 0;

                var xRight = rect.X + rect.Width + offset;
                if (xRight >= GameSettings.MapWidth)
                    xRight = GameSettings.MapWidth - 1;

                var yBottom = rect.Y + rect.Height + offset;
                if (yBottom >= GameSettings.MapHeight)
                    yBottom = GameSettings.MapHeight - 1;

                for (var x = xLeft; x <= xRight; x += stepAgent)
                {
                    for (var y = yTop; y <= yBottom; y += stepAgent)
                    {
                        navMesh[x / stepAgent, y / stepAgent].IsObstacle = true;
                    }
                }
            }
        }

        public static Stack<Point> GetPath(Point start, Point target)
        {
            var startInMesh = new Point(start.X / stepAgent, start.Y / stepAgent);
            var targetInMesh = GetEmptyPoint(target);
            if (targetInMesh is null) return new Stack<Point>();

            var closed = new HashSet<Point>();
            var opened = new HashSet<Point?>() { startInMesh };
            var track = new Dictionary<Point, Point?>()
            {
                [startInMesh] = null
            };
            navMesh[startInMesh.X, startInMesh.Y].SetPathParameters(0, GetDistance(startInMesh, targetInMesh.Value));

            while (opened.Count > 0)
            {
                var currPoint = opened
                    .OrderBy(p => navMesh[p.Value.X, p.Value.Y].F)
                    .Where(p => !closed.Contains(p.Value))
                    .FirstOrDefault();

                if (currPoint is null) break;
                if (currPoint == targetInMesh)
                    return BuildPath(targetInMesh.Value, track);

                closed.Add(currPoint.Value);
                foreach (var neighbourPosition in GetUnclosedNeighbours(currPoint.Value, closed))
                {
                    var tempG = navMesh[currPoint.Value.X, currPoint.Value.Y].G + GetDistance(currPoint.Value, neighbourPosition);
                    if (!opened.Contains(neighbourPosition) || tempG < navMesh[neighbourPosition.X, neighbourPosition.Y].G)
                    {
                        track[neighbourPosition] = currPoint;
                        navMesh[neighbourPosition.X, neighbourPosition.Y].SetPathParameters(tempG, GetH(neighbourPosition, targetInMesh.Value));
                        opened.Add(neighbourPosition);
                    }
                }
            }

            return new Stack<Point>();
        }

        private static Stack<Point> BuildPath(Point target, Dictionary<Point, Point?> track)
        {
            var path = new Stack<Point>();
            Point? end = target;

            while (!(end is null))
            {
                if (!(track[end.Value] is null))
                    path.Push(navMesh[end.Value.X, end.Value.Y].Position);
                end = track[end.Value];
            }

            return path;
        }

        private static int GetDistance(Point pos1, Point pos2) =>
            (int)(Math.Sqrt((pos1.X - pos2.X) * (pos1.X - pos2.X) + (pos1.Y - pos2.Y) * (pos1.Y - pos2.Y)) * costOrthogonalPoint);

        private static int GetH(Point point, Point target) =>
            (Math.Abs(target.X - point.X) + Math.Abs(target.Y - point.Y)) * costOrthogonalPoint;
       
        private static IEnumerable<Point> GetUnclosedNeighbours(Point point, HashSet<Point> closed)
        {
            return Enumerable
                .Range(-1, 3)
                .SelectMany(dx => Enumerable
                                    .Range(-1, 3),
                            (dx, dy) => new Point(point.X + dx, point.Y + dy))
                .Where(neighbor =>
                    neighbor.X > -1 && neighbor.X < width && neighbor.Y > -1 && neighbor.Y < height
                    && !navMesh[neighbor.X, neighbor.Y].IsObstacle
                    && !closed.Contains(neighbor));
        }

        private static Point? GetEmptyPoint(Point point)
        {
            var pointInMesh = new Point(point.X / stepAgent, point.Y / stepAgent);
            if (!navMesh[pointInMesh.X, pointInMesh.Y].IsObstacle)
                return pointInMesh;
      
            return Enumerable
                .Range(-1, 3)
                .SelectMany(dx => Enumerable
                                    .Range(-1, 3),
                            (dx, dy) => new Point?(new Point(pointInMesh.X + dx, pointInMesh.Y + dy)))
                .Where(neighbor =>
                    neighbor.Value.X > -1 && neighbor.Value.X < width && neighbor.Value.Y > -1 && neighbor.Value.Y < height
                    && !navMesh[neighbor.Value.X, neighbor.Value.Y].IsObstacle)
                .FirstOrDefault();
        }
    }
}
