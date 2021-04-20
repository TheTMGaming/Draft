using System.Drawing;

namespace Top_Down_shooter
{
    class Sprite
    {
        public int X { get; set; }
        public int Y { get; set; }
        public float Angle { get; set; }
        public Bitmap Image { get; set; }

        public Sprite(int xLeft, int yTop, float angleInRadian, Bitmap image)
        {
            X = xLeft;
            Y = yTop;
            Angle = angleInRadian;
            Image = image;
        }
    }
}
