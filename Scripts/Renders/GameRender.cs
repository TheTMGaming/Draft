using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using Top_Down_shooter.Properties;
using Top_Down_shooter.Scripts.Controllers;
using Top_Down_shooter.Scripts.GameObjects;
using Top_Down_shooter.Scripts.Renders;
using Top_Down_shooter.Scripts.Source;
using unvell.D2DLib;

namespace Top_Down_shooter
{
    static class GameRender
    {
        public static readonly Camera Camera = new Camera();

        private static readonly int IntervalUpdateAnimations = 250;

        private static readonly Dictionary<GameObject, IRender> dynamicRender = new Dictionary<GameObject, IRender>();

        private static readonly HashSet<IRender> renders = new HashSet<IRender>();

        private static readonly Random randGenerator = new Random();

        #region
        private static readonly Bitmap playerImage = Resources.Player;
        private static readonly Bitmap gunImage = Resources.Gun;
        private static readonly Bitmap tankImage = Resources.Tank;
        private static readonly Bitmap bossImage = Resources.Boss;
        private static readonly Bitmap fireImage = Resources.Fire;
        private static readonly Bitmap bulletImage = Resources.Bullet;
        #endregion

        public static void Initialize()
        {
            renders.Add(new MapRender(GameModel.Map));

            renders.Add(new PowerupsRender(GameModel.Powerups));

            renders.Add(new CharacterRender(GameModel.Player, playerImage, 4, 2));
            renders.Add(new GunRender(GameModel.Player.Gun, gunImage));
            renders.Add(new CharacterRender(GameModel.Boss, bossImage, 2, 2));
          

            renders.Add(new HealthBarRender(GameModel.HealthBarPlayer, 60, 625, followCamera: true));
            renders.Add(new HealthBarRender(GameModel.HealthBarBoss, GameModel.Boss, new Point(0, -150), 82));
            renders.Add(new ImageRender(1100, 660, bulletImage, true));

        }

        public static void DrawScene(D2DGraphicsDevice device)
        {
            foreach (var render in renders.Where(x => x is MapRender || x is ImageRender)
                .Concat(renders.Where(x => x is CharacterRender))
                .Concat(renders.Where(x => x is BulletRender || x is FireRender || x is GunRender))
                .Concat(renders.Where(x => x is HealthBarRender)))
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

        public static void AddRenderFor(Enemy enemy)
        {
            var image = tankImage;
            // if (enemy is)

            var render = new CharacterRender(enemy, image, 4, 2);

            dynamicRender.Add(enemy, render);
            renders.Add(render);
        }

        public static void AddRenderFor(Bullet bullet)
        {
            var image = bulletImage;
            // if (enemy is)

            var render = new BulletRender(bullet, image);

            dynamicRender.Add(bullet, render);
            renders.Add(render);
        }

        public static void AddRenderFor(Fire fire)
        {
            var render = new FireRender(fire, fireImage, randGenerator.Next(0, FireRender.FrameCount));

            dynamicRender.Add(fire, render);
            renders.Add(render);
        }

        public static void RemoveRenderFrom(GameObject gameObject)
        {
            if (dynamicRender.TryGetValue(gameObject, out var render))
            {
                renders.Remove(render);
                dynamicRender.Remove(gameObject);
            }
        }
    }
}
