using System.Drawing;
using Top_Down_shooter.Scripts.GameObjects;
using Top_Down_shooter.Scripts.Source;

namespace Top_Down_shooter.Scripts.Renders
{
    interface IRender
    {
        GameObject Parent { get; }
        int X { get; }
        int Y { get; }

        Size Size { get; }

        void Draw(D2DGraphicsDevice device);
    }
}
