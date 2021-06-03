using System;
using System.Drawing;
using System.Threading;
using Top_Down_shooter.Scripts.Components;
using Top_Down_shooter.Scripts.Source;

namespace Top_Down_shooter.Scripts.GameObjects
{
    class Fireman : Enemy
    {
        public readonly NavMeshAgent Agent;
        private readonly Timer fire;

        private Point nextCheckpoint;
        private Point prevCheckpoint;

        public Fireman(int x, int y, int health, int speed, int delayCooldown)
        {
            X = x;
            Y = y;
            Health = health;
            Speed = speed;

            Collider = new Collider(this, localX: 0, localY: 30, width: 60, height: 60);
            HitBox = new Collider(this, localX: 0, localY: 0, width: 60, height: 90, isTrigger: true, isIgnoreNavMesh: true);
            Agent = new NavMeshAgent(this);

            nextCheckpoint = GameModel.Player.Transform;

            fire = new Timer(new TimerCallback((e) => Fire()), null, delayCooldown, GameSettings.FiremanCooldown);
        }

        public override void Move(bool isReverse = false)
        {
            Agent.Target = GameModel.Player.Transform;
            if (isReverse)
            {
                X = prevCheckpoint.X;
                Y = prevCheckpoint.Y;
            }

            prevCheckpoint = nextCheckpoint;


            if (Agent.Path.Count > 0)
            {
                nextCheckpoint = Agent.Path.Pop();

                var direction = MoveTowards(Transform, nextCheckpoint, Speed);
                X = direction.X;
                Y = direction.Y;
            }
            LookAt(GameModel.Player.Transform);

        }

        private void Fire()
        {
            if (Math.Sqrt((X - GameModel.Player.X) * (X - GameModel.Player.X) 
                + (Y - GameModel.Player.Y) * (Y - GameModel.Player.Y)) <= GameSettings.FiremanDistanceFire)
            {
                var angle = (float)Math.Atan2(GameModel.Player.Y - Y, GameModel.Player.X - X);
                var bullet = new Bullet(this, X, Y, GameSettings.FiremanSpeedBullet, angle, GameSettings.FiremanDamage);

                GameModel.Bullets.AddLast(bullet);
                GameRender.AddDynamicRenderFor(bullet);
            }
        }
    }
}
