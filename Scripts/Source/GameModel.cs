using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Top_Down_shooter.Scripts.Controllers;
using Top_Down_shooter.Scripts.GameObjects;
using Top_Down_shooter.Scripts.Source;
using Top_Down_shooter.Scripts.UI;

namespace Top_Down_shooter
{
    static class GameModel
    {
        public static Player Player;
        public static Boss Boss;

        public static readonly object LockerEnemies = new object();
        public static List<Enemy> Enemies;
        public static Queue<Enemy> NewEnemies = new Queue<Enemy>();
        public static Queue<Enemy> RemovedEnemies = new Queue<Enemy>();

        public static readonly object LockerFires = new object();
        public static List<Fire> Fires;
        public static LinkedList<Fire> MovingFires;
        public static Queue<Fire> NewFires = new Queue<Fire>();

        public static HashSet<Powerup> Powerups;

        public static readonly object LockerBullets = new object();
        public static HashSet<Bullet> Bullets = new HashSet<Bullet>();
        public static Queue<Bullet> NewBullets = new Queue<Bullet>();
        public static Queue<Bullet> DeletedBullets = new Queue<Bullet>();

        public static Map Map;
        public static HealthBar HealthBarPlayer;
        public static HealthBar HealthBarBoss;

        public static bool IsEnd = false;

        private static readonly Random randGenerator = new Random();


        public static void Initialize()
        {
            Player = new Player(120, 120);
            HealthBarPlayer = new HealthBar(Player);
            Physics.AddToTrackingHitBoxes(Player.HitBox);

            Boss = new Boss(GameSettings.MapWidth / 2 , GameSettings.MapHeight / 2, GameSettings.BossHealth);
            HealthBarBoss = new HealthBar(Boss);
            Physics.AddToTrackingColliders(Boss.Collider);
            Physics.AddToTrackingHitBoxes(Boss.HitBox);

            Map = new Map();

            Fires = new List<Fire>();
            MovingFires = new LinkedList<Fire>();

            Enemies = new List<Enemy>();
            for (var i = 0; i < GameSettings.StartEnemiesCount; i++)
                SpawnEnemy();

            Powerups = new HashSet<Powerup>();
            for (var i = 0; i < GameSettings.SmallLootsCount; i++)
                SpawnSmallLoot();

            for (var i = 0; i < GameSettings.CountHPPowerups; i++)
            {
                var tile = Map.FreeTiles[randGenerator.Next(0, Map.FreeTiles.Count)];
                var powerup = new HP(new Powerup(tile.X, tile.Y));

                Powerups.Add(powerup);

                GameRender.AddRenderFor(powerup);
                Physics.AddToTrackingHitBoxes(powerup.Collider);
            }      
        }

        public static void SpawnEnemy()
        {
            var resetPath = randGenerator.Next(GameSettings.TankResetPathMin, GameSettings.TankResetPathMax);

            var speed = randGenerator.Next(
                Math.Min(GameSettings.TankSpeedMin, GameSettings.PlayerSpeed - 1), Math.Max(GameSettings.TankSpeedMin, GameSettings.PlayerSpeed - 1));
            if (randGenerator.NextDouble() > 1 - GameSettings.ProbabilitiSpeedMax)
            {
                speed = randGenerator.Next(
                    Math.Min(GameSettings.PlayerSpeed, GameSettings.TankSpeedMax), Math.Max(GameSettings.PlayerSpeed, GameSettings.TankSpeedMax));
            }

            var health = randGenerator.Next(GameSettings.TankHealthMin, GameSettings.TankHealthMax);

            var enemy = new Tank(GameSettings.MapWidth / 2, GameSettings.MapHeight / 2, health, speed, resetPath, randGenerator.Next(0, 5));

            Enemies.Add(enemy);
            GameRender.AddRenderFor(enemy);
            Physics.AddToTrackingColliders(enemy.Collider);
            Physics.AddToTrackingHitBoxes(enemy.HitBox);
        }

        public static void SpawnSmallLoot()
        {
            var tile = Map.FreeTiles[randGenerator.Next(0, Map.FreeTiles.Count)];

            var loot = new SmallLoot(new Powerup(tile.X, tile.Y));

            Powerups.Add(loot);

            GameRender.AddRenderFor(loot);

            Physics.AddToTrackingHitBoxes(loot.Collider);       
        }

