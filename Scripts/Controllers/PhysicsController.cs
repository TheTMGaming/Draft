using System;
using System.Linq;
using System.Drawing;
using Top_Down_shooter.Scripts.Renders;
using Top_Down_shooter.Scripts.GameObjects;

namespace Top_Down_shooter.Scripts.Controllers
{
    static class PhysicsController
    {
        private static QuadTree boxColliders;

        static PhysicsController()
        {
            boxColliders = new QuadTree(new Rectangle(0, 0, 1920, 1080));
        }

        public static bool IsCollide(Character entity, IRender sprite)
        {
            //if (entity.posX + dir.X <= 0 || entity.posX + dir.X >= MapController.cellSize * (MapController.mapWidth - 1) || entity.posY + dir.Y <= 0 || entity.posY + dir.Y >= MapController.cellSize * (MapController.mapHeight - 1))
            //    return true;

            var rect = new Rectangle(
                sprite.X + (int)entity.DirectionX * entity.Speed,
                sprite.Y + (int)entity.DirectionY * entity.Speed,
                sprite.Size.Width, sprite.Size.Height);

            foreach (var collider in boxColliders.GetCandidatesToCollide(rect))
            {
                if (collider.IntersectsWith(rect))
                    return true;
            }

            return false;
        }

        public static void AddCollider(IRender sprite)
        {
            boxColliders.Insert(new Rectangle(sprite.X, sprite.Y, sprite.Size.Width, sprite.Size.Height));
        }
    }
}
