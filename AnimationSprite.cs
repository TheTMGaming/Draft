using System;
using System.Drawing;

namespace Top_Down_shooter
{
    class AnimationSprite : Sprite
    {
        public int StateCount { get; protected set; }
        public int FrameCount { get; protected set; }

        private int frame;
        private int state;

        public virtual void PlayAnimation(Graphics g)
        {
            Draw(g, new Point(Image.Width / FrameCount * frame, Image.Height / StateCount * state), 
                new Size(Image.Width / FrameCount, Image.Height / StateCount));
        }
        
        public void NextFrame() => frame = (frame + 1) % FrameCount;

        public void ChangeState(int rowInAtlas)
        {
            if (rowInAtlas < 0 || rowInAtlas >= StateCount) 
                throw new ArgumentException("The number of state is not in Atlas", "rowInAtlas");

            state = rowInAtlas;
        }
    }
}
