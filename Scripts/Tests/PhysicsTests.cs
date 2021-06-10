using Top_Down_shooter.Scripts.Components;
using NUnit.Framework;
using System.Threading.Tasks;
using Top_Down_shooter.Scripts.GameObjects;
using System.Threading;
using System.Collections.Generic;
using Top_Down_shooter.Scripts.Source;
using System;
using System.Drawing;
using System.Reflection;

namespace Top_Down_shooter.Scripts.Tests
{
    class PhysicsTests
    {
        private Collider collider1;
        private Collider collider2;
        private Collider collider3;
        private Collider collider4;
        private QuadTree tree;
        private readonly Collider map = new Collider(null, 0, 0, GameSettings.MapWidth, GameSettings.MapHeight);

        private static readonly Random randGenerator = new Random();

        [SetUp]
        public void Init()
        {
            tree = new QuadTree(new Rectangle(0, 0, GameSettings.MapWidth, GameSettings.MapHeight));
        }

        public PhysicsTests()
        {
            collider1 = new Collider(null, 0, 0, 100, 100);
            collider2 = new Collider(null, 10, 10, 100, 100);
            collider3 = new Collider(null, 5, 5, 100, 100);
            collider4 = new Collider(null, 200, 200, 100, 100);

            Physics.AddToTrackingColliders(collider1);
            Physics.AddToTrackingColliders(collider2);
            Physics.AddToTrackingColliders(collider3);
            Physics.AddToTrackingColliders(collider4);

            var task = Task.Run(() => Physics.Update());

            Thread.Sleep(3000);
        }

      
        [Test]
        public void IsCollided_ValidCollisions()
        {
            Assert.IsTrue(Physics.IsCollided(collider1));
            Assert.IsTrue(Physics.IsCollided(collider2));
            Assert.IsTrue(Physics.IsCollided(collider3));
            Assert.IsFalse(Physics.IsCollided(collider4));           
        }

        [Test]
        public void IsCollided_ReturnsValidCollisions()
        {
            Physics.IsCollided(collider1, out var others);
            Assert.IsTrue(others.Contains(collider2) && others.Contains(collider3) && !others.Contains(collider4));

            Physics.IsCollided(collider2, out others);
            Assert.IsTrue(others.Contains(collider1) && others.Contains(collider3) && !others.Contains(collider4));

            Physics.IsCollided(collider3, out others);
            Assert.IsTrue(others.Contains(collider1) && others.Contains(collider2) && !others.Contains(collider4));

            Physics.IsCollided(collider4, out others);
            Assert.IsTrue(!others.Contains(collider1) && !others.Contains(collider2) && !others.Contains(collider3));           
        }

        [Test]
        [Repeat(100)]
        public void CheckCollisionWithMap()
        {
            for (var i = 0; i < 20; i++)
            {
                var x = randGenerator.Next(0, GameSettings.MapWidth);
                var width = randGenerator.Next(0, GameSettings.MapWidth - x);

                var y = randGenerator.Next(0, GameSettings.MapHeight);
                var height = randGenerator.Next(0, GameSettings.MapHeight - y);

                tree.Insert(new Collider(null, x, y, width, height));
            }

            foreach (var other in tree.GetCandidateToCollision(map))
                if (!other.IntersectsWith(map))
                    Assert.Fail("No intersection with the map", other.Transform);
        }

        [Test]
        [Repeat(100)]
        public void IntersectsWith_CheckValidIntersection()
        {
            var colliders = new Collider[20];

            for (var i = 0; i < colliders.Length; i++)
            {
                var x = randGenerator.Next(0, GameSettings.MapWidth);
                var width = randGenerator.Next(0, GameSettings.MapWidth - x);

                var y = randGenerator.Next(0, GameSettings.MapHeight);
                var height = randGenerator.Next(0, GameSettings.MapHeight - y);

                var collider = new Collider(null, x, y, width, height);
                tree.Insert(collider);
                colliders[i] = collider;
            }

            foreach (var collider in colliders)
            {
                foreach (var other in tree.GetCandidateToCollision(collider))
                {
                    Assert.IsTrue(other.IntersectsWith(collider) == other.Transform.IntersectsWith(collider.Transform));
                }
            }
        }
    }
}
