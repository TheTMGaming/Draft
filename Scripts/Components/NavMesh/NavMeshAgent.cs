﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Top_Down_shooter.Scripts.GameObjects;

namespace Top_Down_shooter.Scripts.Components
{
    class NavMeshAgent
    {
        public Point Target { get; set; }
        public Stack<Point> Path = new Stack<Point>();

        private readonly Enemy enemy;

        private static readonly int maxCountOpenedPoint = 800;
        private static readonly int distanceSearching = 6;
        private static readonly int stepSearching = 3;

        private static Random randGenerator = new Random();

        public NavMeshAgent(Enemy enemy)
        {
            this.enemy = enemy;
            NavMesh.AddAgent(this);
        }

        public void ComputePath()
        {
            var startInMesh = new Point(enemy.X / NavMesh.StepAgent, enemy.Y / NavMesh.StepAgent);
            var targetInMesh = GetEmptyPoint(Target);
            if (targetInMesh is null) return;

            var closed = new HashSet<Point>();
            var opened = new HashSet<Point?>() { startInMesh };
            var track = new Dictionary<Point, Point?>()
            {
                [startInMesh] = null
            };
            NavMesh.Map[startInMesh.X, startInMesh.Y].SetPathParameters(0, GetDistance(startInMesh, targetInMesh.Value));

            while (opened.Count > 0 && opened.Count < maxCountOpenedPoint)
            {
                var currPoint = opened
                    .OrderBy(p => NavMesh.Map[p.Value.X, p.Value.Y].F)
                    .Where(p => !closed.Contains(p.Value))
                    .FirstOrDefault();

                if (currPoint is null) break;
                if (currPoint == targetInMesh)
                {
                    BuildPath(targetInMesh.Value, track);
                    return;
                }

                closed.Add(currPoint.Value);
                foreach (var neighbourPosition in GetUnclosedNeighbours(currPoint.Value, closed))
                {
                    var tempG = NavMesh.Map[currPoint.Value.X, currPoint.Value.Y].G + GetDistance(currPoint.Value, neighbourPosition);
                    if (!opened.Contains(neighbourPosition) 
                        || tempG < NavMesh.Map[neighbourPosition.X, neighbourPosition.Y].G)
                    {
                        track[neighbourPosition] = currPoint;
                        NavMesh.Map[neighbourPosition.X, neighbourPosition.Y].SetPathParameters(
                            tempG, GetH(neighbourPosition, targetInMesh.Value));
                        opened.Add(neighbourPosition);
                    }
                }
            }
        }

        private void BuildPath(Point target, Dictionary<Point, Point?> track)
        {
            var path = new Stack<Point>();

            Point? end = target;

            while (!(end is null))
            {
                if (!(track[end.Value] is null))
                    path.Push(NavMesh.Map[end.Value.X, end.Value.Y].Position);
                end = track[end.Value];
            }

            Path = path;
        }

        private int GetDistance(Point pos1, Point pos2) =>
            (int)(Math.Sqrt((pos1.X - pos2.X) * (pos1.X - pos2.X) + (pos1.Y - pos2.Y) * (pos1.Y - pos2.Y)) * NavMesh.CostOrthogonalPoint);

        private int GetH(Point point, Point target) =>
            (Math.Abs(target.X - point.X) + Math.Abs(target.Y - point.Y)) * NavMesh.CostOrthogonalPoint;
       
        private IEnumerable<Point> GetUnclosedNeighbours(Point point, HashSet<Point> closed)
        {
            return Enumerable
                .Range(-1, 3)
                .SelectMany(dx => Enumerable
                                    .Range(-1, 3),
                            (dx, dy) => new Point(point.X + dx, point.Y + dy))
                .Where(neighbor =>
                    neighbor.X > -1 && neighbor.X < NavMesh.Width && neighbor.Y > -1 && neighbor.Y < NavMesh.Height
                    && (!NavMesh.Map[neighbor.X, neighbor.Y].IsObstacle || NavMesh.Map[neighbor.X, neighbor.Y].Parent == enemy.Collider)
                    && !closed.Contains(neighbor));
        }

        private Point? GetEmptyPoint(Point point)
        {
            var pointInMesh = new Point(point.X / NavMesh.StepAgent, point.Y / NavMesh.StepAgent);
            if (!NavMesh.Map[pointInMesh.X, pointInMesh.Y].IsObstacle)
                return pointInMesh;

            var targets = Enumerable
                .Range(0, (pointInMesh.X - distanceSearching + 2 * distanceSearching - pointInMesh.X + distanceSearching) / stepSearching + 1)
                .Select(i => pointInMesh.X - distanceSearching + stepSearching * i)
                .SelectMany(dx => Enumerable
                                    .Range(0, (pointInMesh.Y - distanceSearching + 2 * distanceSearching 
                                        - pointInMesh.Y + distanceSearching) / stepSearching + 1)
                                    .Select(i => pointInMesh.Y - distanceSearching + stepSearching * i),
                            (x, y) => new Point?(new Point(x, y)))
                .Where(neighbor =>
                    neighbor.Value.X > -1 && neighbor.Value.X < NavMesh.Width && neighbor.Value.Y > -1 && neighbor.Value.Y < NavMesh.Height
                    && !NavMesh.Map[neighbor.Value.X, neighbor.Value.Y].IsObstacle)
                .ToList();

            if (targets.Count == 0)
                return null;

            return targets[randGenerator.Next(0, targets.Count)];
        }
    }
}
