using System.Collections.Generic;
using System.Drawing;
using Top_Down_shooter.Properties;
using Top_Down_shooter.Scripts.GameObjects;
using Top_Down_shooter.Scripts.Source;
using unvell.D2DLib;

namespace Top_Down_shooter.Scripts.Renders
{
    class FiresRender : IAnimationRender
    { 
        public int X { get; set; }
        public int Y { get; set; }

        private readonly List<Fire> fires;
        private readonly int frameCount = 8;

        private int frame;

        public Size Size { get; set; } 

        public FiresRender(List<Fire> fires)
        {
            this.fires = fires;
            Size = new Size(Resources.Fire.Width / frameCount, Resources.Fire.Height);
            frame = 0;
        }

        public void ChangeTypeAnimation()
        {
            return;
        }

        public void Draw(D2DGraphicsDevice device, Point startSlice, Size sizeSlice)
        {
            var g = device.Graphics;

            foreach (var fire in fires)
            {
                g.DrawBitmap(device.CreateBitmap(Resources.Fire),
                   new D2DRect(fire.X - Size.Width / 2, fire.Y - Size.Height / 2, sizeSlice.Width, sizeSlice.Height),
                   new D2DRect(startSlice.X, startSlice.Y, sizeSlice.Width, sizeSlice.Height));
            }
        }

        public void Draw(D2DGraphicsDevice device)
        {
            Draw(device, new Point(Size.Width * frame, 0), Size);
        }

        public void PlayAnimation() => frame = (frame + 1) % frameCount;
    }
}
