using System.Drawing;
using Top_Down_shooter.Scripts.Components;
using Top_Down_shooter.Scripts.Source;

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
            Collider = new Collider(this, localX: 0, localY: 30, width: 60, height: 60);
            HitBox = new Collider(this, localX: 0, localY: 0, width: 60, height: 90, isTrigger: true);

            X = x;
            Y = y;
            Speed = GameSettings.PlayerSpeed;
            Health = GameSettings.PlayerHealth;

            Gun = new Gun(X + OffsetPositionGun.X, Y + OffsetPositionGun.Y);           
        }

        public override void Move(bool isReverse = false)
        {
            base.Move(isReverse);
            Gun.Move(X + OffsetPositionGun.X, Y + OffsetPositionGun.Y);
        }
    }
}
