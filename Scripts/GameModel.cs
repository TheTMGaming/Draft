using System;
using System.Collections.Generic;
using System.Drawing;


namespace Top_Down_shooter
{
    class GameModel
    {
        public readonly LinkedList<Sprite> GameSprites = new LinkedList<Sprite>();
        public readonly LinkedList<IUserInterface> UI = new LinkedList<IUserInterface>();

        public readonly Player Player;
        private readonly HealthBar HealthBar;

        public GameModel()
        {
            Player = new Player(100, 100, 5, new Bitmap(@"Sprites\Player.png"), 4, 2);
            HealthBar = new HealthBar(50, 680, 100);
           
            GameSprites.AddLast(Player);
            GameSprites.AddLast(Player.Gun);
            UI.AddLast(HealthBar);
        }

        public void Shoot()
        {
            var newSpawn = RotatePoint(Player.Gun.SpawnBullets, Player.Gun.Angle);
            
            GameSprites.AddLast(new Bullet(
                Player.Gun.X + newSpawn.X, Player.Gun.Y + newSpawn.Y, 
                20, Player.Gun.Angle, 
                new Bitmap(@"Sprites/Bullet.png")));
        }

        public void PlayAnimations()
        {
            foreach (var sprite in GameSprites)
            {
                if (sprite is AnimationSprite animationSprite)
                    animationSprite.PlayAnimation();
            }
        }

        private Point RotatePoint(Point point, float angleInRadian)
        {
            return new Point(
                (int)(point.X * Math.Cos(angleInRadian) - point.Y * Math.Sin(angleInRadian)),
                (int)(point.Y * Math.Cos(angleInRadian) + point.X * Math.Sin(angleInRadian))
                );
        }
    }
}
