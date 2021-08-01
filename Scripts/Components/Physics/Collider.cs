using System.Drawing;
using Top_Down_shooter.Scripts.GameObjects;

namespace Top_Down_shooter.Scripts.Components
{
    class Collider
    {
        public int X => (_parent?.X ?? 0) + _localX;
        public int Y => (_parent?.Y ?? 0) + _localY;

        public int Width { get; set; }
        public int Height { get; set; }

        public Rectangle Transform => new Rectangle(
            _parent is null ? _localX : (X - Width / 2), 
            _parent is null ? _localY : (Y - Height / 2), Width, Height);

        public GameObject GameObject => _parent;
   
        private readonly GameObject _parent;
        private readonly int _localX;
        private readonly int _localY;

        public Collider(GameObject parent, 
            int localX, int localY, 
            int width, int height)
        {
            _parent = parent;

            _localX = localX;
            _localY = localY;

            Width = width;
            Height = height;
        }

        public bool IntersectsWith(Collider collider) => !(collider is null) && Transform.IntersectsWith(collider.Transform);
    }
}
