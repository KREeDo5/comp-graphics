using Assimp;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System.Drawing;
using System.Drawing.Imaging;
using TextureWrapMode = OpenTK.Graphics.OpenGL.TextureWrapMode;

namespace task1
{
    public class Loader
    {
        private int[] textureIds;

        private bool textureLoaded = false;

        public void LoadMaterialTextures(Material material)
        {
            textureIds = new int[material.GetMaterialTextureCount(TextureType.Diffuse)];
            textureLoaded = true;

            for (int i = 0; i < material.GetMaterialTextureCount(TextureType.Diffuse); i++)
            {
                TextureSlot textureSlot;

                if (!material.GetMaterialTexture(TextureType.Diffuse, i, out textureSlot))
                {
                    continue;
                }

                string textureFilePath = $"models/{textureSlot.FilePath}";

                if (!File.Exists(textureFilePath))
                {
                    string absolutePath = Path.Combine(Directory.GetCurrentDirectory(), textureFilePath);
                    Console.WriteLine($"Texture file not found: {absolutePath}");
                    continue;
                }

                int textureId = GL.GenTexture();
                GL.BindTexture(TextureTarget.Texture2D, textureId);

                using (Bitmap bitmap = new Bitmap(textureFilePath))
                {
                    BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                        ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                    GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
                        data.Width, data.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
                        PixelType.UnsignedByte, data.Scan0);
                    bitmap.UnlockBits(data);
                }

                GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

                textureIds[i] = textureId;
            }
        }

        public void ApplyMaterial(Material material, int index)
        {
            Color4 ambientColor = Color4DToColor4(material.ColorAmbient);
            Color4 diffuseColor = Color4DToColor4(material.ColorDiffuse);
            Color4 specularColor = Color4DToColor4(material.ColorSpecular);
            float shininess = material.Shininess;

            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Ambient, ambientColor);
            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Diffuse, diffuseColor);
            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Specular, specularColor);
            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Shininess, shininess);

            if (textureLoaded && material.GetMaterialTexture(TextureType.Diffuse, index, out TextureSlot textureSlot))
            {
                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.Texture2D, textureIds[index]);
            }
        }
        private Color4 Color4DToColor4(Color4D color4d)
        {
            Color4 color4 = new()
            {
                A = color4d.A,
                R = color4d.R,
                G = color4d.G,
                B = color4d.B
            };
            return color4;
        }
    }
}