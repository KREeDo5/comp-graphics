using System;
using System.IO;
using OpenTK.Graphics.OpenGL;
using StbImageSharp;

namespace task1
{
    public static class TextureLoader
    {
        public static int LoadTexture(string path)
        {
            int textureId;
            GL.GenTextures(1, out textureId);
            GL.BindTexture(TextureTarget.Texture2D, textureId);

            try
            {
                StbImage.stbi_set_flip_vertically_on_load(1);

                using (var stream = File.OpenRead(path))
                {
                    var image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);


                    GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0,
                        PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);
                }

                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Texture load error: {e.Message}");
            }

            return textureId;
        }
    }

}
