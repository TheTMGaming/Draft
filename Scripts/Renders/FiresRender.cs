using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Top_Down_shooter.Properties;
using Top_Down_shooter.Scripts.GameObjects;

namespace Top_Down_shooter.Scripts.Renders
{
    class FiresRender : IAnimationRender
    { 
        public int X { get; set; }
        public int Y { get; set; }

        private readonly List<Fire> fires;
        private readonly int frameCount = 8;

        private int frame;
        private static Random randGenerator = new Random();

        public Size Size => new Size(Resources.Fire.Width / frameCount, Resources.Fire.Height);

        public FiresRender(List<Fire> fires)
        {
            this.fires = fires;
            frame = randGenerator.Next(0, frameCount);
        }

        public void ChangeTypeAnimation()
        {
            return;
        }

        public void Draw(Graphics g, Point startSlice, Size sizeSlice)
        {

            foreach (var fire in fires)
            {
                g.DrawImage(Resources.Fire,
                   fire.X - Size.Width / 2, fire.Y - Size.Height / 2,
                   new Rectangle(startSlice, sizeSlice),
                   GraphicsUnit.Pixel);
            }
        }

        public void Draw(Graphics g)
        {
            Draw(g, new Point(Size.Width * frame, 0), Size);
        }

        public void PlayAnimation() => frame = (frame + 1) % frameCount;
    }
}
