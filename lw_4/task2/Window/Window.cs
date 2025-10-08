using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Drawing;
using Task2;

namespace task2
{
    public class Window : GameWindow
    {
        private bool _leftButtonPressed = false;
        private Vector2 _lastMousePos;

        private readonly MobiusStrip _strip;
        private readonly Camera _camera = new Camera();

        public Window(int width, int height, string title)
            : base(width, height, GraphicsMode.Default, title)
        {
            _strip = new MobiusStrip(1.0f, new Vector3(0.0f, 0.0f, 1.0f), new Vector3(1.0f, 1.0f, 0.0f));
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            GL.ClearColor(0.1f, 0.1f, 0.1f, 1.0f);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Width, Height);

            float aspectRatio = Width / (float)Height;
            float orthoSize = 5.0f; 

            Matrix4 projection;

            if (Width >= Height)
            {
                projection = Matrix4.CreateOrthographic(orthoSize * aspectRatio, orthoSize, 0.1f, 100f);
            }
            else
            {
                projection = Matrix4.CreateOrthographic(orthoSize, orthoSize / aspectRatio, 0.1f, 100f);
            }

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projection);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButton.Left)
            {
                _leftButtonPressed = true;
                _lastMousePos = new Vector2(e.X, e.Y);
            }
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            if (e.Button == MouseButton.Left)
            {
                _leftButtonPressed = false;
            }
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            base.OnMouseMove(e);
            if (_leftButtonPressed)
            {
                Vector2 delta = new Vector2(e.X - _lastMousePos.X, e.Y - _lastMousePos.Y);
                _lastMousePos = new Vector2(e.X, e.Y);

                _camera.Rotate(delta.X, delta.Y);
            }
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Matrix4 view = _camera.ViewMatrix;
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref view);

            _strip.Draw();

            SwapBuffers();
            base.OnRenderFrame(args);
        }
    }
}
