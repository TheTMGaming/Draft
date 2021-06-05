using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using Top_Down_shooter.Scripts.Components;
using Top_Down_shooter.Scripts.GameObjects;
using Top_Down_shooter.Scripts.Source;

namespace Top_Down_shooter.Scripts.Controllers
{
    static class Physics
    {
        public static HashSet<Collider> Colliders => new HashSet<Collider>(trackingColliders);

        private static readonly QuadTree colliders;
        private static readonly HashSet<Collider> trackingColliders;

        private static readonly Queue<Collider> newColliders = new Queue<Collider>();
        private static readonly Queue<Collider> removedColliders = new Queue<Collider>();

        private static readonly int TimeUpdate = 20;

        private static readonly object locker = new object();

        static Physics()
        {
            colliders = new QuadTree(new Rectangle(0, 0, GameSettings.MapWidth, GameSettings.MapHeight));

            trackingColliders = new HashSet<Collider>();
        }

        public static bool IsCollided(GameObject gameObject) => IsCollided(gameObject, out var other);

        public static bool IsCollided(GameObject gameObject, params Type[] type) => IsCollided(gameObject, out var other, type);

        public static bool IsCollided(GameObject gameObject, out List<GameObject> others, params Type[] type)
        {
            others = GetCollisions(gameObject.Collider, type);

            return others.Count > 0;
        }

        public static void Update()
        {
            while (true)
            {
                lock (locker)
                {
                    while (newColliders.Count > 0)
                        trackingColliders.Add(newColliders.Dequeue());

                    while (removedColliders.Count > 0)
                        trackingColliders.Remove(removedColliders.Dequeue());

                    colliders.Clear();

                    foreach (var collider in trackingColliders)
                        colliders.Insert(collider);
                }

                Thread.Sleep(TimeUpdate);
            }
        }

        public static void AddToTrackingCollisions(Collider collider)
        {
            lock (locker)
            {
                newColliders.Enqueue(collider);
            }
        }

        public static void RemoveFromTrackingCollisions(Collider collider)
        {
            lock (locker)
            {
                removedColliders.Enqueue(collider);
            }
        }

        private static List<GameObject> GetCollisions(Collider collider, Type[] type)
        {
            lock (locker)
            {
                var collisions = new List<GameObject>();

                foreach (var otherCollider in colliders
                    .GetCandidateToCollision(collider)
                    .Where(col => type.Length == 0 || type.Any(t => col.GameObject.GetType().IsAssignableFrom(t))))
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
}
