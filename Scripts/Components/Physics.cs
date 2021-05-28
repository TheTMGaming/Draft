using System.Collections.Generic;
using System.Drawing;
using Top_Down_shooter.Properties;
using Top_Down_shooter.Scripts.GameObjects;
using Top_Down_shooter.Scripts.Source;

namespace Top_Down_shooter.Scripts.Controllers
{
    static class Physics
    {
        private static readonly QuadTree colliders;
        private static readonly QuadTree hitBoxes;

        private static readonly HashSet<GameObject> trackingCollisions;
        private static readonly HashSet<GameObject> trackingHitBoxes;

        static Physics()
        {
            colliders = new QuadTree(new Rectangle(0, 0, GameSettings.MapWidth, GameSettings.MapHeight));
            hitBoxes = new QuadTree(new Rectangle(0, 0, GameSettings.MapWidth, GameSettings.MapHeight));

            trackingCollisions = new HashSet<GameObject>();
            trackingHitBoxes = new HashSet<GameObject>();
        }

        public static bool IsCollided(GameObject gameObject) => IsCollided(gameObject, out var other);

        public static bool IsCollided(GameObject gameObject, out List<GameObject> others)
        {
            others = GetCollisions(gameObject, colliders);

            return others.Count > 0;
        }

        public static bool IsCollidedWith(GameObject gameObject, GameObject other)
        {
            IsCollided(gameObject, out var others);
            foreach (var collision in others)
            {
                if (collision == other)
                    return true;
            }

            return false;
        }

        public static bool IsHit(GameObject gameObject, out GameObject hit)
        {
            foreach (var collisions in GetCollisions(gameObject, hitBoxes))
            {
                hit = collisions;
                return true;
            }

            hit = null;
            return false;
        }

        public static void Update()
        {
            colliders.Clear();
            hitBoxes.Clear();

            foreach (var obj in trackingCollisions)
                colliders.Insert(obj);
            foreach (var obj in trackingHitBoxes)
                hitBoxes.Insert(obj);
        }

        public static void AddToTrackingCollisions(GameObject gameObject)
        {
            trackingCollisions.Add(gameObject);
        }

        public static void RemoveFromTrackingCollisions(GameObject gameObject)
        {
            trackingCollisions.Remove(gameObject);
        }

        public static void AddToTrackingHitBox(GameObject gameObject)
        {
            trackingHitBoxes.Add(gameObject);
        }

        public static void RemoveFromTrackingHitBox(GameObject gameObject)
        {
            trackingHitBoxes.Remove(gameObject);
        }

        private static List<GameObject> GetCollisions(GameObject gameObject, QuadTree tree)
        {
            var collisions = new List<GameObject>();

            foreach (var obj in colliders.GetCandidateToCollision(gameObject))
            {
                if (gameObject.Collider.IntersectsWith(obj.Collider))
                {
                    collisions.Add(obj);
                }
            }

            return collisions;
        }
    }
}
