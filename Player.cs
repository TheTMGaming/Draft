using System.Drawing;

namespace Top_Down_shooter
{
    enum DirectionX
    {
        Left = -1, Idle = 0, Right = 1 
    }

    enum DirectionY
    {
        Up = -1, Idle = 0, Down = 1;
    }

    class Player
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Speed { get; set; }
        public int Health { get; set; }
        public DirectionX DirectionX { get; set; }
        public DirectionY DirectionY { get; set; }
        public Bitmap AtlasAnimations { get; private set; }
        public Size Scale { get; set; }

        public Player()
        {
            AtlasAnimations = new Bitmap(@"Sprites\player.png");
            DirectionX = DirectionX.Idle;
            DirectionY = DirectionY.Idle;
            Scale = new Size(102, 128);
            Speed = 3;
        }
    }
}
