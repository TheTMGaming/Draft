using System;
using System.Drawing;
using Top_Down_shooter.Scripts.GameObjects;
using Top_Down_shooter.Scripts.Source;
using unvell.D2DLib;

namespace Top_Down_shooter.Scripts.Renders
{
    enum AnimationTypes
    {
        IdleRight, IdleLeft, RunRight, RunLeft
    }

    class CharacterRender : IAnimationRender
    {
        public int X => character.X - Size.Width / 2;
        public int Y => character.Y - Size.Height / 2;
        public Size Size { get; private set; }

        public int StateCount { get; set; }
        public int FrameCount { get; set; }

        public GameObject Parent => character;

        private readonly Character character;
        private readonly Bitmap atlasAnimation;
        private readonly CharacterHealthBarRender characterHealthBarRender;

        private int frame;
        private int state;
        private static Random randGenerator = new Random();

        private static Point offsetHealthBaEnemy = new Point(0, -75);
        private static Point offsetHealthBarBoss = new Point(-5, -140);

        public CharacterRender(Boss boss, Bitmap atlasAnimation, int stateCount, int frameCount) : 
            this((Character)boss, atlasAnimation, stateCount, frameCount)
        {
            characterHealthBarRender = new CharacterHealthBarRender(boss, offsetHealthBarBoss);
        }

        public CharacterRender(Enemy enemy, Bitmap atlasAnimation, int stateCount, int frameCount) :
            this((Character)enemy, atlasAnimation, stateCount, frameCount)
        {
            characterHealthBarRender = new CharacterHealthBarRender(enemy, offsetHealthBaEnemy);
        }

        public CharacterRender(Character character, Bitmap atlasAnimation, int stateCount, int frameCount)
        {
            this.character = character;
            this.atlasAnimation = atlasAnimation;
            StateCount = stateCount;
            FrameCount = frameCount;

            Size = new Size(atlasAnimation.Width / FrameCount, atlasAnimation.Height / StateCount);
            frame = randGenerator.Next(0, FrameCount);
        }

        public void Draw(D2DGraphicsDevice device)
        {
            Draw(device, new Point(Size.Width * frame, Size.Height * state), Size);

            characterHealthBarRender?.Draw(device);
        }

        public void Draw(D2DGraphicsDevice device, Point startSlice, Size sizeSlice)
        {
            device.Graphics.DrawBitmap(device.CreateBitmap(atlasAnimation),
                new D2DRect(X, Y, sizeSlice.Width, sizeSlice.Height),
                new D2DRect(startSlice.X, startSlice.Y, sizeSlice.Width, sizeSlice.Height));

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
