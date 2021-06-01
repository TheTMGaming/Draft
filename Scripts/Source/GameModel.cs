using System;
using System.Collections.Generic;
using System.Drawing;
using Top_Down_shooter.Scripts.GameObjects;
using Top_Down_shooter.Scripts.UI;
using Top_Down_shooter.Scripts.Controllers;
using Top_Down_shooter.Scripts.Components;
using Top_Down_shooter.Scripts.Source;
using System.ComponentModel;
using System.Linq;

namespace Top_Down_shooter
{
    static class GameModel
    {
        public static readonly Player Player;
        public static readonly List<Tank> Enemies;
        public static readonly Boss Boss;

        public static readonly HashSet<Powerup> Powerups;
        public static readonly Map Map;
        public static readonly HealthBar HealthBar;
        public static readonly LinkedList<Bullet> Bullets;

        private static readonly Random randGenerator = new Random();

        static GameModel()
        {
            Player = new Player(120, 120);
            Physics.AddToTrackingCollisions(Player.HitBox);

            Boss = new Boss(GameSettings.MapWidth / 2 , GameSettings.MapHeight / 2, GameSettings.BossHealth);
            Physics.AddToTrackingCollisions(Boss.Collider);
            Physics.AddToTrackingCollisions(Boss.HitBox);

            Map = new Map();

            Enemies = new List<Tank>();
            for (var i = 0; i < GameSettings.StartEnemiesCount; i++)
            {
                SpawnEnemy();
            }

            Powerups = new HashSet<Powerup>();
            for (var i = 0; i < GameSettings.CountSmallLoots; i++)
            {
                SpawnSmallLoot();
            }
            for (var i = 0; i < GameSettings.CountHPPowerups; i++)
            {
                var tile = Map.FreeTiles[randGenerator.Next(0, Map.FreeTiles.Count)];

                Powerups.Add(new HP(new Powerup(tile.X, tile.Y)));
                Physics.AddToTrackingCollisions(Powerups.Last().Collider);
            }

            HealthBar = new HealthBar(Player);


            Bullets = new LinkedList<Bullet>();         
        }

        public static void SpawnEnemy()
        {
           // var tile = Map.FreeTiles[randGenerator.Next(0, Map.FreeTiles.Count)];

            var resetPath = randGenerator.Next(GameSettings.TankResetPathMin, GameSettings.TankResetPathMax);

            var speed = randGenerator.Next(GameSettings.TankSpeedMin, GameSettings.PlayerSpeed - 1);
            if (randGenerator.NextDouble() > 1 - GameSettings.ProbabilitiSpeedMax)
                speed = randGenerator.Next(Math.Min(GameSettings.PlayerSpeed, GameSettings.TankSpeedMax), Math.Max(GameSettings.PlayerSpeed, GameSettings.TankSpeedMax));

            var health = randGenerator.Next(GameSettings.TankHealthMin, GameSettings.TankHealthMax);

            var enemy = new Tank(GameSettings.MapWidth / 2, GameSettings.MapHeight / 2, health, speed, resetPath, randGenerator.Next(0, 5));

            Enemies.Add(enemy);
            Physics.AddToTrackingCollisions(enemy.Collider);
            Physics.AddToTrackingCollisions(enemy.HitBox);
        }

        public static void SpawnSmallLoot()
        {
            var tile = Map.FreeTiles[randGenerator.Next(0, Map.FreeTiles.Count)];

            Powerups.Add(new SmallLoot(new Powerup(tile.X, tile.Y)));
            Physics.AddToTrackingCollisions(Powerups.Last().Collider);       
        }

        public static void RespawnStaticPowerup(Powerup powerup)
        {
            var tiles = Map.FreeTiles
                .Where(t =>
                    Math.Sqrt((t.X - Player.X) * (t.X - Player.X) + (t.Y - Player.Y) * (t.Y - Player.Y)) > GameSettings.DistancePlayerToSpawner)
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
                    Math.Sqrt((t.X - Player.X) * (t.X - Player.X) + (t.Y - Player.Y) * (t.Y - Player.Y)) > GameSettings.DistancePlayerToSpawner)
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
                Physics.AddToTrackingCollisions(loot.Collider);
            }

            enemy.X = GameSettings.MapWidth / 2;
            enemy.Y = GameSettings.MapHeight / 2;
            enemy.Health = GameSettings.TankHealthMax;
        }

        public static void MoveEnemies()
        {
            foreach (var enemy in Enemies)
                enemy.Move();
        }

        public static Bullet Shoot()
        {
            var newSpawn = RotatePoint(Player.Gun.SpawnBullets, Player.Gun.Angle);

            return new Bullet(
                Player.Gun.X + newSpawn.X, Player.Gun.Y + newSpawn.Y,
                20, Player.Gun.Angle);           
        }

        public static void ChangeBoxToGrass(Box box)
        {
            Map.Tiles[(box.X - GameSettings.TileSize / 2) / GameSettings.TileSize, 
                (box.Y - GameSettings.TileSize / 2) / GameSettings.TileSize] = new Grass(box.X, box.Y);
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
