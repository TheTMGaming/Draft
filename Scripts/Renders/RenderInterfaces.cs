using System.Drawing;

namespace Top_Down_shooter.Scripts.Renders
{
    interface IRender
    {
        Size Size { get; }

        void Draw(Graphics g);
    }

    interface IAnimationRender : IRender
    {
        void Draw(Graphics g, Point startSlice, Size sizeSlice);
    }
}
