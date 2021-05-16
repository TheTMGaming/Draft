using System;
using System.Collections.Generic;
using System.Drawing;
using Top_Down_shooter.Scripts.Components;
using Top_Down_shooter.Scripts.Source;

namespace Top_Down_shooter.Scripts.GameObjects
{
    class Player : Character
    {
        public Gun Gun { get; set; }

        private readonly Point OffsetPositionGun = new Point(20, 38);
        public Stack<Point> a;
        public Point b;
        public Player(int x, int y, int speed)
        {
            //Size = new Size(70, 110);
            Collider = new Collider(this, localX: 0, localY: 30, width: 60, height: 60);
            X = x;
            Y = y;
            Speed = speed;
            Agent = new NavMeshAgent(this);
            Gun = new Gun(X + OffsetPositionGun.X, Y + OffsetPositionGun.Y);
            
        }

        public override void Move(bool isReverse = false)
        {
            if (a.Count > 0 && Transform == b)
                b = a.Pop();

            var q = MoveTowards(Transform, b, Speed);
            X = (int)q.X;
            Y = (int)q.Y;

            base.Move(isReverse);
            Gun.Move(X + OffsetPositionGun.X, Y + OffsetPositionGun.Y);
        }

        public PointF MoveTowards(Point current, Point target, float maxDistanceDelta)
        {
            Point a = new Point(target.X - current.X, target.Y - current.Y);
            float magnitude = (float)Math.Sqrt(a.X * a.X + a.Y * a.Y);
            if (magnitude <= maxDistanceDelta || magnitude == 0f)
            {
                return target;
            }
            return new PointF(current.X + a.X / magnitude * maxDistanceDelta, current.Y + a.Y / magnitude * maxDistanceDelta);
        }
    }
}
