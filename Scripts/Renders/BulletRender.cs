using System;
using System.Collections.Generic;
using System.Drawing;
using Top_Down_shooter.Scripts.GameObjects;
using Top_Down_shooter.Scripts.Source;
using unvell.D2DLib;

namespace Top_Down_shooter.Scripts.Renders
{
    class BulletRender : IRender
    {
        public int X => bullet.X - image.Width / 2;
        public int Y => bullet.Y - image.Height / 2;
        public Size Size => image.Size;

        public GameObject Parent => bullet;

        private readonly Bullet bullet;
        private readonly Bitmap image;

        public BulletRender(Bullet bullet, Bitmap image)
        {
            this.bullet = bullet;
            this.image = image;
        }

        public void Draw(D2DGraphicsDevice device)
        {
            var g = device.Graphics;

            var t = g.GetTransform();

            g.RotateTransform((float)(bullet.Angle * 180 / Math.PI), new D2DPoint(bullet.X, bullet.Y));

            g.DrawBitmap(device.CreateBitmap(image),
                new D2DRect(X, Y, image.Width, image.Height));

            g.SetTransform(t);
        }
    }
}
