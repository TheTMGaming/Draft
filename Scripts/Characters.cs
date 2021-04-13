using System;
using System.Drawing;

namespace Top_Down_shooter
{
    #region
    enum AnimationTypes
    {
        IdleRight, IdleLeft, RunRight, RunLeft
    }

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

    abstract class Character : AnimationSprite
    {
        public int Speed { get; set; }
        public int Health { get; set; }
        public DirectionX DirectionX { get; set; }
        public DirectionY DirectionY { get; set; }
        public Sight Sight { get; set; }

        public override void Move()
        {
            X += (int)Math.Round(Speed * (int)DirectionX * (DirectionY != DirectionY.Idle ? Math.Sqrt(2) / 2 : 1));
            Y += (int)Math.Round(Speed * (int)DirectionY * (DirectionX != DirectionX.Idle ? Math.Sqrt(2) / 2 : 1));
        }

        public virtual void ChangeDirection(DirectionX directionX)
        {
            DirectionX = directionX;
            if (DirectionX != DirectionX.Idle)
                Sight = directionX == DirectionX.Left ? Sight.Left : Sight.Right;
            ChangeState((int)GetAnimationType(this));
        }

        public virtual void ChangeDirection(DirectionY directionY)
        {
            DirectionY = directionY;
            ChangeState((int)GetAnimationType(this));
        }

        public static AnimationTypes GetAnimationType(Character character)
        {
            if (character.DirectionX == DirectionX.Idle && character.DirectionY == DirectionY.Idle)
                return character.Sight == Sight.Left ? AnimationTypes.IdleLeft : AnimationTypes.IdleRight;

            return character.Sight == Sight.Left ? AnimationTypes.RunLeft : AnimationTypes.RunRight;
        }
    }

    class Player : Character
    {
        public Gun Gun { get; set; }

        private readonly Point OffsetPositionGun = new Point(20, 38);

        public Player(int x, int y, int speed, Bitmap atlas, int stateCountAnimation, int frameCountAnimation)
        {
            X = x;
            Y = y;
            Speed = speed;

            Image = atlas;
            StateCount = stateCountAnimation;
            FrameCount = frameCountAnimation;

            Gun = new Gun(X + OffsetPositionGun.X, Y + OffsetPositionGun.Y, new Bitmap(@"Sprites/Gun.png"));
        }

        public override void Move()
        {
            base.Move();
            Gun.Move(X + OffsetPositionGun.X, Y + OffsetPositionGun.Y);
        }
    }
}
