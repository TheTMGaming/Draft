using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Top_Down_shooter.Scripts.Controllers;
using Top_Down_shooter.Scripts.GameObjects;
using Top_Down_shooter.Scripts.Source;

namespace Top_Down_shooter.Scripts.Components
{
    class NavMeshAgent
    {
        private readonly Node[,] navMesh;

        private readonly int width;
        private readonly int height;
        private readonly int distanceFromObstacle;
        private readonly int stepAgent;

        private readonly int costOrthogonalPoint = 10;

        public NavMeshAgent(Character agent)
        {
            stepAgent = agent.Speed;

            width = GameSettings.MapWidth / stepAgent;
            height = GameSettings.MapHeight / stepAgent;

            distanceFromObstacle = Math.Max(agent.Collider.Width, agent.Collider.Height) / 2;

            navMesh = new Node[width, height];
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    navMesh[x, y] = new Node(new Point(x * stepAgent, y * stepAgent));
                }
            }
        }

        public void Bake(Map map)
        {
            foreach (var tile in map.Tiles)
            {
                if (tile is Grass)
                    continue;

                var rect = tile.Collider.Transform;

                var offsetX = distanceFromObstacle - distanceFromObstacle % stepAgent;
                var offsetY = distanceFromObstacle - distanceFromObstacle & stepAgent;

                var xLeft = rect.X - offsetX;
                if (xLeft < 0) xLeft = 0;

                var yTop = rect.Y - offsetY;
                if (yTop < 0) yTop = 0;

                var xRight = rect.X + rect.Width + offsetX;
                if (xRight >= GameSettings.MapWidth)
                    xRight = GameSettings.MapWidth - 1;

                var yBottom = rect.Y + rect.Height + offsetY;
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

        public Stack<Point> GetPath(Point start, Point target)
        {
            var closed = new HashSet<Point>();
            var opened = new HashSet<Point?>() { new Point(start.X / stepAgent, start.Y / stepAgent) };
            var track = new Dictionary<Point, Point?>()
            {
                [new Point(start.X / stepAgent, start.Y / stepAgent)] = null
            };

            while (opened.Count > 0)
            {
                var currPoint = opened
                    .OrderBy(p => navMesh[p.Value.X, p.Value.Y].F)
                    .Where(p => !closed.Contains(p.Value))
                    .FirstOrDefault();

                if (currPoint is null) return new Stack<Point>();

                if (currPoint == target)
                    return BuildPath(target, track);

                closed.Add(currPoint.Value);
                foreach (var neighbourPosition in GetUnclosedNeighbours(currPoint.Value, closed))
                {
                    var g = navMesh[currPoint.Value.X, currPoint.Value.Y].G + GetDistance(currPoint.Value, neighbourPosition);
                    if (!opened.Contains(neighbourPosition) || g < navMesh[neighbourPosition.X, neighbourPosition.Y].G)
                    {
                        track[neighbourPosition] = currPoint;
                        navMesh[neighbourPosition.X, neighbourPosition.Y].SetPathParameters(g, GetH(neighbourPosition, target));
                        opened.Add(neighbourPosition);
                    }
                }
            }

            return new Stack<Point>();
        }

        private Stack<Point> BuildPath(Point target, Dictionary<Point, Point?> track)
        {
            var path = new Stack<Point>();
            Point? end = target;

            while (!(end is null))
            {
                path.Push(navMesh[end.Value.X, end.Value.Y].Position);
                end = track[end.Value];
            }

            return path;
        }

        private int GetDistance(Point pos1, Point pos2) =>
            (int)(Math.Sqrt((pos1.X - pos2.X) * (pos1.X - pos2.X) + (pos1.Y - pos2.Y) * (pos1.Y - pos2.Y)) * costOrthogonalPoint);

        private int GetH(Point point, Point target) =>
            //(Math.Abs(target.X - point.X) + Math.Abs(target.Y - point.Y)) * costOrthogonalPoint;
            (int)(Math.Sqrt((target.X - point.X) * (target.X - point.X) + (target.Y - point.Y)*(target.Y - point.Y)));

        
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
