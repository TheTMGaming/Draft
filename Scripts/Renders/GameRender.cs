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

        private static List<IRender> renders;
        private static readonly Queue<IRender> newRenders = new Queue<IRender>();
        private static readonly HashSet<GameObject> removedRender = new HashSet<GameObject>();

        private static readonly Random randGenerator = new Random();

        private static readonly object locker = new object();
        private static readonly int IntervalUpdateAnimations = 250;

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
        private static readonly Bitmap firemanImage = Resources.Fireman;
        private static readonly Bitmap bossImage = Resources.Boss;
        private static readonly Bitmap fireImage = Resources.Fire;
        private static readonly Bitmap playerBulletImage = Resources.Bullet;
        private static readonly Bitmap firemanBulletImage = Resources.Fireball;
        private static readonly Bitmap heartImage = Resources.Heart;
        private static readonly Bitmap bigLootImage = Resources.BigPowerup;
        private static readonly Bitmap smallLootImage = Resources.SmallLoot;
        private static readonly List<Bitmap> grassImage = new List<Bitmap>();
        private static readonly Bitmap boxImage = Resources.Box;
        private static readonly Bitmap blockImage = Resources.Block;
        #endregion

        public static void Initialize()
        {
            renders = new List<IRender>()
            {
                new CharacterRender(GameModel.Player, playerImage, 4, 2),
                new GunRender(GameModel.Player.Gun, gunImage),
                new CharacterRender(GameModel.Boss, bossImage, 2, 2),
                new HealthBarRender(GameModel.HealthBarPlayer, 60, 625, followCamera: true),
                new HealthBarRender(GameModel.HealthBarBoss, GameModel.Boss, new Point(0, -150), 82),
                new ImageRender(1100, 660, playerBulletImage, true)
            };
        }

        public static void DrawScene(D2DGraphicsDevice device)
        {
            lock (locker)
            {
                while (newRenders.Count > 0)
                    renders.Add(newRenders.Dequeue());

                renders = renders
                    .Where(render => !removedRender.Contains(render.Parent))
                    .ToList();
                removedRender.Clear();

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
        }

        public static void PlayAnimations()
        {
            while (true)
            {
                lock (locker)
                {
                    foreach (var obj in renders)
                    {
                        if (obj is IAnimationRender render)
                        {
                            render.ChangeTypeAnimation();
                            render.PlayAnimation();
                        }
                    }

                }
                Thread.Sleep(IntervalUpdateAnimations);
            }
        }

        public static void AddRenderFor(GameObject gameObject)
        {
            if (gameObject is Box box)
                newRenders.Enqueue(new TileRender(box, boxImage));

            if (gameObject is Grass grass)
                newRenders.Enqueue(new TileRender(grass, grassImage[grass.ID]));

            if (gameObject is Block block)
                newRenders.Enqueue(new TileRender(block, blockImage));

            if (gameObject is Tank tank)
                newRenders.Enqueue(new CharacterRender(tank, tankImage, stateCount: 4, frameCount: 2));

            if (gameObject is Fireman fireman)
                newRenders.Enqueue(new CharacterRender(fireman, firemanImage, stateCount: 4, frameCount: 2));

            if (gameObject is Bullet bullet)
            {
                var image = playerBulletImage;

                if (bullet.Parent is Fireman)
                    image = firemanBulletImage;

                newRenders.Enqueue(new BulletRender(bullet, image));
            }

            if (gameObject is Fire fire)
                newRenders.Enqueue(new FireRender(fire, fireImage, randGenerator.Next(0, FireRender.FrameCount)));

            if (gameObject is Powerup powerup)
            {
                var image = bigLootImage;

                if (powerup is SmallLoot)
                    image = smallLootImage;
                if (powerup is HP)
                    image = heartImage;

                newRenders.Enqueue(new ImageRender(0, 0, image, parent: powerup));
            }           
        }

        public static void RemoveRender(GameObject parent)
        {
            removedRender.Add(parent);
        }

        private static bool IsInCameraFocus(IRender render)
        {
            return render.X + render.Size.Width > Camera.X && render.X < Camera.X + Camera.Size.Width
                && render.Y + render.Size.Height > Camera.Y && render.Y < Camera.Y + Camera.Size.Height;
        }
    }
}
