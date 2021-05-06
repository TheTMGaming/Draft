using System;
using System.Drawing;
using Top_Down_shooter.Properties;
using Top_Down_shooter.Scripts.GameObjects;

namespace Top_Down_shooter.Scripts.Renders
{
    class BoxRender : IAnimationRender
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Size Size => new Size(sizeFrame, sizeFrame);

        private readonly Box box;
        private readonly Bitmap atlasStates = new Bitmap(Resources.Box);
        private readonly int sizeFrame = int.Parse(Resources.TileSize);

        public BoxRender(Box box)
        {
            this.box = box;
            X = box.X;
            Y = box.Y;
        }
        public void Draw(Graphics g)
        {
            var state = 0;

            if (box.Health < 5 && box.Health > 0)
                state = 1;
            else if (box.Health < 1)
                state = 2;

            Draw(g, new Point(sizeFrame * state, 0), new Size(sizeFrame, sizeFrame));
        }
        
        public void Draw(Graphics g, Point startSlice, Size sizeSlice)
        {
            g.DrawImage(atlasStates,
               X, Y,
               new Rectangle(startSlice, sizeSlice),
               GraphicsUnit.Pixel);
        }

    }
}
