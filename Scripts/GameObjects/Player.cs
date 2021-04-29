using System.Drawing;

namespace Top_Down_shooter.Scripts.GameObjects
{
    class Player : Character
    {
        public Gun Gun { get; set; }

        private readonly Point OffsetPositionGun = new Point(20, 38);

        public Player(int x, int y, int speed)
        {
            Size = new Size(70, 110);
            X = x;
            Y = y;
            Speed = speed;
            Gun = new Gun(X + OffsetPositionGun.X, Y + OffsetPositionGun.Y);
        }

        public override void Move()
        {
            base.Move();
            Gun.Move(X + OffsetPositionGun.X, Y + OffsetPositionGun.Y);
        }

        public override void ComeBack()
        {
            base.ComeBack();
            Gun.Move(X + OffsetPositionGun.X, Y + OffsetPositionGun.Y);
        }
    }
}
