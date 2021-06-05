using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using Top_Down_shooter.Scripts.GameObjects;
using Top_Down_shooter.Scripts.Source;

namespace Top_Down_shooter.Scripts.Components
{
    static class Physics
    {
        public static HashSet<Collider> Colliders => new HashSet<Collider>(trackingColliders);

        private static readonly QuadTree colliders;
        private static readonly HashSet<Collider> trackingColliders;
        private static readonly Queue<Collider> newColliders = new Queue<Collider>();
        private static readonly Queue<Collider> removedColliders = new Queue<Collider>();

        private static readonly QuadTree hitBoxes;
        private static readonly HashSet<Collider> trackingHitBoxes;
        private static readonly Queue<Collider> newHitBoxes = new Queue<Collider>();
        private static readonly Queue<Collider> removedHitBoxes = new Queue<Collider>();


        private static readonly int TimeUpdate = 20;

        private static readonly object locker = new object();

        static Physics()
        {
            colliders = new QuadTree(new Rectangle(0, 0, GameSettings.MapWidth, GameSettings.MapHeight));
            trackingColliders = new HashSet<Collider>();

            hitBoxes = new QuadTree(new Rectangle(0, 0, GameSettings.MapWidth, GameSettings.MapHeight));
            trackingHitBoxes = new HashSet<Collider>();
        }

        public static bool IsCollided(Collider collider) => IsCollided(collider, out var other);

        public static bool IsCollided(Collider collider, params Type[] typeWith) => IsCollided(collider, out var other, typeWith);

        public static bool IsCollided(Collider collider, out List<Collider> others, params Type[] typeWith)
        {
            others = GetCollisions(collider, typeWith, colliders);

            return others.Count > 0;
        }

        public static bool IsHit(Collider hitBox) => IsHit(hitBox, out var other);

        public static bool IsHit(Collider hitBox, params Type[] typeWith) => IsHit(hitBox, out var other, typeWith);

        public static bool IsHit(Collider hitBox, out List<Collider> others, params Type[] typeWith)
        {
            others = GetCollisions(hitBox, typeWith, hitBoxes);

            return others.Count > 0;
        }

        public static void Update()
        {
            while (true)
            {
                lock (locker)
                {
                    UpdateTree(colliders, trackingColliders, newColliders, removedColliders);

                    UpdateTree(hitBoxes, trackingHitBoxes, newHitBoxes, removedHitBoxes);
                }

                Thread.Sleep(TimeUpdate);
            }
        }

        public static void AddToTrackingColliders(Collider collider)
        {
            lock (locker)
            {
                newColliders.Enqueue(collider);
            }
        }

        public static void RemoveFromTrackingColliders(Collider collider)
        {
            lock (locker)
            {
                removedColliders.Enqueue(collider);
            }
        }

        public static void AddToTrackingHitBoxes(Collider hitBox)
        {
            lock (locker)
            {
                newHitBoxes.Enqueue(hitBox);
            }
        }

        public static void RemoveFromTrackingHitBoxes(Collider hitBox)
        {
            lock (locker)
            {
                removedHitBoxes.Enqueue(hitBox);
            }
        }

        private static List<Collider> GetCollisions(Collider collider, Type[] type, QuadTree tree)
        {
            lock (locker)
            {
                var collisions = new List<Collider>();

                foreach (var otherCollider in tree
                    .GetCandidateToCollision(collider)
                    .Where(col => type.Length == 0 || type.Contains(col.GameObject.GetType())))
                {
                    if (collider.IntersectsWith(otherCollider) && collider != otherCollider)
                    {
                        collisions.Add(otherCollider);
                    }
                }

                return collisions;
            }
        }

        private static void UpdateTree(QuadTree tree, HashSet<Collider> tracking, Queue<Collider> newElements, Queue<Collider> removedElements)
        {
            while (newElements.Count > 0)
                tracking.Add(newElements.Dequeue());

            while (removedElements.Count > 0)
                tracking.Remove(removedElements.Dequeue());

            tree.Clear();

            foreach (var collider in tracking)
                tree.Insert(collider);
        }
    }
}
