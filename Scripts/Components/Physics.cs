using System.Collections.Generic;
using System.Drawing;
using Top_Down_shooter.Properties;
using Top_Down_shooter.Scripts.Components;
using Top_Down_shooter.Scripts.GameObjects;
using Top_Down_shooter.Scripts.Source;

namespace Top_Down_shooter.Scripts.Controllers
{
    static class Physics
    {
        private static readonly QuadTree colliders;

        private static readonly HashSet<Collider> trackingCollisions;

        static Physics()
        {
            colliders = new QuadTree(new Rectangle(0, 0, GameSettings.MapWidth, GameSettings.MapHeight));

            trackingCollisions = new HashSet<Collider>();
        }

        public static bool IsCollided(GameObject gameObject) => IsCollided(gameObject, out var other);

        public static bool IsCollided(GameObject gameObject, out List<GameObject> others)
        {
            others = GetCollisions(gameObject.Collider);

            return others.Count > 0;
        }

        public static void Update()
        {
            colliders.Clear();

            foreach (var obj in trackingCollisions)
                colliders.Insert(obj);
        }

        public static void AddToTrackingCollisions(Collider collider)
        {
            trackingCollisions.Add(collider);
        }

        public static void RemoveFromTrackingCollisions(Collider collider)
        {
            trackingCollisions.Remove(collider);
        }

        private static List<GameObject> GetCollisions(Collider collider)
        {
            var collisions = new List<GameObject>();

            foreach (var otherCollider in colliders.GetCandidateToCollision(collider))
            {
                if (collider.IntersectsWith(otherCollider))
                {
                    collisions.Add(otherCollider.GameObject);
                }
            }

            return collisions;
        }
    }
}
