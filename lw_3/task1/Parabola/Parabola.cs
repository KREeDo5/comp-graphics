using OpenTK.Graphics.OpenGL;

namespace task1
{
    public class Parabola
{
    private double a, b, c;
    private double xStart, xEnd;

    public Parabola(double a, double b, double c, double xStart, double xEnd)
    {
        this.a = a;
        this.b = b;
        this.c = c;
        this.xStart = xStart;
        this.xEnd = xEnd;
    }

    public void Draw()
    {
        GL.LoadIdentity();
        GL.Color3(0.0f, 0.0f, 1.0f);
        GL.LineWidth(3.0f);

        GL.Begin(PrimitiveType.LineStrip);
        for (double x = xStart; x <= xEnd; x += 0.01)
        {
            double y = a * x * x + b * x + c;
            GL.Vertex2(x, y);
        }
        GL.End();
    }
}
}

