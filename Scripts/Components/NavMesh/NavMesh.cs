using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Top_Down_shooter.Scripts.Controllers;
using Top_Down_shooter.Scripts.GameObjects;
using Top_Down_shooter.Scripts.Source;

namespace Top_Down_shooter.Scripts.Components
{
    static class NavMesh
    {
        public static readonly Node[,] Map;
        public static readonly List<NavMeshAgent> Agents = new List<NavMeshAgent>();

        public static readonly int Width;
        public static readonly int Height;
        public static readonly int DistanceFromObstacle;
        public static readonly int StepAgent = 32;
        public static readonly int TimeUpdate = 300;

        public static readonly int CostOrthogonalPoint = 10;

        private static Map tileMap;

        static NavMesh()
        {
            Width = GameSettings.MapWidth / StepAgent;
            Height = GameSettings.MapHeight / StepAgent;

            DistanceFromObstacle = Math.Max(
                GameSettings.FiremanSizeCollider, Math.Max(GameSettings.TankSizeCollider, GameSettings.WatermanSizeCollider)) / 2;

            Map = new Node[Width, Height];
            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    Map[x, y] = new Node(new Point(x * StepAgent, y * StepAgent));
                }
            }
        }

        public static void Bake(Map map)
        {
            if (map is null)
                return;
            
            tileMap = map;

            foreach (var tile in map.Tiles)
            {
                if (tile is Grass)
                    continue;

                var rect = tile.Collider.Transform;

                var offset = DistanceFromObstacle - DistanceFromObstacle % StepAgent;

                var xLeft = rect.X - offset;
                if (xLeft < 0) xLeft = 0;

                var yTop = rect.Y - offset;
                if (yTop < 0) yTop = 0;

                var xRight = rect.X + rect.Width + offset;
                if (xRight >= GameSettings.MapWidth)
                    xRight = GameSettings.MapWidth - 1;

                var yBottom = rect.Y + rect.Height + offset;
                if (yBottom >= GameSettings.MapHeight)
                    yBottom = GameSettings.MapHeight - 1;

                for (var x = xLeft; x <= xRight; x += StepAgent)
                {
                    for (var y = yTop; y <= yBottom; y += StepAgent)
                    {
                        Map[x / StepAgent, y / StepAgent].IsObstacle = true;
                    }
                }
            }
        }

        public static void Update()
        {
            while (true)
            {
                Bake(tileMap);

                Thread.Sleep(TimeUpdate);
            }
        }

        public static void AddAgent(NavMeshAgent agent)
        {
            Agents.Add(agent);

            var timer = new Timer((e) => UpdatePath(agent), null, 0, agent.PeriodUpdate);
        }

        private static void UpdatePath(NavMeshAgent agent)
        {
            agent.ComputePath();
        }
    }
}
