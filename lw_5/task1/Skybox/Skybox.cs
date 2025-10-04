using System;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace task1
{
    public class Skybox
    {
        private int textureId;
        private readonly float size;

        public int TextureId => textureId;

        public Skybox(int textureId, float size = 100.0f)
        {
            this.size = size;
            this.textureId = textureId;
        }

        public void Draw()
        {
            if (this.textureId == 0) return;

            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, this.textureId);

            GL.Begin(PrimitiveType.Quads);

            // pered
            GL.TexCoord2(1, 0); GL.Vertex3(-size, -size, size);
            GL.TexCoord2(0, 0); GL.Vertex3(size, -size, size);
            GL.TexCoord2(0, 1); GL.Vertex3(size, size, size);
            GL.TexCoord2(1, 1); GL.Vertex3(-size, size, size);

            // zad
            GL.TexCoord2(0, 0); GL.Vertex3(-size, -size, -size);
            GL.TexCoord2(1, 0); GL.Vertex3(size, -size, -size);
            GL.TexCoord2(1, 1); GL.Vertex3(size, size, -size);
            GL.TexCoord2(0, 1); GL.Vertex3(-size, size, -size);

            // Up
            GL.TexCoord2(0, 1); GL.Vertex3(-size, size, -size);
            GL.TexCoord2(1, 1); GL.Vertex3(size, size, -size);
            GL.TexCoord2(1, 0); GL.Vertex3(size, size, size);
            GL.TexCoord2(0, 0); GL.Vertex3(-size, size, size);

            // Right
            GL.TexCoord2(1, 0); GL.Vertex3(size, -size, -size);
            GL.TexCoord2(0, 0); GL.Vertex3(size, -size, size);
            GL.TexCoord2(0, 1); GL.Vertex3(size, size, size);
            GL.TexCoord2(1, 1); GL.Vertex3(size, size, -size);

            // Left
            GL.TexCoord2(0, 0); GL.Vertex3(-size, -size, -size);
            GL.TexCoord2(1, 0); GL.Vertex3(-size, -size, size);
            GL.TexCoord2(1, 1); GL.Vertex3(-size, size, size);
            GL.TexCoord2(0, 1); GL.Vertex3(-size, size, -size);

            GL.End();
        }
    }
}