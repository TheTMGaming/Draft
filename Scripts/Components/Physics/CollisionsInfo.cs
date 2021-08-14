using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Top_Down_shooter.Scripts.Components
{
    class CollisionsInfo
    {
        private readonly HashSet<Collider> _colliders;
        private readonly QuadTree _tree;

        public CollisionsInfo(Rectangle trackedZone, int maxObjectsCountInNode = 10, int maxDepthTree = 4)
        {
            _colliders = new HashSet<Collider>();
            _tree = new QuadTree(trackedZone, maxObjectsCountInNode, maxDepthTree);
        }

        public List<Collider> GetCollisionsWith(Collider collider)
        {
            return _colliders
                .GetCandidateToCollision(collider)
                .Where(other => collider.IntersectsWith(other) && collider != other)
                .ToList();
        }

        public void Add(Collider collider)
        {
            _colliders.Add(collider);
        }

        public void Remove(Collider collider)
        {
            _colliders.Remove(collider);
        }
 
        public void Update()
        {
            _tree.Clear();

            foreach (var collider in _colliders)
                _tree.Insert(collider);
        }
    }
}
