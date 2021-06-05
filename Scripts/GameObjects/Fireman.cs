using System;
using System.Drawing;
using System.Threading;
using Top_Down_shooter.Scripts.Components;
using Top_Down_shooter.Scripts.Source;

namespace Top_Down_shooter.Scripts.GameObjects
{
    class Fireman : Enemy
    {
        public bool IsCompleteMovingToTarget { get; private set; }

        private Point nextCheckpoint;
        private Point prevCheckpoint;

        public Fireman(int x, int y, int health, int speed, int delayCooldown)
        {
            X = x;
            Y = y;
            Health = health;
            Speed = speed;

            Collider = new Collider(this, localX: 0, localY: 30, width: 60, height: 60);
            HitBox = new Collider(this, localX: 0, localY: 0, width: 60, height: 90, isIgnoreNavMesh: true);
            Agent = new NavMeshAgent(this);

            nextCheckpoint = GameModel.Player.Transform;

            Cooldown = new Timer(new TimerCallback((e) => GameModel.ShootFireman(this)), null, delayCooldown, GameSettings.FiremanCooldown);

            Agent.Target = GameModel.Player.Transform;
        }

        public override void Move(bool isReverse = false)
        {
            if (isReverse)
            {
                X = prevCheckpoint.X;
                Y = prevCheckpoint.Y;
            }

            prevCheckpoint = nextCheckpoint;


            if (Agent.Path.Count > 0)
            {
                IsCompleteMovingToTarget = false;

                nextCheckpoint = Agent.Path.Pop();

                var direction = MoveTowards(Transform, nextCheckpoint, Speed);
                X = direction.X;
                Y = direction.Y;
            }
            else
                IsCompleteMovingToTarget = true;

            LookAt(GameModel.Player.Transform);

        }
    }
}
