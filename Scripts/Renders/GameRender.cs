using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
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

        private static readonly int IntervalUpdateAnimations = 250;

        private static readonly List<IRender> renders = new List<IRender>();

        public static void Initialize()
        {
            renders.Add(new MapRender(GameModel.Map));

            renders.Add(new PowerupsRender(GameModel.Powerups));

            renders.Add(new CharacterRender(GameModel.Player, Resources.Player, 4, 2));
            renders.Add(EnemiesRender);
            renders.Add(new CharacterRender(GameModel.Boss, Resources.Boss, 2, 2));

            renders.Add(new GunRender(GameModel.Player.Gun, Resources.Gun));
            renders.Add(new BulletsRender(GameModel.Bullets, Resources.Bullet));

            renders.Add(new HealthBarRender(GameModel.HealthBarPlayer, 60, 625, followCamera: true));
            renders.Add(new HealthBarRender(GameModel.HealthBarBoss, GameModel.Boss, new Point(0, -150), 82));
            renders.Add(new ImageRender(1100, 660, Resources.BulletIcon, true));

            renders.Add(new FiresRender(GameModel.Fires));
        }

        public static void DrawScene(Graphics g)
        {
            foreach (var obj in renders)
            {
                obj.Draw(g);
            }
        }

        public static void PlayAnimations()
        {
            while (true)
            {
                foreach (var obj in renders)
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
