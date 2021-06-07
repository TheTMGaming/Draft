using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Top_Down_shooter.Scripts.Components;
using Top_Down_shooter.Scripts.Source;
using Top_Down_shooter.Scripts.UI;

namespace Top_Down_shooter.Scripts.GameObjects
{
    class Waterman : Enemy
    {
        public bool IsInvisible { get; set; }
        public bool CanBeVisible { get; set; }
        public readonly System.Timers.Timer TimeInVisibility;
        private readonly System.Timers.Timer TimeInvisibility;

        public bool IsCompleteMovingToTarget { get; private set; }

        private Point nextCheckpoint;
        private Point prevCheckpoint;

        public Waterman(int x, int y, int health, int speed)
        {
            X = x;
            Y = y;
            Health = health;
            Speed = speed;

            Collider = new Collider(this, localX: 0, localY: 30, width: 60, height: 60);
            HitBox = new Collider(this, localX: 0, localY: 0, width: 60, height: 90, isIgnoreNavMesh: true);
            Agent = new NavMeshAgent(this);

            nextCheckpoint = GameModel.Player.Transform;

            //Cooldown = new Timer(new TimerCallback((e) => GameModel.ShootWaterman(this)), null, 0, GameSettings.WatermanCooldown);
            HealthBar = new HealthBar(this);

            Agent.Target = GameModel.Player.Transform;

            TimeInVisibility = new System.Timers.Timer(GameSettings.WatermanTimeInVisibility);
            TimeInVisibility.Elapsed += (e, o) =>
            {
                IsInvisible = true;
                TimeInVisibility.Stop();
            };

            //TimeInvisibility = new System.Timers.Timer(GameSettings.WatermanTimeInvisibility);
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

            if (Math.Sqrt((X - GameModel.Player.X) * (X - GameModel.Player.X)
                    + (Y - GameModel.Player.Y) * (Y - GameModel.Player.Y)) <= GameSettings.WatermanDistanceFire)
            {
                IsInvisible = false;
            }
            else IsInvisible = true;
        }
    }
}

