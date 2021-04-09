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
        public readonly float SpawnBulletX = 5;
        public readonly float SpawnBulletY = -25;

        public Gun(float x, float y, Bitmap image)
        {
            X = x;
            Y = y;
            Image = image;
        }

        public void Move(float x, float y)
        {
            X = x;
            Y = y;
        }
    }

    class Bullet : Sprite
    {
        public float Speed { get; set; }

        public Bullet(float x, float y, float speed, float angle, Bitmap image)
        {
            Speed = speed;
            X = x;
            Y = y;
            Angle = angle;
            Image = image;
        }

        public void Move()
        {
            X += Speed * (float)Math.Cos(Angle);
            Y += Speed * (float)Math.Sin(Angle);
        }
    }
}
