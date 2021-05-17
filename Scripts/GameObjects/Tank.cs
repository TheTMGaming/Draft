using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Top_Down_shooter.Scripts.Components;

namespace Top_Down_shooter.Scripts.GameObjects
{
    class Tank : Character
    {
        private Stack<Point> path = new Stack<Point>();
        private Point nextCheckPoint;

        public Tank(int x, int y)
        {
            X = x;
            Y = y;
            Speed = 6;
            Collider = new Collider(this, localX: 0, localY: 30, width: 60, height: 60);
            nextCheckPoint = GameModel.Player.Transform;
        }

        public override void Move(bool isReverse = false)
        {
            if (path.Count == 0)
            {
                path = NavMeshAgent.GetPath(Transform, GameModel.Player.Transform);;
            }
            if (path.Count > 0 && Transform == nextCheckPoint)
                nextCheckPoint = path.Pop();

            var q = MoveTowards(Transform, nextCheckPoint, Speed);
            X = (int)q.X;
            Y = (int)q.Y;
            base.Move(isReverse);
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
