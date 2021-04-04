using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Top_Down_shooter
{
    enum MoveTo
    {
        Left, Right, Up, Down
    }

    class GameModel
    {
        private Player player;

        public int PlayerX => player.X;
        public int PlayerY => player.Y;
        public Direction DirectionPlayer => player.Direction;
        public Size ScalePlayer => player.Scale;
        public Bitmap AtlasAnimationsPlayer => player.AtlasAnimations;

        public GameModel()
        {
            player = new Player();
        }

        public void MovePlayerTo(MoveTo move)
        {
            switch (move)
            {
                case MoveTo.Up:
                    player.Y -= player.Speed;
                    break;
                case MoveTo.Down:
                    player.Y += player.Speed;
                    break;
                case MoveTo.Right:
                    player.Direction = Direction.Right;
                    player.X += player.Speed;
                    break;
                case MoveTo.Left:
                    player.Direction = Direction.Left;
                    player.X -= player.Speed;
                    break;
            }
        }
         
    }
}
