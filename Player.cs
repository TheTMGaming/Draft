using System.Drawing;

namespace Top_Down_shooter
{
    enum Direction
    {
        Idle, MoveLeft, MoveRight
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
            var image = new Bitmap(@"Sprites\player.png");
        }
    }
}
