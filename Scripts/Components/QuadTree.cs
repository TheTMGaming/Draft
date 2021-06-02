using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Top_Down_shooter.Scripts.Components;
using Top_Down_shooter.Scripts.GameObjects;

namespace Top_Down_shooter.Scripts.Controllers
{
    class QuadTree
    {
        private readonly Rectangle bounds;
        private readonly int depth;

        private List<Collider> objects;
        private readonly List<QuadTree> nodes;

        private readonly int maxObjectsCount = 10;
        private readonly int maxDepth = 4;

        public QuadTree(Rectangle bounds, int nextDepth = 0)
        {
            this.bounds = bounds;
            depth = nextDepth;

            nodes = new List<QuadTree>();
            objects = new List<Collider>();
        }

        public List<Collider> GetCandidateToCollision(Collider gameObject)
        {
            var returnedList = new List<Collider>(objects);

            if (nodes.Count > 0)
                return returnedList.Concat(
                    GetContainedNodes(gameObject)
                     .SelectMany(node => node.GetCandidateToCollision(gameObject))
                    )
                    .ToList()
                    .Distinct()
                    .ToList();

            return returnedList;
        }

        public void Insert(Collider collider)
        {
            if (nodes.Count > 0)
            {
                foreach (var node in GetContainedNodes(collider))
                    node.Insert(collider);

                return;
            }

            objects.Add(collider);

            if (objects.Count > maxObjectsCount && depth < maxDepth)
            {
                if (nodes.Count == 0)
                    Split();

                foreach (var obj in objects)
                {
                    foreach (var node in GetContainedNodes(collider))
                        node.Insert(collider);
                }

                objects = new List<Collider>();
            }
        }

        public void Clear()
        {
            objects = new List<Collider>();

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

        private List<QuadTree> GetContainedNodes(Collider collider)
        {
            var list = new List<QuadTree>();

            var verticalMidpoint = bounds.X + bounds.Width / 2;
            var horizontalMidpoint = bounds.Y + bounds.Height / 2;

            if (collider.Transform.Y < horizontalMidpoint
                && collider.Transform.X + collider.Transform.Width > verticalMidpoint)
            {
                list.Add(nodes[0]);
            }

            if (collider.Transform.X < verticalMidpoint && collider.Transform.Y < horizontalMidpoint)
                list.Add(nodes[1]);

            if (collider.Transform.X < verticalMidpoint 
                && collider.Y + collider.Transform.Height > horizontalMidpoint)
                list.Add(nodes[2]);

            if (collider.Transform.X + collider.Transform.Width > verticalMidpoint 
                && collider.Transform.Y + collider.Transform.Height > horizontalMidpoint)
                list.Add(nodes[3]);

            return list;
        }
    }
}
