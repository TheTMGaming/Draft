using System.Collections.Generic;
using System.Drawing;

namespace Top_Down_shooter
{
    class QuadTree
    {
        private readonly Rectangle bounds;
        private readonly int depth;
        private List<Rectangle> objects;
        private readonly List<QuadTree> nodes;

        private static int maxObjectsCount = 10;
        private static int maxDepth = 4;

        public QuadTree(Rectangle bounds, int nextDepth)
        {
            this.bounds = bounds;
            depth = nextDepth;
            nodes = new List<QuadTree>();
            objects = new List<Rectangle>();
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

        public List<QuadTree> GetQuadTreesBelongsTo(Rectangle rect)
        {
            var list = new List<QuadTree>();
            var verticalMidpoint = bounds.X + bounds.Width / 2;
            var horizontalMidpoint = bounds.Y + bounds.Height / 2;

            if (rect.Y < horizontalMidpoint && rect.X + rect.Width > verticalMidpoint)
                list.Add(nodes[0]);

            if (rect.X < verticalMidpoint && rect.Y < horizontalMidpoint)
                list.Add(nodes[1]);

            if (rect.X < verticalMidpoint && rect.Y + rect.Height > horizontalMidpoint)
                list.Add(nodes[2]);

            if (rect.X + rect.Width > verticalMidpoint && rect.Y + rect.Height > horizontalMidpoint)
                list.Add(nodes[3]);

            return list;
        }

        public void Insert(Rectangle rect)
        {
            if (nodes.Count > 0)
            {
                foreach (var node in GetQuadTreesBelongsTo(rect))
                    node.Insert(rect);

                return;
            }

            objects.Add(rect);

            if (objects.Count > maxObjectsCount && depth < maxDepth)
            {
                if (nodes.Count == 0)
                    Split();

                foreach (var obj in objects)
                {
                    foreach (var node in GetQuadTreesBelongsTo(obj))
                        node.Insert(obj);
                }
            }

            objects = new List<Rectangle>();
        }
    }
}
