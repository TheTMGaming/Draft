using System.Drawing;
using System.Threading;
using Top_Down_shooter.Scripts.Components;
using Top_Down_shooter.Scripts.Controllers;
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
        private readonly Timer spawnFireman;

        public Fire(int x, int y, int targetX, int targetY, int speed)
        {
            X = x;
            Y = y;
            this.targetX = targetX;
            this.targetY = targetY;

            this.speed = speed;

            Collider = new Collider(this, localX: 0, localY: 0, width: 40, height: 128, isIgnoreNavMesh: true);

            cooldown = new Timer(new TimerCallback((e) => CanKick = true), null, 0, GameSettings.FireCooldown);
            spawnFireman = new Timer(new TimerCallback(e => SpawnFireman()), null, 0, GameSettings.FireSpawnEnemy);
        }

        public void Move()
        {
            var step = MoveTowards(Transform, new Point(targetX, targetY), speed);

            X = step.X;
            Y = step.Y;

            if (X == targetX && Y == targetY)
                IsCompleteMoving = true;
        }

        private void SpawnFireman()
        {
            if (!IsCompleteMoving || GameModel.IsEnd)
                return;

            lock (GameModel.LockerEnemies)
            {
                var fireman = new Fireman(X, Y, GameSettings.FiremanHealth, GameSettings.FiremanSpeed, 0);

                GameModel.NewEnemies.Enqueue(fireman);
            }
        }
    }
}
