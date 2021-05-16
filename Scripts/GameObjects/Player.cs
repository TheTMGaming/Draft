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
            if (a.Count > 0)
            {
                var b = a.Pop();
                X = b.X;
                Y = b.Y;
            }
            else
                base.Move(isReverse);
            Gun.Move(X + OffsetPositionGun.X, Y + OffsetPositionGun.Y);
        }
    }
}
