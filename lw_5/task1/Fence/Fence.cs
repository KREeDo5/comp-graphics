using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Text;
using System.Threading.Tasks;

namespace task1
{
    public class Fence
    {
        public Vector3 Position { get; set; }
        public float Scale { get; set; } = 1.0f;
        private int[] _textures;

        private readonly float fenceHeight = 1.5f;
        private readonly float fenceThickness = 0.2f;
        private readonly float fenceLength = 20f;
        private readonly float fenceDepth = 20f;

        public Fence(int[] textures)
        {
            _textures = textures;
        }

        public void Draw()
        {
            GL.PushMatrix();
            GL.Translate(Position);
            GL.Scale(Scale, Scale, Scale);

            DrawFenceSide(new Vector3(0, fenceHeight / 2 + 0.01f, -fenceDepth / 2), new Vector3(fenceLength, fenceHeight, fenceThickness)); // levaya
            DrawFenceSide(new Vector3(0, fenceHeight / 2 + 0.01f, fenceDepth / 2), new Vector3(fenceLength, fenceHeight, fenceThickness)); // pravaya
            DrawFenceSide(new Vector3(-fenceLength / 2, fenceHeight / 2 + 0.01f, 0), new Vector3(fenceThickness, fenceHeight, fenceDepth)); // perdnyaya
            DrawFenceSide(new Vector3(fenceLength / 2, fenceHeight / 2 + 0.01f, 0), new Vector3(fenceThickness, fenceHeight, fenceDepth)); // zadnyya

            DrawHelper.DrawRectangularCuboid(
                   new Vector3(-10f,
                   0.77f,
                   2.5f),
                   new Vector3(0.3f, 1.53f, 1.0f),
                   _textures[8],
                   1.0f);

            DrawHelper.DrawRectangularCuboid(
                   new Vector3(-10f,
                   0.77f,
                   8f),
                   new Vector3(0.3f, 1.53f, 3.0f),
                   _textures[9],
                   1.0f);

            GL.PopMatrix();
        }

        private void DrawFenceSide(Vector3 position, Vector3 size)
        {
            DrawHelper.DrawRectangularCuboid(position, size, _textures[7], 1.0f);
        }
    }
}
