using System.Drawing;

namespace Top_Down_shooter
{
    enum Direction
    {
        IdleRight = 0,
        IdleLeft = 1,
        Right = 2,
        Left = 3      
    }

    class Player
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Speed { get; set; }
        public int Health { get; set; }
        public Direction Direction { get; set; }
        public Bitmap AtlasAnimations { get; private set; }
        public Size Scale { get; set; }

        public Player()
        {
            AtlasAnimations = new Bitmap(@"Sprites\player.png");
            Direction = Direction.IdleRight;
            Scale = new Size(102, 128);
            Speed = 3;
        }
    }
}
