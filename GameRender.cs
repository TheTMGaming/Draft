using System.Collections.Generic;
using System.Drawing;
using Top_Down_shooter.Scripts.Renders;

namespace Top_Down_shooter
{
    class GameRender
    {
        private readonly List<IRender> gameObjects;
        private readonly TileMapRender mapRender;

        public GameRender(GameModel gameModel)
        {
            gameObjects = new List<IRender>();

            gameObjects.Add(new CharacterRender(gameModel.Player, new Bitmap("Sprites/Player.png"), 4, 2));

            gameObjects.Add(new GunRender(gameModel.Player.Gun, new Bitmap("Sprites/Gun.png")));

            gameObjects.Add(new BulletsRender(gameModel.Bullets, new Bitmap("Sprites/Bullet.png")));

            mapRender = new TileMapRender(gameModel.Map);
            
        }

        public void DrawObjects(Graphics g)
        {
            foreach (var obj in gameObjects)
            {
                obj.Draw(g);
            }
        }

        public void DrawMap(Graphics g)
        {
            mapRender.Draw(g);
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
