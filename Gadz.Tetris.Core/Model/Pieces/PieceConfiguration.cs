namespace Gadz.Tetris.Model.Pieces
{
    /// <summary>
    /// TetraminoConfiguration
    /// </summary>
    public struct PieceConfiguration
    {
        public PieceType Type;
        public Point Position;
        public int Rotation;
        public PieceColor Color => Piece.GetPieceColor(Type);
    }
}
