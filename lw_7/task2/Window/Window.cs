using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace task2;

public class Window : GameWindow
    {
        private int _shaderProgram;
        private int _vao;
        private int _vbo;
        private Matrix4 _projectionMatrix;

        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
            _shaderProgram = LoadShaders("vertex.glsl", "fragment.glsl");
    
            float flagWidth = 3.0f;
            float flagHeight = 2.0f;

            float[] vertices = {
                -flagWidth / 2f, -flagHeight / 2f,
                flagWidth / 2f, -flagHeight / 2f,
                -flagWidth / 2f,  flagHeight / 2f,
                flagWidth / 2f,  flagHeight / 2f
            };

            _vao = GL.GenVertexArray();
            GL.BindVertexArray(_vao);

            _vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
    
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
            
            GL.CompileShader(fragmentShader);
            
            int program = GL.CreateProgram();
            GL.AttachShader(program, vertexShader);
            GL.AttachShader(program, fragmentShader);
            GL.LinkProgram(program);
            
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);

            return program;
        }
        
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.UseProgram(_shaderProgram);
    
            int projLoc = GL.GetUniformLocation(_shaderProgram, "projectionMatrix");
            GL.UniformMatrix4(projLoc, false, ref _projectionMatrix);
            
            GL.BindVertexArray(_vao); 
            GL.DrawArrays(PrimitiveType.TriangleStrip, 0, 4);
            GL.BindVertexArray(0);

            SwapBuffers();
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

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
            SetupProjectionMatrix(e.Width, e.Height);
        }
    }