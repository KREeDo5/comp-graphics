using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.IO;

namespace task3_elParabaloid
{
    public class Window : GameWindow
    {
        private int _program;
        private int _vao;

        private Camera _camera;
        private Matrix4 _projection;

        private string _vertexShaderPath = "vertex.glsl";
        private string _fragmentShaderPath = "fragment.glsl";

        private Vector2 _lastMousePos;
        private bool _firstMouse = true;

        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0.2f, 0.3f, 0.4f, 1f);
            GL.Enable(EnableCap.DepthTest);

            _camera = new Camera();

            string vertexSource = File.ReadAllText(_vertexShaderPath);
            string fragmentSource = File.ReadAllText(_fragmentShaderPath);

            _program = CreateProgram(vertexSource, fragmentSource);

            _vao = GL.GenVertexArray();
            GL.BindVertexArray(_vao);
        }

        private void SetupProjectionMatrix(int width, int height)
        {
            float diff = (float)width / height;
            float size = 1.25f;
            
            if (diff > 1.0f)
            {
                _projection = Matrix4.CreateOrthographicOffCenter(
                    -size * diff, size * diff,
                    -size, size,
                    -1f, 1f);
            }
            else
            {
                _projection = Matrix4.CreateOrthographicOffCenter(
                    -size, size,
                    -size / diff, size / diff,
                    -1f, 1f);
            }
        }


        protected override void OnResize(OpenTK.Windowing.Common.ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Size.X, Size.Y);
            SetupProjectionMatrix(e.Height, e.Width);
        }

        protected override void OnUpdateFrame(OpenTK.Windowing.Common.FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            if (KeyboardState.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Escape))
                Close();

            Vector2 mouse = MouseState.Position;

            if (_firstMouse)
            {
                _lastMousePos = mouse;
                _firstMouse = false;
            }
            
            if (MouseState.IsButtonDown(OpenTK.Windowing.GraphicsLibraryFramework.MouseButton.Left))
            {
                Vector2 delta = mouse - _lastMousePos;
                _camera.Rotate(delta.X, delta.Y);
            }

            _lastMousePos = mouse;
        }

        protected override void OnRenderFrame(OpenTK.Windowing.Common.FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.UseProgram(_program);
            GL.BindVertexArray(_vao);

            Matrix4 view = _camera.ViewMatrix;

            int viewLoc = GL.GetUniformLocation(_program, "view");
            int projLoc = GL.GetUniformLocation(_program, "projection");
            int screenSizeLoc = GL.GetUniformLocation(_program, "screenSize");
            int viewPosLoc = GL.GetUniformLocation(_program, "viewPos");

            GL.UniformMatrix4(viewLoc, false, ref view);
            GL.UniformMatrix4(projLoc, false, ref _projection);
            GL.Uniform2(screenSizeLoc, new Vector2(Size.X, Size.Y));
            GL.Uniform3(viewPosLoc, GetCameraPositionFromView(view));

            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

            SwapBuffers();
        }

        private Vector3 GetCameraPositionFromView(Matrix4 view)
        {
            Matrix4 inverseView = Matrix4.Invert(view);
            return inverseView.Row3.Xyz;
        }

        private static int CompileShader(ShaderType type, string source)
        {
            int shader = GL.CreateShader(type);
            GL.ShaderSource(shader, source);
            GL.CompileShader(shader);

            GL.GetShader(shader, ShaderParameter.CompileStatus, out int success);

            return shader;
        }

        private static int CreateProgram(string vertexSource, string fragmentSource)
        {
            int vertexShader = CompileShader(ShaderType.VertexShader, vertexSource);
            int fragmentShader = CompileShader(ShaderType.FragmentShader, fragmentSource);

            int program = GL.CreateProgram();
            GL.AttachShader(program, vertexShader);
            GL.AttachShader(program, fragmentShader);
            GL.LinkProgram(program);

            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out int success);

            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);

            return program;
        }
    }
}
