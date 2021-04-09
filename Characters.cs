using System.Drawing;

namespace Top_Down_shooter
{
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
    
    abstract class Character
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Speed { get; set; }
        public int Health { get; set; }
        public DirectionX DirectionX { get; set; }
        public DirectionY DirectionY { get; set; }
        public Sight Sight { get; set; }
        public Bitmap Image { get; set; }
        public Size Scale { get; set; }

        public virtual void Move()
        {
            X += Speed * (int)DirectionX;
            Y += Speed * (int)DirectionY;
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

        private int trunkX, trunkY;

        private const int OffsetTrunkX = 23;
        private const int OffsetTrunkY = 33;

        public Player(int x, int y, int speed, Bitmap image)
        {
            trunkX = x + OffsetTrunkX;
            trunkY = y + OffsetTrunkY;
            Gun = new Gun(trunkX, trunkY);
            Image = image;
            Speed = speed;
            X = x;
            Y = y;
        }

        public override void Move()
        {
            base.Move();
            Gun.Move(
                trunkX += Speed * (int)DirectionX,
                trunkY += Speed * (int)DirectionY
                );
        }
    }
}
