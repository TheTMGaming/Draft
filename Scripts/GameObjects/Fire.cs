using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Top_Down_shooter.Scripts.Components;
using Top_Down_shooter.Scripts.Source;

namespace Top_Down_shooter.Scripts.GameObjects
{
    class Fire : GameObject
    {
        public bool CanKick { get; set; } = true;
        public bool IsCompleteMoving = false;

        private readonly int speed;
        private readonly int targetX;
        private readonly int targetY;
        private readonly Timer cooldown;

        public Fire(int x, int y, int targetX, int targetY, int speed)
        {
            X = x;
            Y = y;
            this.targetX = targetX;
            this.targetY = targetY;

            this.speed = speed;

            Collider = new Collider(this, localX: 0, localY: 0, width: 40, height: 128, isTrigger: true, isIgnoreNavMesh: true);
            cooldown = new Timer(new TimerCallback((e) => CanKick = true), null, 0, GameSettings.FireCooldown);
        }

        public void Move()
        {
            var step = MoveTowards(Transform, new Point(targetX, targetY), speed);

            X = step.X;
            Y = step.Y;

            if (X == targetX && Y == targetY)
                IsCompleteMoving = true;
        }
    }
}
