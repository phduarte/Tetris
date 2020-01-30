namespace Gadz.Tetris.Model
{
    /// <summary>
    /// Bloco
    /// </summary>
    public struct Block
    {
        public Point Point { get; private set; }
        public PieceColor Color { get; private set; }
        public int X { get { return Point.X; } }
        public int Y { get { return Point.Y; } }

        public Block(int x, int y, PieceColor color)
        {
            Point = new Point(x, y);
            Color = color;
        }

        public bool CollideWith(Block block)
        {
            return Point.ColideCom(block.Point);
        }

        public override string ToString()
        {
            return Point.ToString();
        }
    }
}
