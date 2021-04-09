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

    class Character
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float TrunkX { get; set; }
        public float TrunkY { get; set; }
        public float Speed { get; set; }
        public float Health { get; set; }
        public DirectionX DirectionX { get; set; }
        public DirectionY DirectionY { get; set; }
        public Sight Sight { get; set; }
        public Bitmap Image { get; private set; }
        public Size Scale { get; set; }
        public Gun Gun { get; set; }

        public static float OffsetTrunkX = 23f;
        public static float OffsetTrunkY = 33f;

        public Character(float x, float y, float speed)
        {
            TrunkX = x + OffsetTrunkX;
            TrunkY = y + OffsetTrunkY;
            Gun = new Gun(TrunkX, TrunkY);
            Image = new Bitmap(@"Sprites/player.png");
            Speed = speed;
            X = x;
            Y = y;
        }

        public virtual void Move()
        {
            X += Speed * (int)DirectionX;
            Y += Speed * (int)DirectionY;
            Gun.Move(
                TrunkX += Speed * (int)DirectionX,
                TrunkY += Speed * (int)DirectionY
                );
        }

        public virtual void ChangeDirection(DirectionX directionX)
        {
            DirectionX = directionX;
            if (DirectionX != DirectionX.Idle)
                Sight = directionX == DirectionX.Left ? Sight.Left : Sight.Right;
        }

        public virtual void ChangeDirection(DirectionY directionY) => DirectionY = directionY;
    }

}
