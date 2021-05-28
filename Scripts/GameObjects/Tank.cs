using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Top_Down_shooter.Scripts.GameObjects;
using Top_Down_shooter.Scripts.Components;
using Top_Down_shooter.Scripts.Source;

namespace Top_Down_shooter.Scripts.GameObjects
{
    class Tank : Enemy
    {
        public Stack<Point> path = new Stack<Point>();
        private Point nextCheckPoint;

        public Tank(int x, int y)
        {
            X = x;
            Y = y;

            Speed = GameSettings.TankSpeed;
            Health = GameSettings.TankHealth;

            Collider = new Collider(this, localX: 0, localY: 30, width: 60, height: 60);
            HitBox = new Collider(this, localX: 0, localY: 0, width: 60, height: 90);

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
            LookAt(GameModel.Player.Transform);

            X = q.X;
            Y = q.Y;
        }


        
    }
}
