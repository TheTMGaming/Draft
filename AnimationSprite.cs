using System.Drawing;
using System;

namespace Top_Down_shooter
{
    class AnimationSprite : Sprite
    {
        public readonly int StateCount;
        public readonly int FrameCount;

        private int frame;
        private int state;

        public AnimationSprite(int xLeft, int yTop, int stateCount, int frameCount, Bitmap atlas)
        {
            X = xLeft;
            Y = yTop;
            StateCount = stateCount;
            FrameCount = frameCount;
            Image = atlas;
        }

        public virtual void PlayAnimation(Graphics g)
        {
            Draw(g, 
                new Point(Image.Width / FrameCount * frame, Image.Height / StateCount * state), 
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
