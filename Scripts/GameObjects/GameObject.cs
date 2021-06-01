using System;
using System.Drawing;
using Top_Down_shooter.Scripts.Components;

namespace Top_Down_shooter.Scripts.GameObjects
{
    class GameObject
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Point Transform => new Point(X, Y);
        public Collider Collider { get; set; }

        public static Point MoveTowards(Point current, Point target, float maxDistanceDelta)
        {
            var direction = new Point(target.X - current.X, target.Y - current.Y);
            var magnitude = (float)Math.Sqrt(direction.X * direction.X + direction.Y * direction.Y);

            if (magnitude <= maxDistanceDelta || magnitude == 0f)
                return target;

            return new Point(
                (int)(current.X + direction.X / magnitude * maxDistanceDelta),
                (int)(current.Y + direction.Y / magnitude * maxDistanceDelta));
        }
    }
}
