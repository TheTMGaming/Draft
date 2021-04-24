using System.Drawing;

namespace Top_Down_shooter.Scripts.Renders
{
    interface IRender
    {
        int X { get; set; }
        int Y { get; set; }
        Size Size { get; }

        void Draw(Graphics g);
    }
}
