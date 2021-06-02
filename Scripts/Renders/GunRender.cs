using System;
using System.Drawing;
using Top_Down_shooter.Scripts.GameObjects;
using unvell.D2DLib;

namespace Top_Down_shooter.Scripts.Renders
{
    class GunRender : IRender
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Size Size => image.Size;

        private readonly Gun gun;
        private readonly Bitmap image;

        public GunRender(Gun gun, Bitmap image)
        {
            
            this.gun = gun;
            this.image = image;
        }

        public void Draw(D2DGraphics g)
        {
           
            X = gun.X - image.Width / 2;
            Y = gun.Y - image.Height / 2;
            g.TranslateTransform(gun.X, gun.Y);
            g.RotateTransform((float)(gun.Angle * 180 / Math.PI));
            g.TranslateTransform(-gun.X, -gun.Y);

            g.DrawBitmap(g.Device.CreateBitmapFromGDIBitmap(image),
                new D2DRect(X, Y, image.Width, image.Height));
               

            g.TranslateTransform(gun.X, gun.Y);
            g.RotateTransform(-(float)(gun.Angle * 180 / Math.PI));
            g.TranslateTransform(-gun.X, -gun.Y);
        }
    }

}
