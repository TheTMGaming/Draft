using System.Drawing;
using Top_Down_shooter.Scripts.Components;

namespace Top_Down_shooter.Scripts.GameObjects
{
    class GameObject
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Point Transform => new Point(X, Y);
        public Collider Collider { get; set; }
    }
}
