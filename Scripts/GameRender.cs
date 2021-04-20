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
            gameObjects.Add(new CharacterRender(gameModel.Player, 
                new Sprite(gameModel.Player.X - playerImg.Width / 4, gameModel.Player.Y - playerImg.Height / 8, 0, playerImg),
                4, 2));

            var gunImg = new Bitmap("Sprites/Gun.png");
            gameObjects.Add(new GunRender(gameModel.Player.Gun,
                new Sprite(gameModel.Player.Gun.X - gunImg.Width / 4, gameModel.Player.Gun.Y - gunImg.Height / 8, 0, gunImg)));
            
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


        //private void DrawSprite(Graphics g, Sprite sprite)
        //{
        //    g.TranslateTransform(sprite.X, sprite.Y);
        //    g.RotateTransform((float)(sprite.Angle * 180 / Math.PI));
        //    g.TranslateTransform(-sprite.X, -sprite.Y);

        //    sprite.Draw(g);

        //    g.ResetTransform();
        //}

    }
}
