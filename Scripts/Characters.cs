using System;
using System.Drawing;

namespace Top_Down_shooter
{
    #region
    enum DirectionX
    {
        Left = -1, Idle = 0, Right = 1 
    }

    enum DirectionY
    {
        Up = -1, Idle = 0, Down = 1
    }

    enum Sight
    {
        Left, Right
    }
    #endregion

    abstract class Character
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Speed { get; set; }
        public int Health { get; set; }
        public DirectionX DirectionX { get; set; }
        public DirectionY DirectionY { get; set; }
        public Sight Sight { get; set; }

        public virtual void Move()
        {
            X += (int)Math.Round(Speed * (int)DirectionX * (DirectionY != DirectionY.Idle ? Math.Sqrt(2) / 2 : 1));
            Y += (int)Math.Round(Speed * (int)DirectionY * (DirectionX != DirectionX.Idle ? Math.Sqrt(2) / 2 : 1));
        }

        public virtual void ChangeDirection(DirectionX directionX)
        {
            DirectionX = directionX;
            if (DirectionX != DirectionX.Idle)
                Sight = directionX == DirectionX.Left ? Sight.Left : Sight.Right;
        }

        public virtual void ChangeDirection(DirectionY directionY) => DirectionY = directionY;   
    }

    class Player : Character
    {
        public Gun Gun { get; set; }

        private readonly Point OffsetPositionGun = new Point(20, 38);

        public Player(int x, int y, int speed)
        {
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
    }
}
