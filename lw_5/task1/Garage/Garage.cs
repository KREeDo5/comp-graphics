using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task1
{
    public class Garage
    {
        public Vector3 Position { get; set; }
        public float Scale { get; set; } = 1.0f;
        public Vector3 Size { get; set; } = new Vector3(6, 2.5f, 5); 

        public int[] _textures;

        public Garage(int[] textures)
        {
            _textures = textures;
        }

        public void Draw()
        {
            GL.PushMatrix();
            GL.Translate(Position);
            GL.Scale(Scale, Scale, Scale);

            DrawHelper.DrawRectangularCuboid(
                new Vector3(0, Size.Y / 2 + 0.01f, 0),
                Size,
                _textures[2], 
                3.0f);

            float roofHeight = 0.5f; 

            Vector3 frontLeftLow = new Vector3(-Size.X / 2, Size.Y, -Size.Z / 2);
            Vector3 frontRightLow = new Vector3(Size.X / 2, Size.Y, -Size.Z / 2);
            Vector3 backLeftLow = new Vector3(-Size.X / 2, Size.Y, Size.Z / 2);
            Vector3 backRightLow = new Vector3(Size.X / 2, Size.Y, Size.Z / 2);

            Vector3 frontLeftHigh = frontLeftLow + new Vector3(0, roofHeight, 0);
            Vector3 frontRightHigh = frontRightLow + new Vector3(0, roofHeight, 0);
            Vector3 backLeftHigh = backLeftLow;
            Vector3 backRightHigh = backRightLow;


            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, _textures[3]);

            GL.Begin(PrimitiveType.Quads);

            GL.TexCoord2(0, 0); GL.Vertex3(frontLeftHigh);
            GL.TexCoord2(3, 0); GL.Vertex3(frontRightHigh);
            GL.TexCoord2(3, 3); GL.Vertex3(backRightHigh);
            GL.TexCoord2(0, 3); GL.Vertex3(backLeftHigh);

            GL.TexCoord2(0, 0); GL.Vertex3(frontLeftLow);
            GL.TexCoord2(3, 0); GL.Vertex3(backLeftLow);
            GL.TexCoord2(3, 3); GL.Vertex3(backLeftHigh);
            GL.TexCoord2(0, 3); GL.Vertex3(frontLeftHigh);

            GL.TexCoord2(0, 0); GL.Vertex3(frontRightLow);
            GL.TexCoord2(3, 0); GL.Vertex3(backRightLow);
            GL.TexCoord2(3, 3); GL.Vertex3(backRightHigh);
            GL.TexCoord2(0, 3); GL.Vertex3(frontRightHigh);

            GL.End();

            GL.Disable(EnableCap.Texture2D);

            float windowWidth = 0.8f;
            float windowHeight = 0.6f;
            float windowDepth = 0.1f;
            float windowY = Size.Y * 0.6f;
            float windowSpacing = Size.Z * 0.3f; 

            DrawHelper.DrawRectangularCuboid(
                new Vector3(Size.X / 2 + windowDepth / 2, windowY, -windowSpacing),
                new Vector3(windowDepth, windowHeight, windowWidth),
                _textures[4],
                1.0f);

            DrawHelper.DrawRectangularCuboid(
                new Vector3(Size.X / 2 + windowDepth / 2, windowY, windowSpacing),
                new Vector3(windowDepth, windowHeight, windowWidth),
                _textures[4],
                1.0f);

            DrawHelper.DrawRectangularCuboid(
                   new Vector3(-3,
                   1.0f + 0.01f,
                   0),
                   new Vector3(0.1f, 2.0f, 3.0f),
                   _textures[6],
                   1.0f);

            GL.PopMatrix();
        }
    }
}
