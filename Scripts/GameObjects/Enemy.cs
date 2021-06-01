using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Top_Down_shooter.Scripts.Components;

namespace Top_Down_shooter.Scripts.GameObjects
{
    class Enemy : Character
    {
        public int Damage { get; set; }

        public virtual void LookAt(Point target)
        {
            var direction = new Point(target.X - X, target.Y - Y);

            if (direction.X > 0) ChangeDirection(DirectionX.Right);
            else if (direction.X < 0) ChangeDirection(DirectionX.Left);
            else ChangeDirection(DirectionX.Idle);

            if (direction.Y > 0) ChangeDirection(DirectionY.Down);
            else if (direction.Y < 0) ChangeDirection(DirectionY.Up);
            else ChangeDirection(DirectionY.Idle);
        }
    }
}
