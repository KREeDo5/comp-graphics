using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;

namespace task2
{
    public static class Helper
    {
        public static void DrawCircle(float x, float y, float radius)
        {
            GL.Begin(PrimitiveType.TriangleFan);
            GL.Vertex2(x, y);
            for (float i = 0.0f; i <= 360; i++)
            {
                GL.Vertex2(radius * (Math.Cos(Math.PI * i / 180.0) + x),
                    radius * (Math.Sin(Math.PI * i / 180.0) + y));
            }

            GL.End();
        }

        public static void DrawEllipse(float x, float y, float width, float height)
        {
            GL.Begin(PrimitiveType.TriangleFan);
            GL.Vertex2(x, y);

            for (float i = 0.0f; i <= 360; i++)
            {
                GL.Vertex2(width * Math.Cos(Math.PI * i / 180.0) + x,
                    height * Math.Sin(Math.PI * i / 180.0) + y);
            }

            GL.End();
        }

        public static void DrawCasteljauBezier(List<Vector2> points, bool fill)
        {
            int segments = 50;

            if (points.Count < 2) return;

            if (fill)
            {
                GL.Begin(PrimitiveType.TriangleFan);
            }
            else
            {
                GL.Begin(PrimitiveType.LineStrip);
            }

            List<Vector2> bezierPoints = new List<Vector2>();

            for (int i = 0; i <= segments; i++)
            {
                float t = i / (float)segments;

                List<Vector2> tempPoints = new List<Vector2>(points);

                while (tempPoints.Count > 1)
                {
                    List<Vector2> nextTempPoints = new List<Vector2>();

                    for (int j = 0; j < tempPoints.Count - 1; j++)
                    {
                        float x = (1 - t) * tempPoints[j].X + t * tempPoints[j + 1].X;
                        float y = (1 - t) * tempPoints[j].Y + t * tempPoints[j + 1].Y;

                        nextTempPoints.Add(new Vector2(x, y));
                    }

                    tempPoints = nextTempPoints;
                }

                bezierPoints.Add(tempPoints[0]);
            }

            foreach (var p in bezierPoints)
            {
                GL.Vertex2(p.X, p.Y);
            }

            GL.End();
        }
    }
}
