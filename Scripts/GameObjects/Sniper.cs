using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Top_Down_shooter.Scripts.Source;

namespace Top_Down_shooter.Scripts.GameObjects
{
    class Sniper : Enemy
    {
        public bool IsCompleteMovingToTarget { get; set; } = true;

        private Point nextCheckpoint;
        private Point prevCheckpoint;

        private static Random randGenerator = new Random();


        public override void Move(bool isReverse = false)
        {
            UpdateTarget();
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

        public void UpdateTarget()
        {
            if (!IsCompleteMovingToTarget)
                return;

            var targets = new List<GameObject>();

            if (GameModel.Player.X - GameSettings.FiremanDistanceRotation > GameSettings.TileSize * 2)
                targets.Add(GameModel.Map.GetTileIn(GameModel.Player.X - GameSettings.FiremanDistanceRotation, GameModel.Player.Y));

            if (GameModel.Player.X + GameSettings.FiremanDistanceRotation < GameSettings.MapWidth - GameSettings.TileSize * 2)
                targets.Add(GameModel.Map.GetTileIn(GameModel.Player.X + GameSettings.FiremanDistanceRotation, GameModel.Player.Y));

            if (GameModel.Player.Y - GameSettings.FiremanDistanceRotation > GameSettings.TileSize * 2)
                targets.Add(GameModel.Map.GetTileIn(GameModel.Player.X, GameModel.Player.Y - GameSettings.FiremanDistanceRotation));

            if (GameModel.Player.Y + GameSettings.FiremanDistanceRotation < GameSettings.MapHeight - GameSettings.TileSize * 2)
                targets.Add(GameModel.Map.GetTileIn(GameModel.Player.X, GameModel.Player.Y + GameSettings.FiremanDistanceRotation));

            targets = targets
                .Where(t => t is Grass)
                .ToList();

            if (targets.Count == 0)
                return;

            Agent.Target = targets[randGenerator.Next(0, targets.Count)].Transform;
        }
    }
}
