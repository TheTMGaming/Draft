//using NUnit.Framework;
//using System.Drawing;
//using Top_Down_shooter.Scripts.Components;
//using Top_Down_shooter.Scripts.Controllers;
//using Top_Down_shooter.Scripts.Source;
//using System.Reflection;

//namespace Top_Down_shooter.Scripts.Tests
//{
//    [TestFixture]
//    class NavMeshTests
//    {
//        private Map map;
//        private NavMeshAgent mesh;
//        private readonly int tileSize = GameSettings.TileSize;

//        public NavMeshTests()
//        {
//            map = new Map();
//            mesh = new NavMeshAgent(map);
//        }

//        [TestCase(0, 0)]
//        [TestCase(12, 63)]
//        [TestCase(63, 12)]
//        [TestCase(12, 64)]
//        [TestCase(63, 63)]
//        [TestCase(64, 64)]
//        [TestCase(0, 64)]
//        [TestCase(0, 65)]
//        [TestCase(125, 19)]
//        public void GetPositionInNavMesh_ReturnsCorrectTile(int xPoint, int yPoint)
//        {
//            var actual = (Point?)mesh
//                .GetType()
//                .GetMethod("GetPositionInNavMesh", BindingFlags.NonPublic | BindingFlags.Instance)
//                .Invoke(mesh, new object[] { new Point(xPoint, yPoint) });

//            Assert.IsNotNull(actual);
//            Assert.IsTrue(map.Tiles[actual.Value.X, actual.Value.Y].Collider.Transform.Contains(xPoint, yPoint));
//        }
//    }
//}
