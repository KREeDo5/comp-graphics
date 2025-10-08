using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task2
{
    public abstract class BaseSmesharik
    {
        public float x { get; set; }
        public float y { get; set; }
        public float size { get; set; }
        protected Color4 bodyColor { get; set; }
        protected Color4 borderColor { get; set; }

        protected BaseSmesharik(float x, float y, float size, Color4 bodyColor)
        {
            this.x = x;
            this.y = y;
            this.size = size;
            this.bodyColor = bodyColor;
            this.borderColor = AdjustColor(bodyColor, 0.8f);
        }

        private static Color4 AdjustColor(Color4 color, float factor)
        {
            return new Color4(
                Math.Min(Math.Max(color.R * factor, 0.0f), 1.0f),
                Math.Min(Math.Max(color.G * factor, 0.0f), 1.0f),
                Math.Min(Math.Max(color.B * factor, 0.0f), 1.0f),
                color.A
            );
        }

        protected abstract void DrawBody();
        protected abstract void DrawEyes();
        protected abstract void DrawMouth();
        protected abstract void DrawHands();
        protected abstract void DrawLegs();
        protected abstract void DrawEars();

        public void Draw()
        {
            GL.PushMatrix();
            GL.Translate(x, y, 0); 
            GL.Scale(size, size, 1);

            DrawBody();
            DrawEyes();
            DrawMouth();
            DrawHands();
            DrawLegs();
            DrawEars();

            GL.PopMatrix();
        }
    }
}
