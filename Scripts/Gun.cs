using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Top_Down_shooter
{
    class Gun
    {
        public int X { get; set; }
        public int Y { get; set; }
        public float Angle { get; set; }

        public readonly Point SpawnBullets = new Point(10, -10);

        public Gun(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void Move(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    class Bullet
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Speed { get; set; }
        public float Angle { get; set; }

        public Bullet(int x, int y, int speed, float angle)
        {
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
