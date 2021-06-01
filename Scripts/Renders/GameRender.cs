using System.Collections.Generic;
using System.Drawing;
using Top_Down_shooter.Properties;
using Top_Down_shooter.Scripts.Controllers;
using Top_Down_shooter.Scripts.Renders;
using Top_Down_shooter.Scripts.Source;

namespace Top_Down_shooter
{
    static class GameRender
    {
        public static readonly Camera Camera = new Camera();
        public static readonly EnemiesRender EnemiesRender = new EnemiesRender(GameModel.Enemies, Resources.Tank);

        private static readonly List<IRender> gameObjects = new List<IRender>();

        public static void Initialize()
        {
            gameObjects.Add(new MapRender(GameModel.Map));

            gameObjects.Add(new PowerupsRender(GameModel.Powerups));

            gameObjects.Add(new CharacterRender(GameModel.Player, Resources.Player, 4, 2));
            gameObjects.Add(EnemiesRender);
            gameObjects.Add(new CharacterRender(GameModel.Boss, Resources.Boss, 2, 2));

            gameObjects.Add(new GunRender(GameModel.Player.Gun, Resources.Gun));
            gameObjects.Add(new BulletsRender(GameModel.Bullets, Resources.Bullet));

            gameObjects.Add(new HealthBarRender(GameModel.HealthBarPlayer, 60, 625, followCamera: true));
            gameObjects.Add(new HealthBarRender(GameModel.HealthBarBoss, GameModel.Boss, new Point(0, -100), 80));
            gameObjects.Add(new ImageRender(1100, 660, Resources.BulletIcon, true));
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
                if (obj is IAnimationRender render)
                {
                    render.ChangeTypeAnimation();
                    render.PlayAnimation();
                }
            }
        }
    }
}
