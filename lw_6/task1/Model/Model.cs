using Assimp;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace task1
{
    public class Model
    {
        private Scene scene = new();

        private int[] displayLists = [];
        private Loader Loader = new();

        public void LoadModel(string filePath)
        {
            AssimpContext importer = new AssimpContext();
            scene = importer.ImportFile(
                filePath,
                PostProcessSteps.FlipUVs);
            importer.Dispose();

            LoadTextures();
            CreateDisplayLists();
        }

        private void CreateDisplayLists()
        {
            displayLists = new int[scene.MeshCount];

            for (int i = 0; i < scene.MeshCount; i++)
            {
                Mesh mesh = scene.Meshes[i];

                displayLists[i] = GL.GenLists(1);
                GL.NewList(displayLists[i], ListMode.Compile);

                Loader.ApplyMaterial(scene.Materials[mesh.MaterialIndex], i);

                Vector3[] vertices = AssimpVectorToOpenTKVector([.. mesh.Vertices]);
                Vector3[] normals = AssimpVectorToOpenTKVector([.. mesh.Normals]);
                Vector3[] textureCoordinates = AssimpVectorToOpenTKVector([.. mesh.TextureCoordinateChannels[0]]);

                Vector3 min = vertices[0];
                Vector3 max = vertices[0];

                for (int k = 1; k < vertices.Length; k++)
                {
                    min = Vector3.ComponentMin(min, vertices[k]);
                    max = Vector3.ComponentMax(max, vertices[k]);
                }

                Vector3 center = (min + max) / 2f;

                for (int k = 0; k < vertices.Length; k++)
                {
                    vertices[k] -= center;
                }


                GL.Begin(mesh.Faces[0].IndexCount % 3 == 0 ? OpenTK.Graphics.OpenGL.PrimitiveType.Triangles : OpenTK.Graphics.OpenGL.PrimitiveType.Quads);
                for (int k = 0; k < vertices.Length; k++)
                {
                    GL.Normal3(normals[k]);
                    if (scene.Materials[mesh.MaterialIndex].HasTextureDiffuse)
                    {
                        GL.TexCoord2(textureCoordinates[k].X, textureCoordinates[k].Y);
                    }
                    GL.Vertex3(vertices[k]);
                }
                GL.End();

                GL.EndList();
            }
        }


        private void LoadTextures()
        {
            Loader = new Loader();

            for (int i = 0; i < scene.MeshCount; i++)
            {
                Mesh mesh = scene.Meshes[i];

                if (mesh.MaterialIndex >= 0)
                {
                    Material material = scene.Materials[mesh.MaterialIndex];
                    Loader.LoadMaterialTextures(material);
                }
            }
        }

        public void RenderModel()
        {
            for (int i = 0; i < scene.MeshCount; i++)
            {
                GL.CallList(displayLists[i]);
            }
        }

        private Vector3[] AssimpVectorToOpenTKVector(Vector3D[] vecArr)
        {
            Vector3[] vector3s = new Vector3[vecArr.Length];

            for (int i = 0; i < vecArr.Length; i++)
            {
                vector3s[i].X = vecArr[i].X;
                vector3s[i].Y = vecArr[i].Y;
                vector3s[i].Z = vecArr[i].Z;
            }

            return vector3s;
        }
    }
}
