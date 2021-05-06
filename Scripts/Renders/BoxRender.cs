using System;
using System.Drawing;
using Top_Down_shooter.Properties;
using Top_Down_shooter.Scripts.GameObjects;

namespace Top_Down_shooter.Scripts.Renders
{
    class BoxRender : IRender
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Size Size => new Size(int.Parse(Resources.TileSize), int.Parse(Resources.TileSize));

        private readonly Box box;
        private readonly Bitmap atlasStates = new Bitmap(Resources.Box);
        
        public BoxRender(Box box)
        {
            this.box = box;
            X = box.X - Size.Width / 2;
            Y = box.Y - Size.Height / 2;
        }
        public void Draw(Graphics g)
        {
            g.DrawImage(atlasStates.Blackout(1 - (float)box.Health / Box.MaxHealth),
               X, Y,
               new Rectangle(0, 0, Size.Width, Size.Height),
               GraphicsUnit.Pixel);
        }
      
    }
}
