using System.Drawing;

namespace Top_Down_shooter.Scripts.Components
{
    class Node
    {
        public Collider Parent { get; set; }
        public Point Position { get; set; }
        public bool IsObstacle { get; set; }
        public int G { get; private set; }
        public int H { get; private set; }
        public int F { get; private set; }

        public Node(Point position, int g = 0, int h = 0, bool isObstacle = false)
        {
            Position = position;
            IsObstacle = isObstacle;
            SetPathParameters(g, h);
        }

        public void SetPathParameters(int g, int h)
        {
            G = g;
            H = h;
            F = G + H;
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", Position, IsObstacle);
        }
    }
}
