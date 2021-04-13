using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Top_Down_shooter
{
    class Gun : Sprite
    {
        public readonly Point SpawnBullets = new Point(10, -10);

        public Gun(int x, int y, Bitmap image)
        {
            X = x;
            Y = y;
            Image = image;
        }

        public override void Move(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    class Bullet : Sprite
    {
        public int Speed { get; set; }

        public Bullet(int x, int y, int speed, float angle, Bitmap image)
        {
            Speed = speed;
            X = x;
            Y = y;
            Angle = angle;
            Image = image;
        }

        public override void Move()
        {
            X += (int)(Speed * Math.Cos(Angle));
            Y += (int)(Speed * Math.Sin(Angle));
        }
    }
}
