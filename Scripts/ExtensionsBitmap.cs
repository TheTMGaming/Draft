using System.Drawing;

namespace Top_Down_shooter.Scripts
{
    static class ExtensionsBitmap
    {
        public static Bitmap Extract(this Bitmap source, Rectangle section)
        {
            Bitmap bmp = new Bitmap(section.Width, section.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.DrawImage(source, 0, 0, section, GraphicsUnit.Pixel);
            }
            return bmp;
        }
    }
}
