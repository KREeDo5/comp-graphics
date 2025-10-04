using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace task1
{
    public class Chess
    {
        private readonly Model _rook;
        private readonly Model _knight;
        private readonly Model _bishop;
        private readonly Model _queen;
        private readonly Model _king;
        private readonly Model _pawn;
        private readonly Model _board;

        private readonly float _cellSize = 40f;

        private List<Figure> _pieces = new List<Figure>();

        public List<Move> _moves = new List<Move>();
        public int _currentMoveIndex = 0;
        public bool _isAnimating = false;
        private float _moveSpeed = 0.05f;

        public Chess()
        {
            _board = new Model();
            _board.LoadModel("models/board.obj");

            _rook = new Model();
            _rook.LoadModel("models/rook.obj");

            _knight = new Model();
            _knight.LoadModel("models/knight.obj");

            _bishop = new Model();
            _bishop.LoadModel("models/bishop.obj");

            _queen = new Model();
            _queen.LoadModel("models/queen.obj");

            _king = new Model();
            _king.LoadModel("models/king.obj");

            _pawn = new Model();
            _pawn.LoadModel("models/pawn.obj");

            InitializePieces();
            InitializeMoves();
        }

        public void Draw()
        {
            DrawBoard();

            GL.PushMatrix();
            GL.Translate(0f, 20f, 0f);
            GL.Rotate(-90f, 1f, 0f, 0f);

            foreach (var piece in _pieces)
            {
                if (piece.Enabled)
                {
                    GL.PushMatrix();

                    float zPosition = 20f;
                    if (piece.Model == _rook)
                        zPosition = 32f;
                    else if (piece.Model != _pawn)
                        zPosition = 45f;

                    GL.Translate(_cellSize * piece.Position.X - 140f, _cellSize * (7 - piece.Position.Y) - 140f, zPosition);
                    Color4 color = piece.Color == Color4.Black ? Color4.Gray : piece.Color;
                    piece.Color = color;

                    GL.Rotate(piece.Rotation, 0f, 0f, 1f);
                    GL.Color4(piece.Color);
                    GL.Scale(10f, 10f, 10f);

                    piece.Model.RenderModel();

                    GL.PopMatrix();
                }
            }

            GL.PopMatrix();
        }

        private void DrawBoard()
        {
            GL.PushMatrix();
            GL.Scale(90f, 90f, 90f);
            GL.Color4(Color4.White);
            _board.RenderModel();
            GL.PopMatrix();
        }

        private void InitializePieces()
        {
            for (int col = 0; col < 8; col++)
                _pieces.Add(new Figure(_pawn, 6, col, Color4.White)); // 0-7

            _pieces.Add(new Figure(_rook, 7, 0, Color4.White)); // 8
            _pieces.Add(new Figure(_knight, 7, 1, Color4.White)); // 9
            _pieces.Add(new Figure(_bishop, 7, 2, Color4.White)); // 10
            _pieces.Add(new Figure(_queen, 7, 3, Color4.White)); // 11
            _pieces.Add(new Figure(_king, 7, 4, Color4.White)); // 12
            _pieces.Add(new Figure(_bishop, 7, 5, Color4.White)); // 13
            _pieces.Add(new Figure(_knight, 7, 6, Color4.White)); // 14
            _pieces.Add(new Figure(_rook, 7, 7, Color4.White)); // 15

            for (int col = 0; col < 8; col++)
                _pieces.Add(new Figure(_pawn, 1, col, Color4.Black)); //16-23

            _pieces.Add(new Figure(_rook, 0, 0, Color4.Black, 180f)); // 24
            _pieces.Add(new Figure(_knight, 0, 1, Color4.Black, 180f)); // 25
            _pieces.Add(new Figure(_bishop, 0, 2, Color4.Black, 180f)); // 26
            _pieces.Add(new Figure(_queen, 0, 3, Color4.Black, 180f)); // 27
            _pieces.Add(new Figure(_king, 0, 4, Color4.Black, 180f)); //28
            _pieces.Add(new Figure(_bishop, 0, 5, Color4.Black, 180f)); //29
            _pieces.Add(new Figure(_knight, 0, 6, Color4.Black, 180f)); // 30
            _pieces.Add(new Figure(_rook, 0, 7, Color4.Black, 180f)); //31
        }

        private void InitializeMoves()
        {
            Figure figure = _pieces[4];
            _moves.Add(new Move(figure, 4, 4)); // 0

            figure = _pieces[20];
            _moves.Add(new Move(figure, 3, 4));

            figure = _pieces[13];
            _moves.Add(new Move(figure, 4, 2)); // 3

            figure = _pieces[25];
            _moves.Add(new Move(figure, 2, 2));

            figure = _pieces[11];
            _moves.Add(new Move(figure, 3, 7)); // 5

            figure = _pieces[30];
            _moves.Add(new Move(figure, 2, 5));

            figure = _pieces[11];
            _moves.Add(new Move(figure, 1, 5));
        }

        public void NextStep()
        {
            if (_currentMoveIndex >= _moves.Count)
                return;

            if (_currentMoveIndex == 6)
            {
                _pieces[21].Enabled = false;
            }

            Move move = _moves[_currentMoveIndex];
            Figure figure = move.Figure;

            if (!move.IsFinished)
            {
                MoveFigure(figure, move.TargetPosition);

                if (Vector2.Distance(figure.Position, move.TargetPosition) < 0.01f)
                {
                    figure.Position = move.TargetPosition;
                    move.IsFinished = true;
                    Console.WriteLine("Move finished");
                }
            }
            else
            {
                _currentMoveIndex++;
            }
        }

        public void MoveFigure(Figure figure, Vector2 targetPosition)
        {
            figure.Position = Vector2.Lerp(figure.Position, targetPosition, _moveSpeed);
        }
    }

    public class Move
    {
        public Figure Figure { get; set; }
        public Vector2 TargetPosition { get; set; }
        public bool IsFinished { get; set; }

        public Move(Figure figure, int targetRow, int targetCol)
        {
            Figure = figure;
            TargetPosition = new Vector2(targetCol, targetRow);
            IsFinished = false;
        }
    }
}


