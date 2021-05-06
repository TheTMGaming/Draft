using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Top_Down_shooter.Properties;
using Top_Down_shooter.Scripts.GameObjects;

namespace Top_Down_shooter.Scripts.Renders
{
    class GrassRender : IRender
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Size Size => new Size(sizeTile, sizeTile);

        private readonly Grass grass;
        private readonly Bitmap image;
        private readonly int sizeTile = int.Parse(Resources.TileSize);

        private static Random randGenerator = new Random();

        public GrassRender(Grass grass)
        {
            this.grass = grass;
            X = grass.X - sizeTile / 2;
            Y = grass.Y - sizeTile / 2;
            image = Resources.Grass.Extract(new Rectangle(sizeTile * randGenerator.Next(0, 4), 0, sizeTile, sizeTile));
        }

        public void Draw(Graphics g)
        {
            g.DrawImage(image,
               X, Y,
               new Rectangle(new Point(0, 0), Size),
               GraphicsUnit.Pixel);
        }
    }
}
