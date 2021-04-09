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
        public float X { get; set; }
        public float Y { get; set; }
        public float Angle { get; set; }

        public static Bitmap Image;

        static Gun()
        {
            Image = new Bitmap(@"Sprites\gun.png");
        }

        public Gun(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    class Bullet
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Speed { get; set; }
        public float Angle { get; set; }

        public static Bitmap Image;

        static Bullet()
        {
            Image = new Bitmap(@"Sprites/Bullet.png");
        }

        public Bullet(float x, float y, float speed, float angle)
        {
            Speed = speed;
            X = x;
            Y = y;
            Angle = angle;         
        }

        public void Move()
        {
            X += Speed * (float)Math.Cos(Angle);
            Y += Speed * (float)Math.Sin(Angle);
        }
    }
}
