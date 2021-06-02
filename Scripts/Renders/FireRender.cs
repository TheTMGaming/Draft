using System.Collections.Generic;
using System.Drawing;
using Top_Down_shooter.Properties;
using Top_Down_shooter.Scripts.GameObjects;
using Top_Down_shooter.Scripts.Source;
using unvell.D2DLib;

namespace Top_Down_shooter.Scripts.Renders
{
    class FireRender : IAnimationRender
    {
        public int X => fire.X - Size.Width / 2;
        public int Y => fire.Y - Size.Height / 2;

        public static readonly int FrameCount = 8;

        private readonly Fire fire;

        private readonly Bitmap image;

        private int frame;

        public Size Size { get; set; } 

        public FireRender(Fire fire, Bitmap image, int frame = 0)
        {
            this.fire = fire;
            this.image = image;
            this.frame = frame;

            Size = new Size(Resources.Fire.Width / FrameCount, Resources.Fire.Height);
        }

        public void ChangeTypeAnimation()
        {
            return;
        }

        public void Draw(D2DGraphicsDevice device, Point startSlice, Size sizeSlice)
        {
            device.Graphics.DrawBitmap(device.CreateBitmap(image),
                new D2DRect(X, Y, sizeSlice.Width, sizeSlice.Height),
                new D2DRect(startSlice.X, startSlice.Y, sizeSlice.Width, sizeSlice.Height));
        }

        public void Draw(D2DGraphicsDevice device)
        {
            Draw(device, new Point(Size.Width * frame, 0), Size);
        }

        public void PlayAnimation() => frame = (frame + 1) % FrameCount;
    }
}
