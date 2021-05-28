using System;
using System.Collections.Generic;
using System.Drawing;
using Top_Down_shooter.Scripts.GameObjects;
using Top_Down_shooter.Scripts.UI;
using Top_Down_shooter.Scripts.Controllers;
using Top_Down_shooter.Scripts.Components;
using Top_Down_shooter.Scripts.Source;
using System.ComponentModel;

namespace Top_Down_shooter
{
    static class GameModel
    {
        public static readonly Player Player;
        public static readonly List<Tank> Enemies;
        public static readonly Map Map;
        public static readonly HealthBar HealthBar;
        public static readonly LinkedList<Bullet> Bullets;

        private static readonly Random randGenerator = new Random();

        static GameModel()
        {
            Player = new Player(120, 120);
            Map = new Map();
            NavMeshAgent.Bake(Map);

            Enemies = new List<Tank>();
            for (var i = 0; i < 6; i++)
            {
                SpawnEnemy();
            }
            
            HealthBar = new HealthBar(100);


            Bullets = new LinkedList<Bullet>();         
        }

        public static void SpawnEnemy()
        {
            var tile = Map.FreeTiles[randGenerator.Next(0, Map.FreeTiles.Count)];
            var enemy = new Tank(tile.X, tile.Y);

            Enemies.Add(enemy);
            Physics.AddToTrackingCollisions(enemy.HitBox);
        }

        public static void RespawnEnemy(Character character)
        {
            var tile = Map.FreeTiles[randGenerator.Next(0, Map.FreeTiles.Count)];

            character.X = tile.X;
            character.Y = tile.Y;
            character.Health = GameSettings.TankHealth;
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
            Map.Tiles[(box.X - 64 / 2) / 64, (box.Y - 64 / 2) / 64] = new Grass(box.X, box.Y);
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
