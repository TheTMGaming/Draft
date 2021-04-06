using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Top_Down_shooter
{
    enum AnimationTypes
    {
        IdleRight,
        IdleLeft,
        RunRight,
        RunLeft
    }

    public class Form1 : Form
    {
        private GameModel gameModel;
        private int currentFrameAnimation;

        public Form1()
        {
            DoubleBuffered = true;

            gameModel = new GameModel();

            var timerGameLoop = new Timer();
            timerGameLoop.Interval = 30;
            timerGameLoop.Tick += new EventHandler(UpdateGameLoop);
            timerGameLoop.Start();

            var timerChangeAnimationFrame = new Timer();
            timerChangeAnimationFrame.Interval = 300;
            timerChangeAnimationFrame.Tick += new EventHandler((sender, args) =>
            {
                currentFrameAnimation = ++currentFrameAnimation % 2;
            });
            timerChangeAnimationFrame.Start();
            
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.DrawImage(
                gameModel.AtlasAnimationsPlayer, 
                gameModel.PlayerX, gameModel.PlayerY, 
                new Rectangle(new Point(gameModel.ScalePlayer.Width * currentFrameAnimation, 
                                        gameModel.ScalePlayer.Height * (int)GetAnimationType(gameModel.DirectionXPlayer, gameModel.DirectionYPlayer, gameModel.SightPlayer)), 
                              gameModel.ScalePlayer),
                GraphicsUnit.Pixel);

        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    gameModel.ChangeDirectionPlayer(DirectionY.Up);
                    break;
                case Keys.A:
                    gameModel.ChangeDirectionPlayer(DirectionX.Left);
                    break;
                case Keys.S:
                    gameModel.ChangeDirectionPlayer(DirectionY.Down);
                    break;
                case Keys.D:
                    gameModel.ChangeDirectionPlayer(DirectionX.Right);
                    break;
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                case Keys.S:
                    gameModel.ChangeDirectionPlayer(DirectionY.Idle);
                    break;
                case Keys.A:
                case Keys.D:
                    gameModel.ChangeDirectionPlayer(DirectionX.Idle);
                    break;
            }
        }

        public void UpdateGameLoop(object sender, EventArgs args)
        {
            gameModel.MovePlayer();
            Invalidate();
        }

        private AnimationTypes GetAnimationType(DirectionX directionX, DirectionY directionY, Sight sight)
        {
            if (directionX == DirectionX.Idle && directionY == DirectionY.Idle)
                return sight == Sight.Left ? AnimationTypes.IdleLeft : AnimationTypes.IdleRight;

            return sight == Sight.Left ? AnimationTypes.RunLeft : AnimationTypes.RunRight;
        }
    }
}
