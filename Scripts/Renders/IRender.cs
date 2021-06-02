using System.Drawing;
using unvell.D2DLib;

namespace Top_Down_shooter.Scripts.Renders
{
    interface IRender
    {
        int X { get; set; }
        int Y { get; set; }
        Size Size { get; }

        void Draw(D2DGraphics g);
    }
}
