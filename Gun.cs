using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Top_Down_shooter
{
    class Gun
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Bitmap AtlasAnimations { get; private set; }
        public Size Scale { get; set; }

        public Gun()
        {
            AtlasAnimations = new Bitmap(@"Sprites\gun.png");
            Scale = new Size(96, 96);
            X = 140;
            Y = 140;
        }
    }
}
