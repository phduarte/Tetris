namespace Gadz.Tetris.Domain.Models.Pieces;

/// <summary>
/// Cor de um bloco. Cada <see cref="PieceType"/> tem uma cor fixa (ver <see cref="Piece.GetPieceColor"/>).
/// </summary>
public enum PieceColor
{
    /// <summary>
    /// Sem cor: célula vazia do tabuleiro.
    /// </summary>
    None = 0,

    /// <summary>
    /// Cor da peça O.
    /// </summary>
    Yellow,

    /// <summary>
    /// Cor da peça J.
    /// </summary>
    Blue,

    /// <summary>
    /// Cor da peça I.
    /// </summary>
    Cyan,

    /// <summary>
    /// Cor da peça L.
    /// </summary>
    Orange,

    /// <summary>
    /// Cor da peça S.
    /// </summary>
    Green,

    /// <summary>
    /// Cor da peça Z.
    /// </summary>
    Red,

    /// <summary>
    /// Cor da peça T.
    /// </summary>
    Purple
}
