using System.Drawing;

namespace Top_Down_shooter.Scripts.GameObjects
{
    class GameObject
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Size Size { get; set; }

        public Rectangle Collider => new Rectangle(X - Size.Width / 2, Y - Size.Height / 2, Size.Width, Size.Height);
    }
}
