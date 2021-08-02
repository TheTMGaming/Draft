using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Top_Down_shooter.Scripts.Source
{
    static class PointExtensions
    {
        public static Point Rotate(this Point point, float angleInRadian)
        {
            return new Point(
                (int)(point.X * Math.Cos(angleInRadian) - point.Y * Math.Sin(angleInRadian)),
                (int)(point.Y * Math.Cos(angleInRadian) + point.X * Math.Sin(angleInRadian))
                );
        }

        public static Point MoveTowards(this Point point, Point target, float maxDistanceDelta)
        {
            var direction = new Point(target.X - point.X, target.Y - point.Y);
            var magnitude = (float)Math.Sqrt(direction.X * direction.X + direction.Y * direction.Y);

            if (magnitude <= maxDistanceDelta || magnitude == 0f)
                return target;

            return new Point(
                (int)(point.X + direction.X / magnitude * maxDistanceDelta),
                (int)(point.Y + direction.Y / magnitude * maxDistanceDelta));
        }
    }
}
