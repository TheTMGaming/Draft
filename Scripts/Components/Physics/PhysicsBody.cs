using System.Numerics;
using Top_Down_shooter.Scripts.GameObjects;

namespace Top_Down_shooter.Scripts.Components
{
    class PhysicsBody : IComponent
    {
        public float Mass { get; set; }
        public Vector2 Force { get; set; }
        public Vector2 Velocity { get; set; }
        public Collider Collider => _collider;
        public GameObject Parent => _parent;

        private readonly GameObject _parent;
        private readonly Collider _collider;

        public PhysicsBody(GameObject parent, int localX, int localY, int width, int height)
        {
            _parent = parent;

        }

        public void Update()
        {
            var acceleration = Force / Mass;
            Velocity += acceleration 
        }
    }
}
