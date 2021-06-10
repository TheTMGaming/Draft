using System;
using System.Drawing;
using Top_Down_shooter.Scripts.GameObjects;
using Top_Down_shooter.Scripts.Source;
using unvell.D2DLib;

namespace Top_Down_shooter.Scripts.Renders
{
    class GunRender : IRender
    {
        public int X => gun.X - Size.Width / 2;
        public int Y => gun.Y - Size.Height / 2;
        public Size Size => image.Size;

        public GameObject Parent => gun;

        private readonly Gun gun;
        private readonly Bitmap image;

        public GunRender(Gun gun, Bitmap image)
        {           
            this.gun = gun;
            this.image = image;
        }

        public void Draw(D2DGraphicsDevice device)
        {
            var g = device.Graphics;

            var t = g.GetTransform();

            g.RotateTransform((float)(gun.Angle * 180 / Math.PI), new D2DPoint(gun.X, gun.Y));

            g.DrawBitmap(device.CreateBitmap(image),
                new D2DRect(X, Y, image.Width, image.Height));

            g.SetTransform(t);
        }
    }
}
