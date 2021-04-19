using System.Drawing;

namespace Top_Down_shooter
{
    class QuadTree
    {
        private readonly Rectangle bounds;
        private readonly int depth;
        private readonly Rectangle[] objects;
        private readonly QuadTree[] nodes;

        private static int maxObjectsCount = 10;
        private static int maxLevels = 4;

        public QuadTree(Rectangle bounds, int nextDepth)
        {
            this.bounds = bounds;
            depth = nextDepth;
            nodes = new QuadTree[4];
        }

        private void Split()
        {
            var subWidth = bounds.Width / 2;
            var subHeight = bounds.Height / 2;

            nodes[0] = new QuadTree(
                new Rectangle(bounds.X + subWidth, bounds.Y, subWidth, subHeight),
                depth + 1);

            nodes[1] = new QuadTree(
                new Rectangle(bounds.X, bounds.Y, subWidth, subHeight),
                depth + 1);

            nodes[2] = new QuadTree(
                new Rectangle(bounds.X, bounds.Y + subHeight, subWidth, subHeight),
                depth + 1);

            nodes[3] = new QuadTree(
                new Rectangle(bounds.X + subWidth, bounds.Y + subHeight, subWidth, subHeight),
                depth + 1);
        }

    }
}
