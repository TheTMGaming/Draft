using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Top_Down_shooter.Scripts.GameObjects;

namespace Top_Down_shooter.Scripts.Renders
{
    class BulletRender : IRender
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Size Size => image.Size;

        private readonly Bullet bullet;
        private readonly Bitmap image;

        public BulletRender(Bullet bullet, Bitmap image)
        {
            this.bullet = bullet;
            this.image = image;
        }

        public void Draw(Graphics g)
        {
            X = bullet.X - image.Width / 2;
            Y = bullet.Y - image.Height / 2;

            g.TranslateTransform(bullet.X, bullet.Y);
            g.RotateTransform((float)(bullet.Angle * 180 / Math.PI));
            g.TranslateTransform(-bullet.X, -bullet.Y);

            g.DrawImage(image,
                X, Y,
                new Rectangle(0, 0, image.Width, image.Height),
                GraphicsUnit.Pixel);

            g.ResetTransform();
        }
    }
}
