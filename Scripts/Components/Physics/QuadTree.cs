using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Top_Down_shooter.Scripts.Components
{
    class QuadTree
    {
        public Rectangle Bounds => _bounds;

        private readonly Rectangle _bounds;
        private readonly int _depth;

        private List<Collider> _objects = new List<Collider>();
        private readonly List<QuadTree> _nodes = new List<QuadTree>();

        private readonly int _maxObjectsCount;
        private readonly int _maxDepth;

        public QuadTree(Rectangle bounds, int maxObjectsCount, int maxDepth, int depthNode = 0)
        {
            _bounds = bounds;
            _depth = depthNode;
            _maxObjectsCount = maxObjectsCount;
            _maxDepth = maxDepth;
        }

        public List<Collider> GetCandidateToCollision(Collider gameObject)
        {
            var returnedList = new List<Collider>(_objects);

            if (_nodes.Count > 0)
            {
                return returnedList.Concat(GetContainedNodes(gameObject)
                        .SelectMany(node => node.GetCandidateToCollision(gameObject)))
                    .Distinct()
                    .ToList();
            }

            return returnedList;
        }

        public void Insert(Collider collider)
        {
            if (_nodes.Count > 0)
            {
                foreach (var node in GetContainedNodes(collider))
                    node.Insert(collider);

                return;
            }

            if (!_bounds.Contains(collider.Transform))
                throw new ArgumentException($"Bounds {_bounds} doesn't contain collider {collider}");

            _objects.Add(collider);

            if (_objects.Count > _maxObjectsCount && _depth < _maxDepth)
            {
                if (_nodes.Count == 0)
                    Split();

                foreach (var obj in _objects)
                {
                    foreach (var node in GetContainedNodes(collider))
                        node.Insert(collider);
                }

                _objects = new List<Collider>();
            }
        }

        public void Remove(Collider collider)
        {
            if (_nodes.Count > 0)
            {
                foreach (var node in GetContainedNodes(collider))
                    node.Remove(collider);

                return;
            }

            if (!_objects.Remove(collider))
                throw new ArgumentException($"Collider {collider} doesn't contain in tree");
        }

        public void Clear()
        {
            _objects = new List<Collider>();

            foreach (var node in _nodes)
                node.Clear();
        }

        public bool IsInBounds(Collider collider) => _bounds.IntersectsWith(collider.Transform);

        private void Split()
        {
            var halfWidth = _bounds.Width / 2;
            var halfHeight = _bounds.Height / 2;

            for (var i = 0; i < 2; i++)
            {
                for (var j = 0; j < 2; j++)
                {
                    _nodes.Add(new QuadTree(
                        new Rectangle(_bounds.X + halfWidth * i, _bounds.Y + halfHeight * j, halfWidth, halfHeight),
                        _maxObjectsCount, _maxDepth, _depth + 1));
                }
            }
        }

        private IEnumerable<QuadTree> GetContainedNodes(Collider collider)
        {
            return _nodes.Where(node => node.Bounds.IntersectsWith(collider.Transform));
        }
    }
}
