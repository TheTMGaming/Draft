
namespace Top_Down_shooter.Scripts.Components
{
    class NodeData
    {
        public NodeData Previous { get; set; }
        public int G { get; }
        public int H { get; }
        public int F { get; }

        public NodeData(NodeData previous, int g, int h)
        {
            Previous = previous;
            G = g;
            H = h;
            F = G + H;
        }
    }
}
