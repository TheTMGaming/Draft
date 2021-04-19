using System;
using System.Drawing;

namespace Top_Down_shooter
{
    interface IRender
    {
        void Draw(Graphics g);
    }

    enum AnimationTypes
    {
        IdleRight, IdleLeft, RunRight, RunLeft
    }


    class CharacterRender : IRender
    {
        public int StateCount { get; protected set; }
        public int FrameCount { get; protected set; }
        public Size FrameSize => new Size(atlasAnimation.Image.Width / FrameCount, atlasAnimation.Image.Height / StateCount);

        private readonly Character character;
        private readonly Sprite atlasAnimation;

        private int frame;
        private int state;

        public CharacterRender(Character character, Sprite atlasAnimation)
        {
            this.character = character;
            this.atlasAnimation = atlasAnimation;
        }

        public void Draw(Graphics g)
        {
            g.DrawImage(
               atlasAnimation.Image, character.X - FrameSize.Width / 2, character.Y - FrameSize.Height / 2,
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

        public static AnimationTypes GetAnimationType(Character character)
        {
            if (character.DirectionX == DirectionX.Idle && character.DirectionY == DirectionY.Idle)
                return character.Sight == Sight.Left ? AnimationTypes.IdleLeft : AnimationTypes.IdleRight;

            return character.Sight == Sight.Left ? AnimationTypes.RunLeft : AnimationTypes.RunRight;
        }
    }
}
