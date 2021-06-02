using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using Top_Down_shooter.Properties;
using Top_Down_shooter.Scripts.Controllers;
using Top_Down_shooter.Scripts.Renders;
using Top_Down_shooter.Scripts.Source;
using unvell.D2DLib;

namespace Top_Down_shooter
{
    static class GameRender
    {
        public static readonly Camera Camera = new Camera();
        public static readonly EnemiesRender EnemiesRender = new EnemiesRender(GameModel.Enemies, Resources.Tank);

        private static readonly int IntervalUpdateAnimations = 250;

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
            gameObjects.Add(new HealthBarRender(GameModel.HealthBarBoss, GameModel.Boss, new Point(0, -150), 82));
            gameObjects.Add(new ImageRender(1100, 660, Resources.BulletIcon, true));

            gameObjects.Add(new FiresRender(GameModel.Fires));
        }

        public static void DrawObjects(D2DGraphics g)
        {
            foreach (var obj in gameObjects)
            {
                obj.Draw(g);
            }
        }

        public static void PlayAnimations()
        {
            while (true)
            {
                foreach (var obj in gameObjects)
                {
                    if (obj is IAnimationRender render)
                    {
                        render.ChangeTypeAnimation();
                        render.PlayAnimation();
                    }
                }

                Thread.Sleep(IntervalUpdateAnimations);
            }
        }
    }
}
