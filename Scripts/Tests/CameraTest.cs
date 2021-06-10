using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Top_Down_shooter.Scripts.Components;
using Top_Down_shooter.Scripts.GameObjects;
using Top_Down_shooter.Scripts.Source;

namespace Top_Down_shooter.Scripts.Tests
{
    [TestFixture]
    class CameraTest
    {
        private readonly Player player;
        private readonly Camera camera;

        public CameraTest()
        {
            player = new Player(0, 0);
            camera = new Camera();
        }

        [SetUp]
        public void Init()
        {
            player.X = 0;
            player.Y = 0;
            camera.X = 0;
            camera.Y = 0;
        }

        [TestCase(0, 0)]
        [TestCase(GameSettings.MapWidth / 1000, GameSettings.MapHeight / 1000)]
        [TestCase(GameSettings.MapWidth / 400, GameSettings.MapHeight / 1000)]
        [TestCase(GameSettings.MapWidth / 10, GameSettings.MapHeight / 5)]
        [TestCase(GameSettings.ScreenWidth / 2 - 1, GameSettings.ScreenHeight / 2 - 1)]
        public void CameraCannotMove(int xPlayer, int yPlayer)
        {
            player.X = xPlayer;
            player.Y = yPlayer;

            camera.Move(player);

            Assert.IsTrue(camera.X == 0 && camera.Y == 0);
        }

        [TestCase(GameSettings.ScreenWidth + 1, 0)]
        [TestCase(GameSettings.MapWidth / 2, 0)]
        [TestCase(GameSettings.MapWidth - GameSettings.ScreenWidth / 2 - 1, 0)]
        [TestCase(GameSettings.MapWidth / 2 + 10, 0)]
        [TestCase(GameSettings.MapWidth - GameSettings.ScreenWidth / 2 - 1, GameSettings.ScreenWidth / 4)]
        public void CameraMustMoveX(int xPlayer, int yPlayer)
        {
            player.X = xPlayer;
            player.Y = yPlayer;

            camera.Move(player);

            Assert.IsTrue(camera.X == player.X - GameSettings.ScreenWidth / 2);
        }

        [TestCase(0, GameSettings.ScreenHeight)]
        [TestCase(0, GameSettings.MapHeight / 2)]
        [TestCase(0, GameSettings.MapHeight - GameSettings.ScreenHeight / 2 - 1)]
        [TestCase(0, GameSettings.MapHeight / 2 + 10)]
        [TestCase(GameSettings.ScreenWidth / 4, GameSettings.MapHeight - GameSettings.ScreenHeight / 2 - 1)]
        public void CameraMustMoveY(int xPlayer, int yPlayer)
        {
            player.X = xPlayer;
            player.Y = yPlayer;

            camera.Move(player);

            Assert.IsTrue(camera.Y == player.Y - GameSettings.ScreenHeight / 2);
        }


        [TestCase(GameSettings.ScreenWidth / 2, GameSettings.ScreenHeight / 2)]
        [TestCase(GameSettings.ScreenWidth, GameSettings.ScreenHeight)]
        [TestCase(GameSettings.ScreenWidth + 1, GameSettings.ScreenHeight + 1)]
        [TestCase(GameSettings.MapWidth - GameSettings.ScreenWidth / 2 - 1, GameSettings.MapHeight - GameSettings.ScreenHeight / 2 - 1)]
        public void CameraMustMoveXY(int xPlayer, int yPlayer)
        {
            player.X = xPlayer;
            player.Y = yPlayer;

            camera.Move(player);

            Assert.IsTrue(camera.X == player.X - GameSettings.ScreenWidth / 2 
                && camera.Y == player.Y - GameSettings.ScreenHeight / 2);
        }
    }
}
