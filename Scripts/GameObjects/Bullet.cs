using System;
using System.Drawing;

namespace Top_Down_shooter.Scripts.GameObjects
{
    class Bullet : GameObject
    {
        public int Speed { get; set; }
        public float Angle { get; set; }

        public Bullet(int x, int y, int speed, float angle)
        {
            Size = new Size(10, 10);
            Speed = speed;
            X = x;
            Y = y;
            Angle = angle;
        }

        public void Move()
        {
            X += (int)(Speed * Math.Cos(Angle));
            Y += (int)(Speed * Math.Sin(Angle));
        }
    }
}
