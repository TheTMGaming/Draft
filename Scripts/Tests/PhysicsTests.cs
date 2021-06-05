using Top_Down_shooter.Scripts.Components;
using NUnit.Framework;
using System.Threading.Tasks;
using Top_Down_shooter.Scripts.GameObjects;

namespace Top_Down_shooter.Scripts.Tests
{
    class PhysicsTests
    {
        private Collider collider1;
        private Collider collider2;
        private Collider collider3;
        private Collider collider4;

        public PhysicsTests()
        {
            GameModel.Initialize();

            collider1 = new Collider(new Tank(50, 50, 0, 0, 0, 0), 0, 0, 100, 100);
            collider2 = new Collider(new Enemy() { X = 60, Y = 60 }, 10, 10, 100, 100);
            collider3 = new Collider(new Fireman(50, 50, 0, 0, 0), 5, 5, 100, 100);
            collider4 = new Collider(new Tank(250, 250, 0, 0, 0, 0), 200, 200, 100, 100);

            Physics.AddToTrackingColliders(collider1);
            Physics.AddToTrackingColliders(collider2);
            Physics.AddToTrackingColliders(collider3);
            Physics.AddToTrackingColliders(collider4);

            Task.Run(() => Physics.Update());
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
        public void IsCollided_ValidCollisionsWithTypeOfGameObject()
        {
            Assert.IsTrue(Physics.IsCollided(collider1, typeof(Enemy)));
            Assert.IsTrue(Physics.IsCollided(collider1, typeof(Fireman)));
            Assert.IsFalse(Physics.IsCollided(collider1, typeof(Tank)));
            Assert.IsTrue(Physics.IsCollided(collider1, typeof(Enemy), typeof(Fireman)));
            Assert.IsTrue(Physics.IsCollided(collider1, typeof(Enemy), typeof(Fireman), typeof(Tank)));

            Assert.IsFalse(Physics.IsCollided(collider2, typeof(Enemy)));
            Assert.IsTrue(Physics.IsCollided(collider2, typeof(Tank)));
            Assert.IsTrue(Physics.IsCollided(collider2, typeof(Fireman)));
            Assert.IsTrue(Physics.IsCollided(collider2, typeof(Enemy), typeof(Fireman), typeof(Tank)));

            Assert.IsTrue(Physics.IsCollided(collider3, typeof(Tank)));
            Assert.IsTrue(Physics.IsCollided(collider3, typeof(Enemy)));
            Assert.IsTrue(Physics.IsCollided(collider3, typeof(Tank), typeof(Enemy)));

            Assert.IsFalse(Physics.IsCollided(collider4, typeof(Tank)));
            Assert.IsFalse(Physics.IsCollided(collider4, typeof(Enemy)));
            Assert.IsFalse(Physics.IsCollided(collider4, typeof(Fireman)));
            Assert.IsFalse(Physics.IsCollided(collider4, typeof(Enemy), typeof(Fireman), typeof(Tank)));
        }
    }
}
