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

        public Point PositionPlayer => player.Position;
        public Direction DirectionPlayer => player.Direction;
        public Size ScalePlayer => player.Scale;
        public Bitmap AtlasAnimationsPlayer => player.AtlasAnimations;

        public GameModel()
        {
            player = new Player();
        }

         
    }
}
