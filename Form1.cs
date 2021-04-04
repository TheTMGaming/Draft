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
        

        public Form1()
        {
            
            
            
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            var image = new Bitmap(@"Sprites\player.png");

            g.DrawImage(image, 0, 0);
        }
    }
}
