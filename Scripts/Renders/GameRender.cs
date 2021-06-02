using System.Collections.Generic;
using System.Drawing;
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

        private static readonly int IntervalUpdateAnimations = 250;

        private static readonly List<IRender> renders = new List<IRender>();

        private static readonly Bitmap playerImage = Resources.Player;
        private static readonly Bitmap gunImage = Resources.Gun;
        private static readonly Bitmap tankImage = Resources.Tank;
        private static readonly Bitmap bossImage = Resources.Boss;
        private static readonly Bitmap fireImage = Resources.Fire;
        private static readonly Bitmap bulletImage = Resources.Bullet;

        public static void Initialize()
        {
            renders.Add(new MapRender(GameModel.Map));

            renders.Add(new PowerupsRender(GameModel.Powerups));

            renders.Add(new CharacterRender(GameModel.Player, playerImage, 4, 2));
            renders.Add(new GunRender(GameModel.Player.Gun, gunImage));
            renders.Add(new EnemiesRender(GameModel.Enemies, tankImage));
            renders.Add(new CharacterRender(GameModel.Boss, bossImage, 2, 2));
            renders.Add(new FiresRender(GameModel.Fires, fireImage));

            renders.Add(new BulletsRender(GameModel.Bullets, bulletImage));

            renders.Add(new HealthBarRender(GameModel.HealthBarPlayer, 60, 625, followCamera: true));
            renders.Add(new HealthBarRender(GameModel.HealthBarBoss, GameModel.Boss, new Point(0, -150), 82));
            renders.Add(new ImageRender(1100, 660, bulletImage, true));

        }

        public static void DrawScene(D2DGraphicsDevice device)
        {
            foreach (var render in renders)
            {
                render.Draw(device);
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
