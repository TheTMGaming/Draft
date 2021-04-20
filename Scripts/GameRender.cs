using System.Collections.Generic;
using System.Drawing;

namespace Top_Down_shooter
{
    class GameRender
    {
        private readonly List<IRender> gameObjects;

        public GameRender(GameModel gameModel)
        {
            gameObjects = new List<IRender>();

            var playerImg = new Bitmap("Sprites/Player.png");
            gameObjects.Add(new CharacterRender(gameModel.Player, playerImg, 4, 2));

            var gunImg = new Bitmap("Sprites/Gun.png");
            gameObjects.Add(new GunRender(gameModel.Player.Gun, gunImg));
            
        }

        public void DrawObjects(Graphics g)
        {
            foreach (var obj in gameObjects)
            {
                obj.Draw(g);
            }
        }

        public void PlayAnimations()
        {
            foreach (var obj in gameObjects)
            {
                if (obj is CharacterRender character)
                {
                    character.ChangeTypeAnimation();
                    character.PlayAnimation();
                }
            }
        }
    }
}
