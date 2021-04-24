using System;
using System.Linq;
using System.Drawing;
using Top_Down_shooter.Scripts.Renders;
using Top_Down_shooter.Scripts.GameObjects;

namespace Top_Down_shooter.Scripts.Controllers
{
    static class PhysicsController
    {
        private static QuadTree boxCollider;

        static PhysicsController()
        {
            boxCollider = new QuadTree(new Rectangle(0, 0, 1920, 1080));
        }

        public static bool IsCollide(Character entity, IAnimationRender sprite)
        {
            var rect = new Rectangle(sprite.X, sprite.Y, sprite.Size.Width, sprite.Size.Height);

            foreach (var collider in boxCollider.GetCandidatesToCollide(rect))
            {
                if (rect.IntersectsWith(rect))
                    return true;
            }

            return false;
        }

        public static void AddCollider(IRender sprite)
        {
            boxCollider.Insert(new Rectangle(sprite.X, sprite.Y, sprite.Size.Width, sprite.Size.Height));
        }
    }
}
