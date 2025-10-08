using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Input;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace task3
{
    public class Window : GameWindow
    {
        private bool _leftButtonPressed = false;
        private Vector2 _lastMousePos;

        private int _shaderProgram;
        private int _vao;
        private int _vbo;
        private Matrix4 _projectionMatrix;
        private Matrix4 _modelMatrix;
        private Camera _camera;  
        private float _t = 0f;
        private bool _forward = true;
        private int _vertexCount;

        private float _elapsedTime = 0f; 
        private float _delayTime = 1.5f; 
        private bool _transformationCompleted = false;  

        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
            _camera = new Camera();
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            
            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
            GL.Enable(EnableCap.DepthTest);

            _shaderProgram = LoadShaders("vertex.glsl", "fragment.glsl");

            List<float> vertices = new();
            int N = 50;

            for (int i = 0; i < N; i++)
            {
                float u0 = (float)i / N;
                float u1 = (float)(i + 1) / N;
                for (int j = 0; j < N; j++)
                {
                    float v0 = (float)j / N;
                    float v1 = (float)(j + 1) / N;
                    
                    vertices.AddRange(new[] { u0, v0, u1, v0, u1, v1 });
                    vertices.AddRange(new[] { u0, v0, u1, v1, u0, v1 });
                }
            }
            
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            
            _vertexCount = vertices.Count / 2;

            _vao = GL.GenVertexArray();
            _vbo = GL.GenBuffer();

            GL.BindVertexArray(_vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Count * sizeof(float), vertices.ToArray(), BufferUsageHint.StaticDraw);

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            
            _modelMatrix = Matrix4.Identity;
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            if (_transformationCompleted)
            {
                _elapsedTime += (float)e.Time;
                if (_elapsedTime >= _delayTime)
                {
                    _transformationCompleted = false;
                    _elapsedTime = 0f; 
                    _forward = !_forward;
                }
            }

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.UseProgram(_shaderProgram);
            
            Matrix4 viewMatrix = _camera.ViewMatrix;

            GL.UniformMatrix4(GL.GetUniformLocation(_shaderProgram, "model"), false, ref _modelMatrix);
            GL.UniformMatrix4(GL.GetUniformLocation(_shaderProgram, "view"), false, ref viewMatrix);
            GL.UniformMatrix4(GL.GetUniformLocation(_shaderProgram, "projection"), false, ref _projectionMatrix);
            GL.Uniform1(GL.GetUniformLocation(_shaderProgram, "t"), _t);

            GL.BindVertexArray(_vao);
            GL.DrawArrays(PrimitiveType.QuadStrip, 0, _vertexCount);
            GL.BindVertexArray(0);
            
            if (!_transformationCompleted)
            {
                _t += (float)e.Time * (_forward ? 0.3f : -0.3f);
                if (_t >= 1.0f)
                {
                    _t = 1.0f;
                    _transformationCompleted = true; 
                }
                if (_t <= 0.0f)
                {
                    _t = 0.0f;
                    _transformationCompleted = true; 
                }
            }

            SwapBuffers();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
            SetupProjectionMatrix(e.Width, e.Height);
        }

        private void SetupProjectionMatrix(int width, int height)
        {
            float aspect = (float)width / height;
            _projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45), aspect, 0.1f, 100f);
        }

        private int LoadShaders(string vertexPath, string fragmentPath)
        {
            string vertexShaderSource = File.ReadAllText(vertexPath);
            string fragmentShaderSource = File.ReadAllText(fragmentPath);

            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, vertexShaderSource);
            GL.CompileShader(vertexShader);
            CheckShaderCompileStatus(vertexShader);
            
            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, fragmentShaderSource);
            GL.CompileShader(fragmentShader);
            CheckShaderCompileStatus(fragmentShader);
            
            int program = GL.CreateProgram();
            GL.AttachShader(program, vertexShader);
            GL.AttachShader(program, fragmentShader);
            GL.LinkProgram(program);
            CheckProgramLinkStatus(program);
            
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);

            return program;
        }

        void CheckShaderCompileStatus(int shader)
        {
            GL.GetShader(shader, ShaderParameter.CompileStatus, out int success);
            if (success == 0)
            {
                string infoLog = GL.GetShaderInfoLog(shader);
                Console.WriteLine("Shader compilation failed:\n" + infoLog);
            }
        }

        void CheckProgramLinkStatus(int program)
        {
            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out int success);
            if (success == 0)
            {
                string infoLog = GL.GetProgramInfoLog(program);
                Console.WriteLine("Program linking failed:\n" + infoLog);
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
