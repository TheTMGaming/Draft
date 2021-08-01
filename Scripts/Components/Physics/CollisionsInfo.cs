using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Top_Down_shooter.Scripts.Components
{
    class CollisionsInfo
    {
        private readonly QuadTree _colliders;

        public CollisionsInfo(Rectangle trackedZone, int maxObjectsCountInNode = 10, int maxDepthTree = 4)
        {
            _colliders = new QuadTree(trackedZone, maxObjectsCountInNode, maxDepthTree);
        }

        public List<Collider> GetCollisionsWith(Collider collider)
        {
            return _colliders
                .GetCandidateToCollision(collider)
                .Where(other => collider.IntersectsWith(other) && collider != other)
                .ToList();
        }

        public void Remove(Collider collider)
        {
            _colliders.Remove(collider);
        }
 
        public void Update(IEnumerable<Collider> colliders)
        {
            _colliders.Clear();

            foreach (var collider in colliders)
                _colliders.Insert(collider);
        }
    }
}
