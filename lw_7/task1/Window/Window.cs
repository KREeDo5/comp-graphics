using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace task1 
{
    internal class Window : GameWindow
    {
        private int _shaderProgram;
        private int _vao;
        private int _vbo;
        private int _vertexCount;
        private Matrix4 _projectionMatrix;
        private Matrix4 _modelViewMatrix;

        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {

        }

        protected override void OnLoad()
        {
            base.OnLoad();
    
            GL.ClearColor(1.0f, 1.0f, 1.0f, 1.0f);
            GL.LineWidth(4.0f);
    
            _shaderProgram = LoadShaders("vertex.glsl", "fragment.glsl");
    
            InitializeKannabola();
            _modelViewMatrix = Matrix4.Identity;
            SetupProjectionMatrix(Size.X, Size.Y);
        }
        
        private int LoadShaders(string vertexPath, string fragmentPath)
        {
            string vertexShaderSource = File.ReadAllText(vertexPath);
            string fragmentShaderSource = File.ReadAllText(fragmentPath);
            
            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, vertexShaderSource);
            GL.CompileShader(vertexShader);

            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, fragmentShaderSource);
            GL.CompileShader(fragmentShader);
            
            int program = GL.CreateProgram();
            GL.AttachShader(program, vertexShader);
            GL.AttachShader(program, fragmentShader);
            GL.LinkProgram(program);
            
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);

            return program; 
        }
        

        private void InitializeKannabola()
        {
            List<float> points = new List<float>();
            for (float x = 0; x < 2 * MathF.PI; x += MathF.PI / 500f)
            {
                points.Add(x);
            }
            _vertexCount = points.Count;
            
            _vao = GL.GenVertexArray();
            GL.BindVertexArray(_vao);

            _vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, points.Count * sizeof(float), points.ToArray(), BufferUsageHint.StaticDraw);

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 1, VertexAttribPointerType.Float, false, 0, 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
        }
        
        private void SetupProjectionMatrix(int width, int height)
        {
            float diff = (float)width / height;
            float size = 2.5f;
            
            if (diff > 1.0f)
            {
                _projectionMatrix = Matrix4.CreateOrthographicOffCenter(
                    -size * diff, size * diff,
                    -size, size,
                    -1f, 1f);
            }
            else
            {
                _projectionMatrix = Matrix4.CreateOrthographicOffCenter(
                    -size, size,
                    -size / diff, size / diff,
                    -1f, 1f);
            }
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit);

            
            
            GL.UseProgram(_shaderProgram);
            
            int modelViewLoc = GL.GetUniformLocation(_shaderProgram, "modelViewMatrix");
            int projLoc = GL.GetUniformLocation(_shaderProgram, "projectionMatrix");

            GL.UniformMatrix4(modelViewLoc, false, ref _modelViewMatrix);
            GL.UniformMatrix4(projLoc, false, ref _projectionMatrix);
            
            GL.BindVertexArray(_vao);
            GL.DrawArrays(PrimitiveType.LineLoop, 0, _vertexCount);
            GL.BindVertexArray(0);

            SwapBuffers();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
            SetupProjectionMatrix(e.Width, e.Height);
        }
    }
}