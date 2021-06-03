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

        static GameRender()
        {
            for (var i = 0; i < 4; i++)
            {
                grassImage.Add(Resources.Grass.Extract(new Rectangle(64 * i, 0, 64, 64)));
            }
        }

        #region
        private static readonly Bitmap playerImage = Resources.Player;
        private static readonly Bitmap gunImage = Resources.Gun;
        private static readonly Bitmap tankImage = Resources.Tank;
        private static readonly Bitmap bossImage = Resources.Boss;
        private static readonly Bitmap fireImage = Resources.Fire;
        private static readonly Bitmap bulletImage = Resources.Bullet;
        private static readonly Bitmap heartImage = Resources.Heart;
        private static readonly Bitmap bigLootImage = Resources.BigPowerup;
        private static readonly Bitmap smallLootImage = Resources.SmallLoot;
        private static readonly List<Bitmap> grassImage = new List<Bitmap>();
        private static readonly Bitmap boxImage = Resources.Box;
        private static readonly Bitmap blockImage = Resources.Block;
        #endregion

        public static void Initialize()
        {
            renders.Add(new CharacterRender(GameModel.Player, playerImage, 4, 2));
            renders.Add(new GunRender(GameModel.Player.Gun, gunImage));
            renders.Add(new CharacterRender(GameModel.Boss, bossImage, 2, 2));
          

            renders.Add(new HealthBarRender(GameModel.HealthBarPlayer, 60, 625, followCamera: true));
            renders.Add(new HealthBarRender(GameModel.HealthBarBoss, GameModel.Boss, new Point(0, -150), 82));
            renders.Add(new ImageRender(1100, 660, bulletImage, true));
        }

        public static void DrawScene(D2DGraphicsDevice device)
        {
            foreach (var render in renders
                .Where(x => x is TileRender && IsInCameraFocus(x))
                .Concat(renders.Where(x => x is ImageRender && IsInCameraFocus(x)))
                .Concat(renders.Where(x => x is CharacterRender && IsInCameraFocus(x)))
                .Concat(renders.Where(x => (x is BulletRender || x is FireRender || x is GunRender) && IsInCameraFocus(x)))
                .Concat(renders.Where(x => x is HealthBarRender))
                )
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

        public static void AddTIleRender(GameObject tile)
        {
            if (tile is Box box)
            {
                AddDynamicRenderFor(box);
                return;
            }

            var image = blockImage;
            if (tile is Grass grass)
                image = grassImage[grass.ID];

            renders.Add(new TileRender(tile, image));
        }

        public static void AddDynamicRenderFor(Enemy enemy)
        {
            var image = tankImage;
            // if (enemy is)

            var render = new CharacterRender(enemy, image, 4, 2);

            dynamicRender.Add(enemy, render);
            renders.Add(render);
        }

        public static void AddDynamicRenderFor(Bullet bullet)
        {
            var image = bulletImage;
            // if (enemy is)

            var render = new BulletRender(bullet, image);

            dynamicRender.Add(bullet, render);
            renders.Add(render);
        }

        public static void AddDynamicRenderFor(Fire fire)
        {
            var render = new FireRender(fire, fireImage, randGenerator.Next(0, FireRender.FrameCount));

            dynamicRender.Add(fire, render);
            renders.Add(render);
        }

        public static void AddDynamicRenderFor(Powerup powerup)
        {
            var image = bigLootImage;

            if (powerup is SmallLoot)
                image = smallLootImage;

            if (powerup is HP)
                image = heartImage;

            var render = new ImageRender(0, 0, image, parent: powerup);

            dynamicRender.Add(powerup, render);
            renders.Add(render);
        }

        public static void AddDynamicRenderFor(Box box)
        {
            var render = new TileRender(box, boxImage);

            dynamicRender.Add(box, render);
            renders.Add(render);
        }

        public static void RemoveDynamicRenderFrom(GameObject gameObject)
        {
            if (dynamicRender.TryGetValue(gameObject, out var render))
            {
                renders.Remove(render);
                dynamicRender.Remove(gameObject);
            }
        }

        private static bool IsInCameraFocus(IRender render)
        {
            return render.X + render.Size.Width > Camera.X && render.X < Camera.X + Camera.Size.Width
                && render.Y + render.Size.Height > Camera.Y && render.Y < Camera.Y + Camera.Size.Height;
        }
    }
}
