using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using Top_Down_shooter.Scripts.GameObjects;
using Top_Down_shooter.Scripts.Components;
using Top_Down_shooter.Scripts.Source;

namespace Top_Down_shooter.Scripts.GameObjects
{
    class Tank : Enemy
    {
        public bool CanKick { get; set; }

        public readonly NavMeshAgent Agent;
        private readonly Timer cooldown;

        private Point nextCheckpoint;
        private Point prevCheckpoint;
        private readonly int resetPath;

        public Tank(int x, int y, int health, int speed, int timeResetPath, int delayCooldown)
        {
            X = x;
            Y = y;
            Health = health;
            Speed = speed;

            resetPath = timeResetPath;

            Collider = new Collider(this, localX: 0, localY: 30, width: 60, height: 60);
            HitBox = new Collider(this, localX: 0, localY: 0, width: 60, height: 90, isTrigger: true, isIgnoreNavMesh: true);
            Agent = new NavMeshAgent(this);

            nextCheckpoint = GameModel.Player.Transform;

            cooldown = new Timer(new TimerCallback((e) => CanKick = true), null, delayCooldown, GameSettings.TankCooldown);
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
    }
}
