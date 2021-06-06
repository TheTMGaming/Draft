using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using Top_Down_shooter.Properties;
using Top_Down_shooter.Scripts.Components;
using Top_Down_shooter.Scripts.GameObjects;
using Top_Down_shooter.Scripts.Renders;
using Top_Down_shooter.Scripts.Source;

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
                GameImages.Grass.Add(Resources.Grass.Extract(new Rectangle(64 * i, 0, 64, 64)));
            }
        }

        public static void Initialize()
        {
            renders = new List<IRender>()
            {
                new CharacterRender(GameModel.Player, GameImages.Player, 4, 2),
                new GunRender(GameModel.Player.Gun, GameImages.Gun),
                new CharacterRender(GameModel.Boss, GameImages.Boss, 2, 2),
                new HealthBarRender(GameModel.HealthBarPlayer, 60, 625, followCamera: true),
                new ImageRender(1100, 660, GameImages.PlayerBullet, true)
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
            lock (locker)
            {
                if (gameObject is Box box)
                    newRenders.Enqueue(new TileRender(box, GameImages.Box));

                if (gameObject is Grass grass)
                    newRenders.Enqueue(new TileRender(grass, GameImages.Grass[grass.ID]));

                if (gameObject is Block block)
                    newRenders.Enqueue(new TileRender(block, GameImages.Block));

                if (gameObject is Tank tank)
                    newRenders.Enqueue(new CharacterRender(tank, GameImages.Tank, stateCount: 4, frameCount: 2));

                if (gameObject is Fireman fireman)
                    newRenders.Enqueue(new CharacterRender(fireman, GameImages.Fireman, stateCount: 4, frameCount: 2));

                if (gameObject is Bullet bullet)
                {
                    var image = GameImages.PlayerBullet;

                    if (bullet.Parent is Fireman)
                        image = GameImages.FiremanBullet;

                    newRenders.Enqueue(new BulletRender(bullet, image));
                }

                if (gameObject is Fire fire)
                    newRenders.Enqueue(new FireRender(fire, GameImages.Fire, randGenerator.Next(0, FireRender.FrameCount)));

                if (gameObject is Powerup powerup)
                {
                    var image = GameImages.BigLoot;

                    if (powerup is SmallLoot)
                        image = GameImages.SmallLoot;
                    if (powerup is HP)
                        image = GameImages.Heart;

                    newRenders.Enqueue(new ImageRender(0, 0, image, parent: powerup));
                }
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
