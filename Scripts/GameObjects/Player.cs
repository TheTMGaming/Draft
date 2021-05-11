using System.Drawing;
using Top_Down_shooter.Scripts.Components;

namespace Top_Down_shooter.Scripts.GameObjects
{
    class Player : Character
    {
        public Gun Gun { get; set; }

        private readonly Point OffsetPositionGun = new Point(20, 38);

        public Player(int x, int y, int speed)
        {
            //Size = new Size(70, 110);
            Collider = new Collider(this, localX: 0, localY: 30, width: 60, height: 60);
            X = x;
            Y = y;
            Speed = speed;
            Gun = new Gun(X + OffsetPositionGun.X, Y + OffsetPositionGun.Y);
        }

        public override void Move(bool isReverse = false)
        {
            base.Move(isReverse);
            Gun.Move(X + OffsetPositionGun.X, Y + OffsetPositionGun.Y);
        }
    }
}
