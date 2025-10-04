using OpenTK.Mathematics;

namespace task1
{
    public class Figure
    {
        public Model Model { get; set; }
        public Vector2 Position { get; set; }
        public Color4 Color { get; set; }
        public float Rotation { get; set; }

        public Vector2 TargetPosition { get; set; }
        public bool Enabled { get; set; }

        public Figure(Model model, int row, int col, Color4 color, float rotation = 0f, bool enabled = true)
        {
            Model = model;
            Position = new Vector2(col, row);
            TargetPosition = new Vector2(col, row);
            Color = color;
            Rotation = rotation;
            Enabled = enabled;
        }
    }
}


