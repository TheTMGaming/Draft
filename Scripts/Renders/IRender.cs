using System.Drawing;
using Top_Down_shooter.Scripts.Source;

namespace Top_Down_shooter.Scripts.Renders
{
    interface IRender
    {
        int X { get; }
        int Y { get; }

        Size Size { get; }

        void Draw(D2DGraphicsDevice device);
    }
}
