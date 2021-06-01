using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Top_Down_shooter.Properties;

namespace Top_Down_shooter.Scripts.Renders
{
    class FireRender : IAnimationRender
    {
        private readonly int frameCount = 8;

        private int frame;

        public int X { get; set; }
        public int Y { get; set; }

        public Size Size => new Size(Resources.Fire.Width / frameCount, Resources.Fire.Height);

        public FireRender(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void ChangeTypeAnimation()
        {
            return;
        }

        public void Draw(Graphics g, Point startSlice, Size sizeSlice)
        {
            g.DrawImage(Resources.Fire,
               X, Y,
               new Rectangle(startSlice, sizeSlice),
               GraphicsUnit.Pixel);
        }

        public void Draw(Graphics g)
        {
            Draw(g, new Point(Size.Width * frame, 0), Size);
        }

        public void PlayAnimation() => frame = (frame + 1) % frameCount;
    }
}
