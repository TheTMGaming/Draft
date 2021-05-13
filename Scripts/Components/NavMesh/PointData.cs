
using System.Drawing;

namespace Top_Down_shooter.Scripts.Components
{
    class PointData
    {
        public Point? Previous { get; set; }
        public int G { get; }
        public int H { get; }
        public int F { get; }

        public PointData(Point? previous, int g, int h)
        {
            Previous = previous;
            G = g;
            H = h;
            F = G + H;
        }
    }
}
