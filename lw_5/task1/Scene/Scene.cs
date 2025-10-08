using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace task1
{
    public class Scene
    {
        public Vector3 Position { get; set; }
        public float Size { get; set; }

        private readonly Cottage cottage;
        private readonly Garage garage;
        private readonly Fence fence;

        public Scene(Vector3 position, float size, int[] textures)
        {
            Position = position;
            Size = size;

            cottage = new Cottage(textures)
            {
                Position = new Vector3(0, 0, 0),
                Scale = size
            };

            garage = new Garage(textures)
            {
                Position = new Vector3(5, 0, 37),
                Scale = size
            };

            fence = new Fence(textures)
            {
                Position = new Vector3(0, 0, 0),
                Scale = size
            };
        }

        public void Draw()
        {
            GL.PushMatrix();
            GL.Translate(Position);

            cottage.Draw();
            garage.Draw();
            fence.Draw();

            GL.PopMatrix();
        }
    }
}