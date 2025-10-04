using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;


namespace task1
{
    public class Window : GameWindow
    {

        private Chess _chess;
        private Camera _camera = new Camera();

        private bool _leftButtonPressed;
        private Vector2 _lastMousePos;

        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {

        }

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(Color4.White);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Light0);
            GL.Enable(EnableCap.ColorMaterial);
            GL.Enable(EnableCap.Normalize);

            GL.ColorMaterial(MaterialFace.Front, ColorMaterialParameter.AmbientAndDiffuse);
            
            _chess = new Chess();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            int width = e.Width;
            int height = e.Height;

            GL.Viewport(0, 0, width, height);

            SetupProjectionMatrix(width, height);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            base.OnResize(e);
        }

        void SetupProjectionMatrix(int width, int height)
        {
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            
            float aspectRatio = (float)width / height;

            Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4,
                aspectRatio,        
                0.1f,              
                1000f               
            );

            GL.LoadMatrix(ref perspective);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.MatrixMode(MatrixMode.Modelview);
            Matrix4 view = _camera.ViewMatrix;
            GL.LoadMatrix(ref view);

            _chess.Draw();

            Vector4 lightPosition = new Vector4(-80f, 50f, 100f, 1.0f);
            GL.Light(LightName.Light0, LightParameter.Position, lightPosition);

            Vector4 lightDiffuse = new Vector4(1f, 1f, 1f, 1.0f);
            Vector4 lightAmbient = new Vector4(0.2f, 0.2f, 0.2f, 1.0f);
            Vector4 lightSpecular = new Vector4(1f, 1f, 1f, 1.0f);

            GL.Light(LightName.Light0, LightParameter.Diffuse, lightDiffuse);
            GL.Light(LightName.Light0, LightParameter.Ambient, lightAmbient);
            GL.Light(LightName.Light0, LightParameter.Specular, lightSpecular);

            SwapBuffers();
            base.OnRenderFrame(args);
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            if (KeyboardState.IsKeyPressed(Keys.Space) && !_chess._isAnimating)
            {
                _chess._isAnimating = true;
                _chess._currentMoveIndex = 0;
                Console.WriteLine("Start animating");
            }

            if (_chess._isAnimating)
            {
                _chess.NextStep(); 

                if (_chess._currentMoveIndex >= _chess._moves.Count)
                {
                    _chess._isAnimating = false;
                    Console.WriteLine("Animation finished");
                }
            }
        }


        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            if (e.Button == MouseButton.Left)
            {
                _leftButtonPressed = true;
                _lastMousePos = MousePosition;
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            if (e.Button == MouseButton.Left)
            {
                _leftButtonPressed = false;
            }
            base.OnMouseUp(e);
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            if (_leftButtonPressed)
            {
                Vector2 delta = new Vector2(e.X - _lastMousePos.X, e.Y - _lastMousePos.Y);
                _lastMousePos = new Vector2(e.X, e.Y);

                _camera.Rotate(delta.X, delta.Y);
            }
            base.OnMouseMove(e);
        }
    }
}
