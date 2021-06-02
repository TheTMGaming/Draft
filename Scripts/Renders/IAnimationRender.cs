using System.Drawing;
using unvell.D2DLib;

namespace Top_Down_shooter.Scripts.Renders
{
    interface IAnimationRender : IRender
    {
        void Draw(D2DGraphics g, Point startSlice, Size sizeSlice);

        void PlayAnimation();

        void ChangeTypeAnimation();
    }
}
