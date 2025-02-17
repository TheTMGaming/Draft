﻿using System;
using Top_Down_shooter.Scripts.Components;
using Top_Down_shooter.Scripts.UI;

namespace Top_Down_shooter.Scripts.GameObjects
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

    abstract class Character : GameObject
    {
        public int Speed { get; set; }
        public virtual int Health { get; set; }
        public Collider HitBox { get; set; }
        public DirectionX DirectionX { get; set; }
        public DirectionY DirectionY { get; set; }
        public Sight Sight { get; set; }
        public HealthBar HealthBar { get; set; }

        public virtual void Move(bool isReverse = false)
        {
            MoveX(isReverse);
            MoveY(isReverse);
        }

        public virtual void MoveX(bool isReverse = false)
        {
            X += GetRoundSpeedX() * (isReverse ? -1 : 1);
        }

        public virtual void MoveY(bool isReverse = false)
        {
            Y += GetRoundSpeedY() * (isReverse ? -1 : 1);
        }

        public virtual void ChangeDirection(DirectionX directionX)
        {
            if (directionX == DirectionX)
                return;

            DirectionX = directionX;
            if (DirectionX != DirectionX.Idle)
                Sight = directionX == DirectionX.Left ? Sight.Left : Sight.Right;
        }

        public virtual void ChangeDirection(DirectionY directionY) => DirectionY = directionY;

        public int GetRoundSpeedX() => 
            (int)Math.Round(Speed * (int)DirectionX * (DirectionY != DirectionY.Idle ? Math.Sqrt(2) / 2 : 1));

        public int GetRoundSpeedY() => 
            (int)Math.Round(Speed * (int)DirectionY * (DirectionX != DirectionX.Idle ? Math.Sqrt(2) / 2 : 1));
    }
}
