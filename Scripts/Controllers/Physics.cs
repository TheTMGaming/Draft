using System.Collections.Generic;
using System.Drawing;
using Top_Down_shooter.Properties;
using Top_Down_shooter.Scripts.GameObjects;

namespace Top_Down_shooter.Scripts.Controllers
{
    static class Physics
    {
        private static readonly QuadTree colliders;
        private static readonly HashSet<GameObject> trackingCollisions;

        static Physics()
        {
            colliders = new QuadTree(new Rectangle(0, 0, int.Parse(Resources.MapWidth), int.Parse(Resources.MapHeight)));
            trackingCollisions = new HashSet<GameObject>();
        }

        public static bool IsCollided(GameObject gameObject, out GameObject other)
        {
            foreach (var obj in colliders.GetCandidateToCollision(gameObject))
            {
                if (gameObject.Collider.IntersectsWith(obj.Collider))
                {
                    other = obj;
                    return true;
                }
            }

            other = new GameObject();
            return false;
        }

        public static void Update()
        {
            colliders.Clear();
            foreach (var obj in trackingCollisions)
                colliders.Insert(obj);
        }

        public static void AddToTrackingCollisions(GameObject gameObject)
        {
            trackingCollisions.Add(gameObject);
        }

        public static void RemoveFromTrackingCollisions(GameObject gameObject)
        {
            trackingCollisions.Remove(gameObject);
        }
    }
}
