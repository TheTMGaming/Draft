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
    public class Form1 : Form
    {
        private GameModel gameModel;
        private int currentFrame;

        public Form1()
        {
            DoubleBuffered = true;

            gameModel = new GameModel();

            var timerGameLoop = new Timer();
            timerGameLoop.Interval = 60;
            timerGameLoop.Tick += new EventHandler(UpdateGameLoop);
            timerGameLoop.Start();

            var timerChangeAnimationFrame = new Timer();
            timerChangeAnimationFrame.Interval = 300;
            timerChangeAnimationFrame.Tick += new EventHandler((sender, args) =>
            {
                currentFrame = ++currentFrame % 2;
            });
            timerChangeAnimationFrame.Start();
            
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.DrawImage(
                gameModel.AtlasAnimationsPlayer, 
                gameModel.PlayerX, gameModel.PlayerY, 
                new Rectangle(new Point(gameModel.ScalePlayer.Width * currentFrame, 
                                        gameModel.ScalePlayer.Height * (int)gameModel.DirectionPlayer), 
                                        gameModel.ScalePlayer),
                GraphicsUnit.Pixel);

        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.D:
                    gameModel.MovePlayerTo(MoveTo.Right);
                    break;
                case Keys.A:
                    gameModel.MovePlayerTo(MoveTo.Left);
                    break;
                case Keys.W:
                    gameModel.MovePlayerTo(MoveTo.Up);
                    break;
                case Keys.S:
                    gameModel.MovePlayerTo(MoveTo.Down);
                    break;
            }
        }

        public void UpdateGameLoop(object sender, EventArgs args)
        {
            Invalidate();
        }


    }
}
