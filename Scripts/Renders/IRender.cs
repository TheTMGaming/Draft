using System.Drawing;
using Top_Down_shooter.Scripts.Source;

namespace Top_Down_shooter.Scripts.Renders
{
    interface IRender
    {
        int X { get; set; }
        int Y { get; set; }
        Size Size { get; }

        void Draw(D2DGraphicsDevice g);
    }
}
