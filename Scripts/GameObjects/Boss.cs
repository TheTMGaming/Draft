using System.Drawing;
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

            Collider = new Collider(this, 10, 10, 150, 270, isIgnoreNavMesh: true);
            HitBox = new Collider(this, localX: 0, localY: 0, width: 110, height: 256, isIgnoreNavMesh: true);
        }

        public override void LookAt(Point target)
        {
            var direction = new Point(target.X - X, target.Y - Y);

            if (direction.X > 0) Sight = Sight.Right;
            else if(direction.X < 0) Sight = Sight.Left;
        }
    }
}
