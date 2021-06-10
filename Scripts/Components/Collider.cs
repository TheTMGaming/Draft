using System.Drawing;
using Top_Down_shooter.Scripts.GameObjects;

namespace Top_Down_shooter.Scripts.Components
{
    class Collider
    {
        public int X => (parent?.X ?? 0) + localX;
        public int Y => (parent?.Y ?? 0) + localY;
        public Rectangle Transform => new Rectangle(
            parent is null ? localX : (X - Width / 2), 
            parent is null ? localY : (Y - Height / 2), Width, Height);
        public GameObject GameObject => parent;
        public int Width { get; set; }
        public int Height { get; set; }
        public bool IsTrigger { get; set; }
        public bool IsIgnoreNavMesh { get; set; }

        private readonly GameObject parent;
        private readonly int localX;
        private readonly int localY;

        public Collider(GameObject parent, 
            int localX, int localY, 
            int width, int height, 
            bool isTrigger = false, bool isIgnoreNavMesh = false)
        {
            this.parent = parent;
            this.localX = localX;
            this.localY = localY;
            Width = width;
            Height = height;
            IsTrigger = isTrigger;
            IsIgnoreNavMesh = isIgnoreNavMesh;
        }

        public bool IntersectsWith(Collider other)
        {
            if (other is null)
                return false;

            return Transform.IntersectsWith(other.Transform);
        }
    }
}
