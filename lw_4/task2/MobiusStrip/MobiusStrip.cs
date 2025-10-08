using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;

namespace task2
{
    public class MobiusStrip
    {
        private Vector3 lowColor { get; set; }
        private Vector3 highColor { get; set; }
        private float size { get; set; }

        public MobiusStrip(float size, Vector3? lowColor = null, Vector3? highColor = null)
        {
            this.size = size;
            this.lowColor = lowColor ?? new Vector3(0f, 0f, 0f);
            this.highColor = highColor ?? new Vector3(1f, 1f, 1f);
        }

        private void GetColorByZ(float z)
        {
            float t = (z + 0.5f) / 1.0f;
            Vector3 color = Vector3.Lerp(lowColor, highColor, t);
            GL.Color3(color.X, color.Y, color.Z);
        }

        private void GetVertex(double u, double v)
        {
            float x = (float)((1 + v / 2 * Math.Cos(u / 2)) * Math.Cos(u));
            float y = (float)((1 + v / 2 * Math.Cos(u / 2)) * Math.Sin(u));
            float z = (float)(v / 2 * Math.Sin(u / 2));

            GetColorByZ(z);
            GL.Vertex3(x, y, z);
        }

        public void Draw()
        {
            GL.PushMatrix();
            GL.Scale(size, size, 1);

            GL.Begin(PrimitiveType.QuadStrip);
            for (double v = -1; v <= 1; v += 0.05)
            {
                for (double u = 0; u < 2 * Math.PI + 0.05; u += 0.05)
                {
                    GetVertex(u, v);
                    GetVertex(u, v + 0.05);
                }
            }
            GL.End();

            GL.PopMatrix();
        }
    }
}
