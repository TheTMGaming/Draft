using System;
using System.Collections.Generic;
using System.Drawing;
using Top_Down_shooter.Scripts.GameObjects;
using Top_Down_shooter.Scripts.Source;
using unvell.D2DLib;

namespace Top_Down_shooter.Scripts.Renders
{
    class BulletsRender : IRender
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Size Size => image.Size;

        private readonly LinkedList<Bullet> bullets;
        private readonly Bitmap image;

        public BulletsRender(LinkedList<Bullet> bullets, Bitmap image)
        {
            this.bullets = bullets;
            this.image = image;
        }

        public void Draw(D2DGraphicsDevice device)
        {
            var g = device.Graphics;

            foreach (var bullet in bullets)
            {
                X = bullet.X - image.Width / 2;
                Y = bullet.Y - image.Height / 2;

                g.TranslateTransform(bullet.X, bullet.Y);
                g.RotateTransform((float)(bullet.Angle * 180 / Math.PI));
                g.TranslateTransform(-bullet.X, -bullet.Y);

                g.DrawBitmap(device.CreateBitmap(image),
                    new D2DRect(X, Y, image.Width, image.Height));

                g.TranslateTransform(bullet.X, bullet.Y);
                g.RotateTransform(-(float)(bullet.Angle * 180 / Math.PI));
                g.TranslateTransform(-bullet.X, -bullet.Y);
            }

        }
    }
}
