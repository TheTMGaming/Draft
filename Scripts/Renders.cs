using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace Top_Down_shooter
{
    interface IRender
    {
        void Draw(Graphics g);
    }

    interface IAnimationRender
    {
        void Draw(Graphics g, Point startSlice, Size sizeSlice);
    }

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

    class SpriteRender : IRender, IAnimationRender
    {
        private readonly int x, y;
        private readonly Bitmap image;

        public SpriteRender(int xLeft, int yTop, Bitmap image)
        {
            x = xLeft;
            y = yTop;
            this.image = image;
        }

        public void Draw(Graphics g)
        {
            Draw(g, new Point(0, 0), new Size(image.Width, image.Height));
        }

        public void Draw(Graphics g, Point startSlice, Size sizeSlice)
        {
            g.DrawImage(image,
                x, y,
                new Rectangle(startSlice, sizeSlice),
                GraphicsUnit.Pixel);
        }
    }

    class GunRender : IRender
    {
        private readonly Gun gun;
        private readonly Bitmap image;

        public GunRender(Gun gun, Bitmap image)
        {
            this.gun = gun;
            this.image = image;
        }

        public void Draw(Graphics g)
        {
            g.TranslateTransform(gun.X, gun.Y);
            g.RotateTransform((float)(gun.Angle * 180 / Math.PI));
            g.TranslateTransform(-gun.X, -gun.Y);

            g.DrawImage(image,
                gun.X - image.Width / 2, gun.Y - image.Height / 2,
                new Rectangle(0, 0, image.Width, image.Height),
                GraphicsUnit.Pixel);

            g.ResetTransform();
        }
    }

    class HealthBarRender : IRender
    {
        private readonly int x;
        private readonly int y;

        private readonly HealthBar healthBar;
        private readonly Size sizeBar;

        private readonly SpriteRender background;
        private readonly SpriteRender bar;
        private readonly SpriteRender heart;

        private readonly Point offsetBackground = new Point(140, 20);
        private readonly Point offsetBar = new Point(140, 20);

        public HealthBarRender(HealthBar healthBar, int xLeft, int yTop)
        {
            this.healthBar = healthBar;
            x = xLeft;
            y = yTop;

            heart = new SpriteRender(x, y, new Bitmap(@"Sprites/Heart.png"));

            background = new SpriteRender(
                x + offsetBackground.X, y + offsetBackground.Y, new Bitmap(@"Sprites/BackgroundHealthBar.png")
                );

            var imageBar = new Bitmap(@"Sprites/HealthBar.png");
            bar = new SpriteRender(x + offsetBar.X, y + offsetBar.Y, imageBar); 
        }

        public void Draw(Graphics g)
        {
            heart.Draw(g);

            background.Draw(g);

            bar.Draw(g, new Point(0, 0), new Size(sizeBar.Width * healthBar.Percent / 100, sizeBar.Height));
        }
    }
}
