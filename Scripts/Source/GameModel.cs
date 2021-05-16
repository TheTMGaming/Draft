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
        public static readonly Map Map;
        public static readonly HealthBar HealthBar;
        public static readonly LinkedList<Bullet> Bullets;


        static GameModel()
        {
            Player = new Player(120, 120, 8);
            HealthBar = new HealthBar(100);
            Map = new Map();
            Player.Agent.Bake(Map);

             Player.a = Player.Agent.GetPath(new Point(Player.Collider.X, Player.Collider.Y), new Point(GameSettings.MapWidth / 2, GameSettings.MapHeight / 2));
            Player.b = Player.a.Pop();
            Bullets = new LinkedList<Bullet>();
          
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
