using System.Drawing;

namespace Top_Down_shooter
{
    enum Direction
    {
        Left = -1,
        Idle = 0,
        Right = 1
    }

    class Player
    {
        public Point Position { get; set; }
        public int Speed { get; set; }
        public int Health { get; set; }
        public Direction Direction { get; set; }
        public Bitmap AtlasAnimations { get; private set; }
        public Size Scale { get; set; }

        public Player()
        {
            AtlasAnimations = new Bitmap(@"Sprites\player.png");
            Direction = Direction.Idle;
            Scale = new Size(102, 128);
        }
    }
}
