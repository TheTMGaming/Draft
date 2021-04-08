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
        public float TrunkX { get; set; }
        public float TrunkY { get; set; }
        public Bitmap AtlasAnimations { get; private set; }
        public Size Scale { get; set; }

        public Gun()
        {
            AtlasAnimations = new Bitmap(@"Sprites\gun.png");
            Scale = new Size(96, 96);
            X = 140;
            Y = 140;
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

        public Bullet(float x, float y, float angle)
        {
            Speed = 20;
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
