using System.Collections.Generic;
using System.Drawing;
using Top_Down_shooter.Properties;
using Top_Down_shooter.Scripts.Controllers;
using Top_Down_shooter.Scripts.Renders;

namespace Top_Down_shooter
{
    static class GameRender
    {
        public static readonly Camera Camera = new Camera();

        private static readonly List<IRender> gameObjects = new List<IRender>();

        static GameRender()
        {
            gameObjects.Add(new MapRender(GameModel.Map));
            gameObjects.Add(new CharacterRender(GameModel.Player, Resources.Player, 4, 2));
            gameObjects.Add(new CharacterRender(GameModel.Tank, Resources.Tank, 4, 2));
            gameObjects.Add(new GunRender(GameModel.Player.Gun, Resources.Gun));
            gameObjects.Add(new BulletsRender(GameModel.Bullets, Resources.Bullet));

        }

        public static void DrawObjects(Graphics g)
        {
            foreach (var obj in gameObjects)
            {
                obj.Draw(g);
            }
        }

        public static void PlayAnimations()
        {
            foreach (var obj in gameObjects)
            {
                if (obj is CharacterRender character)
                {
                    character.ChangeTypeAnimation();
                    character.PlayAnimation();
                }
            }
        }
    }
}
