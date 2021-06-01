using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Top_Down_shooter.Scripts.Components;

namespace Top_Down_shooter.Scripts.GameObjects
{
    class Boss : Enemy
    {
        public Boss(int x, int y, int health)
        {
            X = x;
            Y = y;
            Health = health;

            Collider = new Collider(this, localX: 0, localY: 30, width: 30, height: 30);

            HitBox = new Collider(this, localX: 0, localY: 0, width: 256, height: 128, isTrigger: true);
        }

        public void Update()
        {
            LookAt(GameModel.Player.Transform);
        }

        public override void LookAt(Point target)
        {
            var direction = new Point(target.X - X, target.Y - Y);

            if (direction.X > 0) Sight = Sight.Right;
            else if(direction.X < 0) Sight = Sight.Left;
        }
    }
}
