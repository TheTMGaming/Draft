using System.Drawing;
using System.Windows.Forms;
using unvell.D2DLib;

namespace Top_Down_shooter.Scripts.Source
{
    class D2DGraphicsDevice
    {
        public D2DGraphics Graphics { get; }

        private D2DDevice D2DDevice { get; }

        private Form Form { get; }

        public D2DGraphicsDevice(Form form)
        {
            Form = form;
            D2DDevice = D2DDevice.FromHwnd(Form.Handle);
            D2DDevice.Resize();
            Form.Resize += (sender, args) => D2DDevice.Resize();
            Form.HandleDestroyed += (sender, args) => D2DDevice.Dispose();
            Graphics = new D2DGraphics(D2DDevice);
            Graphics.SetDPI(96, 96);
        }

        public D2DBitmap CreateBitmap(Bitmap bitmap)
        {
            return D2DDevice.CreateBitmapFromGDIBitmap(bitmap);
        }

        public void BeginRender()
        {
            Graphics.BeginRender(D2DColor.FromGDIColor(Form.BackColor));
        }

        public void EndRender()
        {
            Graphics.EndRender();
        }

        public void DrawBitmap(D2DBitmap bitmap, Point location, Size scale, float opacity)
        {
            var rect = new D2DRect(location, scale);
            Graphics.DrawBitmap(bitmap, rect);
        }
    }
}
