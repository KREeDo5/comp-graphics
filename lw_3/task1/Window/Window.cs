using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;

namespace task1
{
    public class Window : GameWindow
    {
        private Axes _axes;
        private Parabola _parabola;

        public Window(int width, int height, string title)
            : base(width, height, GraphicsMode.Default, title)
        {
            _axes = new Axes(0.5, 1.0);
            _parabola = new Parabola(2.0, -3.0, -8.0, -2.0, 3.0);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            GL.ClearColor(1.0f, 1.0f, 1.0f, 1.0f);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Width, Height);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            float diff = (float)Width / Height;
            if (diff > 1)
            {
                GL.Ortho(-10 * diff, 10 * diff, -10, 10, -1, 1);
            }
            else
            {
                GL.Ortho(-10, 10, -10 / diff, 10 / diff, -1, 1);
            }

            GL.MatrixMode(MatrixMode.Modelview);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            _axes.Draw();

            _parabola.Draw();

            SwapBuffers();
        }
    }
}