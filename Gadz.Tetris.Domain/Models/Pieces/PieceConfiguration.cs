using Gadz.Tetris.Domain.Models;

namespace Gadz.Tetris.Domain.Models.Pieces
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
