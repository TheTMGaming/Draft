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

        private int trunkX, trunkY;

        private const int OffsetTrunkX = 72;
        private const int OffsetTrunkY = 99;

        public Player(int xLeft, int yTop, int speed, Bitmap atlas, int stateCountAnimation, int frameCountAnimation)
        {
            X = xLeft;
            Y = yTop;
            Speed = speed;

            trunkX = xLeft + OffsetTrunkX;
            trunkY = yTop + OffsetTrunkY;

            Image = atlas;
            StateCount = stateCountAnimation;
            FrameCount = frameCountAnimation;

            Gun = new Gun(trunkX, trunkY, new Bitmap(@"Sprites/Gun.png"));
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
