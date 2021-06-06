using System.Drawing;
using Top_Down_shooter.Scripts.Components;
using Top_Down_shooter.Scripts.Source;
using Top_Down_shooter.Scripts.UI;

namespace Top_Down_shooter.Scripts.GameObjects
{
    class Player : Character
    {
        public Gun Gun { get; set; }

        public override int Health 
        { 
            get => base.Health; 
            set 
            {
                base.Health = value;
                if (value > GameSettings.PlayerHealth)
                    base.Health = GameSettings.PlayerHealth;
            }
        }

        private readonly Point OffsetPositionGun = new Point(20, 38);

        public Player(int x, int y)
        {
            Collider = new Collider(this, localX: 0, localY: 35, width: 55, height: 55);
            HitBox = new Collider(this, localX: 0, localY: 10, width: 60, height: 100);

            X = x;
            Y = y;
            Speed = GameSettings.PlayerSpeed;
            Health = GameSettings.PlayerHealth;

            HealthBar = new HealthBar(this);

            Gun = new Gun(X + OffsetPositionGun.X, Y + OffsetPositionGun.Y);           
        }

        public override void Move(bool isReverse = false)
        {
            base.Move(isReverse);
            Gun.Move(X + OffsetPositionGun.X, Y + OffsetPositionGun.Y);
        }

        public override void MoveX(bool isReverse = false)
        {
            base.MoveX(isReverse);
            Gun.Move(X + OffsetPositionGun.X, Y + OffsetPositionGun.Y);
        }

        public override void MoveY(bool isReverse = false)
        {
            base.MoveY(isReverse);
            Gun.Move(X + OffsetPositionGun.X, Y + OffsetPositionGun.Y);
        }

        public void SetX(int x)
        {
            X = x;
            Gun.Move(X + OffsetPositionGun.X, Y + OffsetPositionGun.Y);
        }

        public void SetY(int y)
        {
            Y = y;
            Gun.Move(X + OffsetPositionGun.X, Y + OffsetPositionGun.Y);
        }
    }
}
