using System.Drawing;

namespace Top_Down_shooter.Scripts.Renders
{
    interface IRender
    {
        void Draw(Graphics g);
    }

    interface IAnimationRender
    {
        void Draw(Graphics g, Point startSlice, Size sizeSlice);
    }
}
