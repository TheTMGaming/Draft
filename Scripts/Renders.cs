using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace Top_Down_shooter
{
    interface IRender
    {
        void Draw(Graphics g);

        void Draw(Graphics g, Point startSlice, Size sizeSlice);
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
            Draw(g, new Point(FrameSize.Width * frame, FrameSize.Height * state), FrameSize);
        }

        public void Draw(Graphics g, Point startSlice, Size sizeSlice)
        {
            g.DrawImage(atlasAnimation.Image,
               character.X - FrameSize.Width / 2, character.Y - FrameSize.Height / 2,
               new Rectangle(startSlice, sizeSlice),
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

    class SpriteRender : IRender
    {
        private readonly Sprite sprite;

        public SpriteRender(Sprite sprite)
        {
            this.sprite = sprite;
        }

        public void Draw(Graphics g)
        {
            Draw(g, new Point(0, 0), new Size(sprite.Image.Width, sprite.Image.Height));
        }

        public void Draw(Graphics g, Point startSlice, Size sizeSlice)
        {
            g.DrawImage(sprite.Image,
               sprite.X, sprite.Y,
               new Rectangle(startSlice, sizeSlice),
               GraphicsUnit.Pixel);
        }
    }

    class HealthBarRender : IRender
    {
        private readonly int x;
        private readonly int y;

        private readonly HealthBar healthBar;

        private readonly SpriteRender background;
        private readonly SpriteRender bar;
        private readonly SpriteRender heart;

        private readonly Point offsetBackground = new Point(140, 20);
        private readonly Point offsetBar = new Point(140, 20);

        public HealthBarRender(int xLeft, int yTop)
        {
            x = xLeft;
            y = yTop;

            heart = new SpriteRender(new Sprite(x, y, 0, new Bitmap(@"Sprites/Heart.png")));

            background = new SpriteRender(
                new Sprite(x + offsetBackground.X, y + offsetBackground.Y, 0, new Bitmap(@"Sprites/BackgroundHealthBar.png"))
                );

            bar = new SpriteRender(
                new Sprite(x + offsetBar.X, y + offsetBar.Y, 0, new Bitmap(@"Sprites/HealthBar.png"))
                ); 
        }

        public void Draw(Graphics g)
        {
            heart.Draw(g);
        }
    }
}
