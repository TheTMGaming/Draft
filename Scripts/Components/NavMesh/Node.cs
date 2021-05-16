using System.Drawing;

namespace Top_Down_shooter.Scripts.Components
{
    class Node
    {
        public Point Position { get; set; }
        public bool IsObstacle { get; set; }
        public int G { get; }
        public int H { get; }
        public int F { get; }

        public Node(Point position, int g = 0, int h = 0, bool isObstacle = false)
        {
            Position = position;
            IsObstacle = isObstacle;
            G = g;
            H = h;
            F = G + H;
        }
    }
}
