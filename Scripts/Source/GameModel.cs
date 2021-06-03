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

        public static List<Enemy> Enemies;


        public static List<Fire> Fires;
        public static LinkedList<Fire> MovingFires;

        public static HashSet<Powerup> Powerups;

        public static LinkedList<Bullet> Bullets;

        public static Map Map;
        public static HealthBar HealthBarPlayer;
        public static HealthBar HealthBarBoss;

        private static readonly Random randGenerator = new Random();

        public static void Initialize()
        {
            Player = new Player(120, 120);
            HealthBarPlayer = new HealthBar(Player);
            Physics.AddToTrackingCollisions(Player.HitBox);

            Boss = new Boss(GameSettings.MapWidth / 2 , GameSettings.MapHeight / 2, GameSettings.BossHealth);
            HealthBarBoss = new HealthBar(Boss);
            Physics.AddToTrackingCollisions(Boss.Collider);
            Physics.AddToTrackingCollisions(Boss.HitBox);

            Map = new Map();

            Fires = new List<Fire>();
            MovingFires = new LinkedList<Fire>();

            Enemies = new List<Enemy>();
            for (var i = 0; i < GameSettings.StartEnemiesCount; i++)
                SpawnEnemy();

            Powerups = new HashSet<Powerup>();
            for (var i = 0; i < GameSettings.CountSmallLoots; i++)
                SpawnSmallLoot();

            for (var i = 0; i < GameSettings.CountHPPowerups; i++)
            {
                var tile = Map.FreeTiles[randGenerator.Next(0, Map.FreeTiles.Count)];
                var powerup = new HP(new Powerup(tile.X, tile.Y));

                Powerups.Add(powerup);

                GameRender.AddDynamicRenderFor(powerup);
                Physics.AddToTrackingCollisions(powerup.Collider);
            }

            Bullets = new LinkedList<Bullet>();         
        }

        public static void SpawnEnemy()
        {
            var resetPath = randGenerator.Next(GameSettings.TankResetPathMin, GameSettings.TankResetPathMax);

            var speed = randGenerator.Next(GameSettings.TankSpeedMin, GameSettings.PlayerSpeed - 1);
            if (randGenerator.NextDouble() > 1 - GameSettings.ProbabilitiSpeedMax)
            {
                speed = randGenerator.Next(
                    Math.Min(GameSettings.PlayerSpeed, GameSettings.TankSpeedMax), Math.Max(GameSettings.PlayerSpeed, GameSettings.TankSpeedMax));
            }

            var health = randGenerator.Next(GameSettings.TankHealthMin, GameSettings.TankHealthMax);

            var enemy = new Tank(GameSettings.MapWidth / 2, GameSettings.MapHeight / 2, health, speed, resetPath, randGenerator.Next(0, 5));

            Enemies.Add(enemy);

            GameRender.AddDynamicRenderFor(enemy);

            Physics.AddToTrackingCollisions(enemy.Collider);

            Physics.AddToTrackingCollisions(enemy.HitBox);
        }

        public static void SpawnSmallLoot()
        {
            var tile = Map.FreeTiles[randGenerator.Next(0, Map.FreeTiles.Count)];
            var loot = new SmallLoot(new Powerup(tile.X, tile.Y));

            Powerups.Add(loot);

            GameRender.AddDynamicRenderFor(loot);

            Physics.AddToTrackingCollisions(loot.Collider);       
        }

        public static void SpawnFire()
        {
            var tile = Map.GetTileIn(Player.X, Player.Y);

            var fire = new Fire(GameSettings.MapWidth / 2, GameSettings.MapHeight / 2, 
                tile.X, tile.Y, randGenerator.Next(GameSettings.FireMinSpeed, GameSettings.FireMaxSpeed));

            Fires.Add(fire);

            MovingFires.AddLast(fire);

            GameRender.AddDynamicRenderFor(fire);

            Physics.AddToTrackingCollisions(fire.Collider);
        }

        public static void RespawnStaticPowerup(Powerup powerup)
        {
            var tiles = Map.FreeTiles
                .Where(t =>
                    Math.Sqrt((t.X - Player.X) * (t.X - Player.X) + (t.Y - Player.Y) * (t.Y - Player.Y)) > GameSettings.DistanceBossToSpawnerPowerup)
                .ToList();

            GameObject tile;
            if (tiles.Count == 0)
                tile = Map.FreeTiles[randGenerator.Next(0, Map.FreeTiles.Count)];
            else
                tile = tiles[randGenerator.Next(0, tiles.Count)];
            
            powerup.X = tile.X;
            powerup.Y = tile.Y;
        }

        public static void RespawnEnemy(Enemy enemy)
        {
            var tiles = Map.FreeTiles
                .Where(t =>
                    Math.Sqrt((t.X - Player.X) * (t.X - Player.X) + (t.Y - Player.Y) * (t.Y - Player.Y)) > GameSettings.DistancePlayerToSpawnerMonster)
                .ToList();

            GameObject tile;
            if (tiles.Count == 0)
                tile = Map.FreeTiles[randGenerator.Next(0, Map.FreeTiles.Count)];
            else
                tile = tiles[randGenerator.Next(0, tiles.Count)];

            if (randGenerator.NextDouble() > 1 - GameSettings.ProbabilityBigLoot)
            {
                var loot = new BigLoot(new Powerup(enemy.X, enemy.Y));

                Powerups.Add(loot);
                GameRender.AddDynamicRenderFor(loot);
                Physics.AddToTrackingCollisions(loot.Collider);
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

            fireman.Agent.Target = targets[randGenerator.Next(0, targets.Count)].Transform;

        }

        public static Bullet ShootPlayer()
        {
            var newSpawn = RotatePoint(Player.Gun.SpawnBullets, Player.Gun.Angle);

            return new Bullet(Player,
                Player.Gun.X + newSpawn.X, Player.Gun.Y + newSpawn.Y,
                GameSettings.PlayerBulletSpeed, Player.Gun.Angle, GameSettings.PlayerDamage);
        }

        public static void ChangeBoxToGrass(Box box)
        {
            var grass = new Grass(box.X, box.Y);

            Map.Tiles[(box.X - GameSettings.TileSize / 2) / GameSettings.TileSize,
                (box.Y - GameSettings.TileSize / 2) / GameSettings.TileSize] = grass;

            GameRender.AddTIleRender(grass);
        }

        private static Point RotatePoint(Point point, float angleInRadian)
        {
            return new Point(
                (int)(point.X * Math.Cos(angleInRadian) - point.Y * Math.Sin(angleInRadian)),
                (int)(point.Y * Math.Cos(angleInRadian) + point.X * Math.Sin(angleInRadian))
                );
        }
    }
}
