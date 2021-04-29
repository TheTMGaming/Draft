using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Top_Down_shooter.Scripts.GameObjects;

namespace Top_Down_shooter.Scripts.Controllers
{
    class QuadTree
    {
        private readonly Rectangle bounds;
        private readonly int depth;

        private List<GameObject> objects;
        private readonly List<QuadTree> nodes;

        private readonly int maxObjectsCount = 10;
        private readonly int maxDepth = 4;

        public QuadTree(Rectangle bounds, int nextDepth = 0)
        {
            this.bounds = bounds;
            depth = nextDepth;

            nodes = new List<QuadTree>();
            objects = new List<GameObject>();
        }

        public List<GameObject> GetCandidateToCollision(GameObject gameObject)
        {
            var returnedList = new List<GameObject>(objects);

            if (nodes.Count > 0)
            return returnedList.Concat(
                GetContainedNodes(gameObject)
                 .SelectMany(node => node.GetCandidateToCollision(gameObject))
                )
                .Distinct()
                .ToList();

            return returnedList;
        }

        public void Insert(GameObject gameObject)
        {
            if (!gameObject.Collider.Contains(bounds))
                return;

            if (nodes.Count > 0)
            {
                foreach (var node in GetContainedNodes(gameObject))
                    node.Insert(gameObject);

                return;
            }

            objects.Add(gameObject);

            if (objects.Count > maxObjectsCount && depth < maxDepth)
            {
                if (nodes.Count == 0)
                    Split();

                foreach (var obj in objects)
                {
                    foreach (var node in GetContainedNodes(gameObject))
                        node.Insert(gameObject);
                }

                objects = new List<GameObject>();
            }
        }

        public void Clear()
        {
            objects = new List<GameObject>();

            foreach (var node in nodes)
                node.Clear();
        }

        private void Split()
        {
            var subWidth = bounds.Width / 2;
            var subHeight = bounds.Height / 2;

            nodes.Add(new QuadTree(
                new Rectangle(bounds.X + subWidth, bounds.Y, subWidth, subHeight),
                depth + 1));

            nodes.Add(new QuadTree(
                new Rectangle(bounds.X, bounds.Y, subWidth, subHeight),
                depth + 1));

            nodes.Add(new QuadTree(
                new Rectangle(bounds.X, bounds.Y + subHeight, subWidth, subHeight),
                depth + 1));

            nodes.Add(new QuadTree(
                new Rectangle(bounds.X + subWidth, bounds.Y + subHeight, subWidth, subHeight),
                depth + 1));
        }

        private List<QuadTree> GetContainedNodes(GameObject gameObject)
        {
            var list = new List<QuadTree>();

            var verticalMidpoint = bounds.X + bounds.Width / 2;
            var horizontalMidpoint = bounds.Y + bounds.Height / 2;

            var rect = new Rectangle(
                gameObject.X - gameObject.Size.Width / 2,
                gameObject.Y - gameObject.Size.Height / 2,
                gameObject.Size.Width, gameObject.Size.Height);

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
    }
}