        public static void SpawnFire()
        {
            lock (LockerFires)
            {
                var tile = Map.GetTileIn(Player.X, Player.Y);

                var fire = new Fire(GameSettings.MapWidth / 2, GameSettings.MapHeight / 2,
                    tile.X, tile.Y, randGenerator.Next(GameSettings.FireMinSpeed, GameSettings.FireMaxSpeed));

                NewFires.Enqueue(fire);
                GameRender.AddRenderFor(fire);
                Physics.AddToTrackingHitBoxes(fire.Collider);
            }
        }

        public static void RespawnStaticPowerup(Powerup powerup)
        {
            var tiles = Map.FreeTiles
                .Where(t =>
                    Math.Sqrt((t.X - Player.X) * (t.X - Player.X) + (t.Y - Player.Y) * (t.Y - Player.Y)) 
                    > GameSettings.DistanceBossToSpawnPowerup)
                .ToList();

            if (tiles.Count == 0)
                return;

            var tile = Map.FreeTiles[randGenerator.Next(0, Map.FreeTiles.Count)];
            
            powerup.X = tile.X;
            powerup.Y = tile.Y;
        }

        public static void RespawnEnemy(Enemy enemy)
        {
            if (randGenerator.NextDouble() > 1 - GameSettings.ProbabilitySpawnBigLoot)
            {
                var loot = new BigLoot(new Powerup(enemy.X, enemy.Y));

                Powerups.Add(loot);
                GameRender.AddRenderFor(loot);
                Physics.AddToTrackingHitBoxes(loot.Collider);
            }

            enemy.X = GameSettings.MapWidth / 2;
            enemy.Y = GameSettings.MapHeight / 2;
            enemy.Health = GameSettings.TankHealthMax;
        }

        public static void UpdateTargetFireman(Fireman fireman)
        {
            if (!fireman.IsCompleteMovingToTarget)
                return;

            var targets = new List<GameObject>();

            if (Player.X - GameSettings.FiremanDistanceRotation > GameSettings.TileSize * 2)
                targets.Add(Map.GetTileIn(Player.X - GameSettings.FiremanDistanceRotation, Player.Y));

            if (Player.X + GameSettings.FiremanDistanceRotation < GameSettings.MapWidth - GameSettings.TileSize * 2)
                targets.Add(Map.GetTileIn(Player.X + GameSettings.FiremanDistanceRotation, Player.Y));

            if (Player.Y - GameSettings.FiremanDistanceRotation > GameSettings.TileSize * 2)
                targets.Add(Map.GetTileIn(Player.X, Player.Y - GameSettings.FiremanDistanceRotation));

            if (Player.Y + GameSettings.FiremanDistanceRotation < GameSettings.MapHeight - GameSettings.TileSize * 2)
                targets.Add(Map.GetTileIn(Player.X, Player.Y + GameSettings.FiremanDistanceRotation));

            targets = targets
                .Where(t => t is Grass)
                .ToList();

            if (targets.Count == 0)
                return;

            fireman.Agent.Target = targets[randGenerator.Next(0, targets.Count)].Transform;

        }

        public static void ChangeBoxToGrass(Box box)
        {
            var grass = new Grass(box.X, box.Y);

            Map.Tiles[(box.X - GameSettings.TileSize / 2) / GameSettings.TileSize,
                (box.Y - GameSettings.TileSize / 2) / GameSettings.TileSize] = grass;

            GameRender.AddRenderFor(grass);
        }

        public static void ShootFireman(Fireman fireman)
        {
            lock (LockerBullets)
            {
                if (Math.Sqrt((fireman.X - Player.X) * (fireman.X - Player.X)
                    + (fireman.Y - Player.Y) * (fireman.Y - Player.Y)) <= GameSettings.FiremanDistanceFire)
                {
                    var angle = (float)Math.Atan2(Player.Y - fireman.Y, Player.X - fireman.X);
                    var bullet = new Bullet(fireman, fireman.X, fireman.Y, GameSettings.FiremanSpeedBullet, angle, GameSettings.FiremanDamage);

                    NewBullets.Enqueue(bullet);
                }
            }
        }
    }
}
