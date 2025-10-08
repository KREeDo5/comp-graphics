using System;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace task1
{
    public class Grass
    {
        private int textureId;
        private readonly float size;

        public int TextureId => textureId;

        public Grass(int textureId, float size = 100.0f)
        {
            this.size = size;
            this.textureId = textureId;
        }

        public void Draw(float repeat = 20.0f)
        {
            if (textureId == 0) return;

            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, textureId);

            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0, 0); GL.Vertex3(-size, -20f, -size);
            GL.TexCoord2(repeat, 0); GL.Vertex3(size, -20f, -size);
            GL.TexCoord2(repeat, repeat); GL.Vertex3(size, -20f, size);
            GL.TexCoord2(0, repeat); GL.Vertex3(-size, -20f, size);

            GL.End();
        }

    }
}