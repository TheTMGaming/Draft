using System;
using System.Drawing;
using Top_Down_shooter.Scripts.GameObjects;

namespace Top_Down_shooter.Scripts.Renders
{
    enum AnimationTypes
    {
        IdleRight, IdleLeft, RunRight, RunLeft
    }

    class CharacterRender : IRender, IAnimationRender
    {
        public int StateCount { get; set; }
        public int FrameCount { get; set; }
        public Size FrameSize => new Size(atlasAnimation.Width / FrameCount, atlasAnimation.Height / StateCount);

        private readonly Character character;
        private readonly Bitmap atlasAnimation;

        private int frame;
        private int state;

        public CharacterRender(Character character, Bitmap atlasAnimation, int stateCount, int frameCount)
        {
            this.character = character;
            this.atlasAnimation = atlasAnimation;
            StateCount = stateCount;
            FrameCount = frameCount;
        }

        public void Draw(Graphics g)
        {
            Draw(g, new Point(FrameSize.Width * frame, FrameSize.Height * state), FrameSize);
        }

        public void Draw(Graphics g, Point startSlice, Size sizeSlice)
        {
            g.DrawImage(atlasAnimation,
               character.X - FrameSize.Width / 2, character.Y - FrameSize.Height / 2,
               new Rectangle(startSlice, sizeSlice),
               GraphicsUnit.Pixel);
        }

        public void PlayAnimation() => frame = (frame + 1) % FrameCount;

        public void SetState(int rowInAtlas)
        {
            if (rowInAtlas < 0 || rowInAtlas >= StateCount)
                throw new ArgumentException("The number of state is not in Atlas", "rowInAtlas");

            state = rowInAtlas;
        }

        public void ChangeTypeAnimation()
        {
            var rowInAtlas = character.Sight == Sight.Left ? AnimationTypes.RunLeft : AnimationTypes.RunRight;

            if (character.DirectionX == DirectionX.Idle && character.DirectionY == DirectionY.Idle)
                rowInAtlas = character.Sight == Sight.Left ? AnimationTypes.IdleLeft : AnimationTypes.IdleRight;

            state = (int)rowInAtlas;
        }
    }
}
