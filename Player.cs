using System.Drawing;

namespace Top_Down_shooter
{
    enum Direction
    {
        Idle, MoveLeft, MoveRight
    }

    class Player
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Speed { get; set; }
        public int Health { get; set; }
        public Direction Direction { get; set; }
        public Bitmap AtlasAnimations { get; private set; }

        public Player()
        {
            var image = new Bitmap(@"Sprites\player.png");
        }
    }
}
