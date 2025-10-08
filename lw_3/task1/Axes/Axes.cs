using OpenTK.Graphics.OpenGL;

namespace task1
{
    public class Axes
    {
        private double xStep;
        private double yStep;

        public Axes(double xStep, double yStep)
        {
            this.xStep = xStep;
            this.yStep = yStep;
        }

        public void Draw()
        {
            GL.LoadIdentity();
            GL.Color3(0.0, 0.0, 0.0);
            GL.LineWidth(1.0f);

            GL.Begin(PrimitiveType.Lines);
            GL.Vertex2(-10, 0);
            GL.Vertex2(10, 0);
            GL.End();


            GL.Begin(PrimitiveType.Lines);
            GL.Vertex2(0, -10);
            GL.Vertex2(0, 10);
            GL.End();

            for (double x = -10; x <= 10; x += xStep)
            {
                GL.Begin(PrimitiveType.Lines);
                GL.Vertex2(x, -0.1);
                GL.Vertex2(x, 0.1);
                GL.End();
            }

            for (double y = -10; y <= 10; y += yStep)
            {
                GL.Begin(PrimitiveType.Lines);
                GL.Vertex2(-0.1, y);
                GL.Vertex2(0.1, y);
                GL.End();
            }
        }
    }
}
