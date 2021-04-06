using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Top_Down_shooter
{
    class GameModel
    {
        private Player player;

        public Bitmap AtlasAnimationsPlayer => player.AtlasAnimations;
        public int PlayerX => player.X;
        public int PlayerY => player.Y;
        public Size ScalePlayer => player.Scale;
        public DirectionX DirectionXPlayer => player.DirectionX;
        public DirectionY DirectionYPlayer => player.DirectionY;
        public Sight SightPlayer => player.Sight;

        public GameModel()
        {
            player = new Player();
        }

        public void MovePlayer()
        {
            player.X += player.Speed * (int)player.DirectionX;

            player.Y += player.Speed * (int)player.DirectionY;
        }

        public void ChangeDirectionPlayer(DirectionX directionX)
        {
            player.DirectionX = directionX;
            if (player.DirectionX != DirectionX.Idle)
                player.Sight = directionX == DirectionX.Left ? Sight.Left : Sight.Right;
        }
        
        public void ChangeDirectionPlayer(DirectionY directionY) => player.DirectionY = directionY;

    }
}
