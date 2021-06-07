using System;
using System.Drawing;
using System.Threading;
using Top_Down_shooter.Scripts.Components;
using Top_Down_shooter.Scripts.Source;
using Top_Down_shooter.Scripts.UI;

namespace Top_Down_shooter.Scripts.GameObjects
{
    class Fireman : Sniper
    {
        public Fireman(int x, int y, int health, int speed, int delayCooldown)
        {
            X = x;
            Y = y;
            Health = health;
            Speed = speed;

            Collider = new Collider(this, localX: 0, localY: 30, width: 60, height: 60);
            HitBox = new Collider(this, localX: 0, localY: 0, width: 60, height: 90, isIgnoreNavMesh: true);
            Agent = new NavMeshAgent(this);

            Cooldown = new Timer(new TimerCallback((e) => Shoot()), null, delayCooldown, GameSettings.FiremanCooldown);
            HealthBar = new HealthBar(this);

            Agent = new NavMeshAgent(this);
            Agent.Target = GameModel.Player.Transform;
        }

        public void Shoot()
        {
            lock (GameModel.LockerBullets)
            {
                if (Math.Sqrt((X - GameModel.Player.X) * (X - GameModel.Player.X)
                    + (Y - GameModel.Player.Y) * (Y - GameModel.Player.Y)) <= GameSettings.FiremanDistanceFire)
                {
                    var angle = (float)Math.Atan2(GameModel.Player.Y - Y, GameModel.Player.X - X);
                    var bullet = new Bullet(this, X, Y, GameSettings.FiremanSpeedBullet, angle, GameSettings.FiremanDamage);

                    GameModel.NewBullets.Enqueue(bullet);
                }
            }
        }
    }
}
