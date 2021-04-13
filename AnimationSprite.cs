using System;
using System.Drawing;

namespace Top_Down_shooter
{
    class AnimationSprite : Sprite
    {
        public int StateCount { get; protected set; }
        public int FrameCount { get; protected set; }
        public Size FrameSize => new Size(Image.Width / FrameCount, Image.Height / StateCount);

        private int frame;
        private int state;

        public override void Draw(Graphics g)
        {
            g.DrawImage(
               Image, X - FrameSize.Width / 2, Y - FrameSize.Height / 2,
               new Rectangle(
                   new Point(FrameSize.Width * frame, FrameSize.Height * state), 
                   FrameSize),
               GraphicsUnit.Pixel);
        }
        
        public void PlayAnimation() => frame = (frame + 1) % FrameCount;

        public void ChangeState(int rowInAtlas)
        {
            if (rowInAtlas < 0 || rowInAtlas >= StateCount) 
                throw new ArgumentException("The number of state is not in Atlas", "rowInAtlas");

            state = rowInAtlas;
        }
    }
}
