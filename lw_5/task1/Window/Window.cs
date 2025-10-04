using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Linq;

namespace task1
{
    public class Window : GameWindow
    {
        private bool _leftButtonPressed = false;
        private Vector2 _lastMousePos;

        private int[] textures;

        private Skybox skybox;
        private Grass grass;

        private Scene scene;
        private Camera camera = new Camera();

        public Window(int width, int height, string title)
            : base(width, height, GraphicsMode.Default, title)
        {

        }

        protected override void OnLoad(EventArgs e)
        {
            GL.ClearColor(0.0f, 0.5f, 1.0f, 1.0f);
            GL.Enable(EnableCap.DepthTest);

            GL.Enable(EnableCap.Lighting);       
            GL.Enable(EnableCap.Light0);         
            GL.Enable(EnableCap.ColorMaterial);    
            GL.ColorMaterial(MaterialFace.Front, ColorMaterialParameter.AmbientAndDiffuse);

            InitTextures();

            skybox = new Skybox(textures[0]);
            grass = new Grass(textures[1]);

            scene = new Scene(
                new Vector3(0.0f, -20.0f, 0.0f),
                5.0f,
                textures);

            GL.MatrixMode(MatrixMode.Projection);
            Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4,
                (float)Width / Height,
                0.1f,
                1000.0f
            );
            GL.LoadMatrix(ref perspective);
        }

        void InitTextures()
        {
            textures = new int[] {
                TextureLoader.LoadTexture("skybox.jpg"),
                TextureLoader.LoadTexture("grass.jpg"),
                TextureLoader.LoadTexture("brick.jpg"),
                TextureLoader.LoadTexture("roof.jpg"),
                TextureLoader.LoadTexture("window.jpg"),
                TextureLoader.LoadTexture("door.jpg"),
                TextureLoader.LoadTexture("garageDoor.png"),
                TextureLoader.LoadTexture("wood.jpg"),
                TextureLoader.LoadTexture("fenceDoor.jpg"),
                TextureLoader.LoadTexture("fenceGarageDoor.jpg"),
            };
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

                camera.Rotate(delta.X, delta.Y);
            }
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Matrix4 view = camera.ViewMatrix;
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref view);

            GL.PushMatrix(); 
            GL.LoadIdentity(); 

            Vector4 lightPositionWorld = new Vector4(20f, 20f, 60f, 1.0f);
            GL.Light(LightName.Light0, LightParameter.Position, lightPositionWorld);

            GL.PopMatrix(); 

            Vector4 lightDiffuse = new Vector4(1f, 1f, 1f, 1.0f);
            Vector4 lightAmbient = new Vector4(0.2f, 0.2f, 0.2f, 1.0f);

            GL.Light(LightName.Light0, LightParameter.Diffuse, lightDiffuse);
            GL.Light(LightName.Light0, LightParameter.Ambient, lightAmbient);

            GL.DepthMask(true);

            skybox.Draw();
            
            grass.Draw();
            scene.Draw();

            SwapBuffers();
            base.OnRenderFrame(args);
        }
    }
}