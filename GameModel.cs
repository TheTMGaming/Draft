using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Top_Down_shooter
{
    class GameModel
    {
        private readonly Player Player;
        private readonly HealthBar HealthBar;

        public readonly LinkedList<Sprite> GameSprites = new LinkedList<Sprite>();
        public readonly LinkedList<IUserInterface> UI = new LinkedList<IUserInterface>();

        public GameModel()
        {
            Player = new Player(100, 100, 5, new Bitmap(@"Sprites\Player.png"), 4, 2);
            HealthBar = new HealthBar(140, 140, 100);

            GameSprites.AddLast(Player);
            UI.AddLast(HealthBar);
        }

        public void Shoot()
        {
            var newX = Player.Gun.X + (int)(Player.Gun.SpawnBullets.X * Math.Cos(Player.Gun.Angle) - Player.Gun.SpawnBullets.Y * Math.Sin(Player.Gun.Angle));
            var newY = Player.Gun.Y + (int)(Player.Gun.SpawnBullets.Y * Math.Cos(Player.Gun.Angle) + Player.Gun.SpawnBullets.X * Math.Sin(Player.Gun.Angle));
            GameSprites.AddLast(new Bullet(newX, newY, 20, Player.Gun.Angle, new Bitmap(@"Sprites/Bullet.png")));
        }

        public void ChangeDirection(DirectionX directionX) => Player.ChangeDirection(directionX);

        public void ChangeDirectionPlayer(DirectionY directionY) => Player.ChangeDirection(DirectionY);
    }
}
