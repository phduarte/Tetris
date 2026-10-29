namespace Gadz.Tetris.Domain.Models.Pieces;

/// <summary>
/// Os dados de criação de uma peça, montados pelo <see cref="PieceBuilder"/> e entregues ao tetraminó.
/// </summary>
public struct PieceConfiguration
{
    /// <summary>
    /// O tetraminó.
    /// </summary>
    public PieceType Type;

    /// <summary>
    /// A posição inicial da peça.
    /// </summary>
    public Point Position;

    /// <summary>
    /// A rotação inicial, de 0 a 3.
    /// </summary>
    public int Rotation;

    /// <summary>
    /// A cor correspondente ao tipo.
    /// </summary>
    public PieceColor Color => Piece.GetPieceColor(Type);
}
