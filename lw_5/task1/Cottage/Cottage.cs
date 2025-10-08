using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace task1
{
    public class Cottage
    {
        public Vector3 Position { get; set; }
        public float Scale { get; set; } = 1.0f;

        public int[] textures;

        private readonly Vector3 houseSize = new Vector3(8, 3, 10);

        public Cottage(int[] textures)
        {
            this.textures = textures;
        }

        public void Draw()
        {
            GL.PushMatrix();
            GL.Translate(Position);
            GL.Scale(Scale, Scale, Scale);

            DrawMainBuilding();
            DrawRoof();
            DrawWindows();
            DrawDoor();
            DrawPorch();
            DrawCanopy();

            GL.PopMatrix();
        }

        private void DrawMainBuilding()
        {
            Vector3 position = new Vector3(0, houseSize.Y / 2 + 0.01f, 0);
            DrawHelper.DrawRectangularCuboid(position, houseSize, textures[2], 2.0f);
        }

        private void DrawRoof()
        {
            DrawHelper.DrawHouseRoof(Vector3.Zero, houseSize, 2.0f, textures[3], 2.0f, false);
        }

        private void DrawDoor()
        {
            Vector3 doorSize = new Vector3(0.1f, 2.0f, 1.0f);
            Vector3 doorPosition = new Vector3(
                -houseSize.X / 2 - doorSize.X / 2 + 0.01f,
                houseSize.Y * 0.4f,
                houseSize.Z * 0.25f);

            DrawHelper.DrawRectangularCuboid(doorPosition, doorSize, textures[5], 1.0f);
        }

        private void DrawPorch()
        {
            Vector3 doorPosition = new Vector3(
                -houseSize.X / 2,
                houseSize.Y * 0.4f,
                houseSize.Z * 0.25f);

            Vector3 porchPosition = new Vector3(doorPosition.X - 0.5f, 0.11f, doorPosition.Z);
            Vector3 porchSize = new Vector3(1.0f, 0.2f, 2.0f);

            DrawHelper.DrawRectangularCuboid(porchPosition, porchSize, textures[7], 1.0f);
        }


        void DrawWindows()
        {
            float windowWidth = 0.8f;
            float windowHeight = 1.0f;
            float windowDepth = 0.1f;
            Vector3 sideWindow = new Vector3(windowWidth, windowHeight, windowDepth);
            Vector3 frontBackWindow = new Vector3(windowDepth, windowHeight, windowWidth);

            float windowY = houseSize.Y * 0.6f;
            float longSideOffset = houseSize.X * 0.25f;
            float shortSideOffset = houseSize.Z * 0.3f;
            float windowSpacing = houseSize.Z * 0.25f;
            float wallOffset = 0.01f;

            void DrawWindow(Vector3 pos, Vector3 size) =>
                DrawHelper.DrawRectangularCuboid(pos, size, textures[4], 1.0f);

            DrawWindow(new Vector3(-longSideOffset, windowY, -houseSize.Z / 2 - windowDepth / 2), sideWindow);
            DrawWindow(new Vector3(longSideOffset, windowY, -houseSize.Z / 2 - windowDepth / 2), sideWindow);

            for (int i = -1; i <= 1; i++)
            {
                DrawWindow(
                    new Vector3(houseSize.X / 2 + windowDepth / 2, windowY, i * windowSpacing),
                    frontBackWindow
                );
            }

            for (int i = -1; i <= 0; i++)
            {
                DrawWindow(
                    new Vector3(-houseSize.X / 2 - windowDepth / 2 + wallOffset, windowY, i * windowSpacing),
                    frontBackWindow
                );
            }
        }

        private void DrawCanopy()
        {
            float canopyHeight = 0.4f;
            float canopyWidth = 1.3f;
            float canopyDepth = 2.0f;

            Vector3 doorPosition = new Vector3(
                -houseSize.X / 2 - 0.65f,
                houseSize.Y * 0.4f + 0.5f,
                houseSize.Z * 0.25f);

            Vector3 frontLeft = new Vector3(
                doorPosition.X,
                doorPosition.Y + 1f,
                doorPosition.Z - canopyWidth / 2);

            Vector3 frontRight = new Vector3(
                doorPosition.X,
                doorPosition.Y + 1f,
                doorPosition.Z + canopyWidth / 2);

            DrawHelper.DrawHouseRoof(doorPosition, new Vector3(canopyWidth, canopyHeight, canopyDepth), canopyHeight, textures[3], 2.0f, true);

            DrawCanopyPillar(frontLeft + new Vector3(-0.2f, -1.1f, -0.15f));
            DrawCanopyPillar(frontRight + new Vector3(-0.2f, -1.1f, 0.15f));
        }

        private void DrawCanopyPillar(Vector3 basePosition)
        {
            Vector3 pillarSize = new Vector3(0.1f, 2.0f, 0.1f);
            Vector3 position = new Vector3(basePosition.X, 1.1f, basePosition.Z);

            DrawHelper.DrawRectangularCuboid(position, pillarSize, textures[7], 1.0f);
        }
    }
}