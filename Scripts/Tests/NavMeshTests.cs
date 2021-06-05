using NUnit.Framework;
using System.Drawing;
using Top_Down_shooter.Scripts.Components;
using Top_Down_shooter.Scripts.Controllers;
using Top_Down_shooter.Scripts.Source;
using System.Reflection;
using Top_Down_shooter.Scripts.GameObjects;
using System;
using System.Linq;

namespace Top_Down_shooter.Scripts.Tests
{
    [TestFixture]
    public class NavMeshTests
    {
        private Map map;
        private NavMeshAgent agent;

        private readonly Random randGenerator = new Random();
        private readonly int tileSize = GameSettings.TileSize;

        [SetUp]
        public void Init()
        {
            GameModel.Initialize();

            map = new Map();
            NavMesh.Bake();

            agent = new NavMeshAgent(new Enemy());
        }

        [Test]
        [Repeat(100)]
        public void GetPath_EndPointIsNotObstacleWhereTargetIsGlass()
        {
            var freeTile = map.FreeTiles[randGenerator.Next(0, map.FreeTiles.Count)];

            agent.Target = new Point(freeTile.X, freeTile.Y);

            agent.ComputePath();

            var actual = agent.Path.Last();

            Assert.IsTrue(actual == agent.Target);
        }

        [Test]
        [Repeat(100)]
        public void GetPath_EndPointIsNotObstacleWhereTargetIsObstacle()
        {
            var obstacles = map.Tiles
                .Cast<GameObject>()
                .Where(t => !map.FreeTiles.Contains(t))
                .ToList();

            var tile = obstacles[randGenerator.Next(0, obstacles.Count)];

            agent.Target = new Point(tile.X, tile.Y);

            agent.ComputePath();

            var actual = agent.Path.Last();

            Assert.IsNull(obstacles.Where(t => t.Collider.Transform.Contains(actual)).FirstOrDefault());
        }
    }
}
