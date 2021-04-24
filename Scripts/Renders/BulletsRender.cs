using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Top_Down_shooter.Scripts.GameObjects;

namespace Top_Down_shooter.Scripts.Renders
{
    class BulletsRender : IRender
    {
        private readonly LinkedList<Bullet> bullets;
        private readonly Bitmap image;

        public BulletsRender(LinkedList<Bullet> bullets, Bitmap image)
        {
            this.bullets = bullets;
            this.image = image;
        }

        public Size Size => image.Size;

        public void Draw(Graphics g)
        {
            foreach (var bullet in bullets)
            {
                g.TranslateTransform(bullet.X, bullet.Y);
                g.RotateTransform((float)(bullet.Angle * 180 / Math.PI));
                g.TranslateTransform(-bullet.X, -bullet.Y);

                g.DrawImage(image,
                    bullet.X - image.Width / 2, bullet.Y - image.Height / 2,
                    new Rectangle(0, 0, image.Width, image.Height),
                    GraphicsUnit.Pixel);

                g.ResetTransform();
            }
        }
    }
}
