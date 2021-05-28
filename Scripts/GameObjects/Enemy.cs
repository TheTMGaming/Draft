using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Top_Down_shooter.Scripts.GameObjects
{
    class Enemy : Character
    {
        public int Damage { get; private set; }

        public void LookAt(Point target)
        {
            var direction = new Point(target.X - X, target.Y - Y);

            if (direction.X > 0) ChangeDirection(DirectionX.Right);
            else if (direction.X < 0) ChangeDirection(DirectionX.Left);
            else ChangeDirection(DirectionX.Idle);

            if (direction.Y > 0) ChangeDirection(DirectionY.Down);
            else if (direction.Y < 0) ChangeDirection(DirectionY.Up);
            else ChangeDirection(DirectionY.Idle);
        }

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
