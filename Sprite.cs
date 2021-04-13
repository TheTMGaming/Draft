using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Top_Down_shooter
{
    class Sprite
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Angle { get; set; }
        public Bitmap Image { get; set; }

        public virtual void Draw(Graphics g)
        {
            Draw(g, new Point(0, 0), new Size(Image.Width, Image.Height));
        }

        public virtual void Draw(Graphics g, Point startSlice, Size sizeSlice)
        {
            g.DrawImage(
               Image, X, Y,
               new Rectangle(startSlice, sizeSlice),
               GraphicsUnit.Pixel);
        }
    }
}
