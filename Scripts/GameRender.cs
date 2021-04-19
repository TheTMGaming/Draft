using System.Collections.Generic;
using System.Drawing;

namespace Top_Down_shooter
{
    class GameRender
    {
        private readonly List<IRender> gameObjects;

        public GameRender(Character player)
        {
            gameObjects = new List<IRender>();

            var playerImg = new Bitmap("Spirtes/Player.png");
            gameObjects.Add(new CharacterRender(player, 
                new Sprite(player.X - playerImg.Width / 4, player.Y - playerImg.Height / 8, 0, playerImg)));
            
        }

        public void DrawObjects(Graphics g)
        {
            foreach (var obj in gameObjects)
            {

            }
        }
    }
}
