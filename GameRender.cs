using System.Collections.Generic;
using System.Drawing;
using Top_Down_shooter.Scripts.Renders;

namespace Top_Down_shooter
{
    class GameRender
    {
        public readonly List<IRender> gameObjects;
        public readonly IRender player;

        public GameRender(GameModel gameModel)
        {
            gameObjects = new List<IRender>();

            player = new CharacterRender(gameModel.Player, new Bitmap("Sprites/Player.png"), 4, 2);
            gameObjects.Add(player);

            gameObjects.Add(new GunRender(gameModel.Player.Gun, new Bitmap("Sprites/Gun.png")));

            
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
