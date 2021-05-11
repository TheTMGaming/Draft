using System.Drawing;
using Top_Down_shooter.Scripts.Source;
using Top_Down_shooter.Scripts.GameObjects;

namespace Top_Down_shooter.Scripts.Components
{
    class NavMesh
    {
        private readonly Point?[,] map;
        private readonly Character character;

        private readonly int width;
        private readonly int height;

        public NavMesh(Character character)
        {
            this.character = character;

            width = GameSettings.MapWidth / character.Speed;
            height = GameSettings.MapHeight / character.Speed;

            map = new Point?[width, height];
            Bake();
        }

        private void Bake()
        {
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {

                }
            }
        }
    }
}
