namespace Gadz.Tetris.Domain.Models.Pieces;

/// <summary>
/// Os sete tetraminós, nomeados pela letra que lembram.
/// </summary>
public enum PieceType
{
    /// <summary>
    /// Três blocos em linha com um no meio de um dos lados.
    /// </summary>
    T = 0,

    /// <summary>
    /// Quadrado 2×2.
    /// </summary>
    O,

    /// <summary>
    /// Três blocos em linha com um dobrado numa ponta.
    /// </summary>
    L,

    /// <summary>
    /// Espelho do L.
    /// </summary>
    J,

    /// <summary>
    /// Dois pares de blocos desencontrados.
    /// </summary>
    S,

    /// <summary>
    /// Espelho do S.
    /// </summary>
    Z,

    /// <summary>
    /// Quatro blocos em linha; a única peça que completa 4 linhas de uma vez (TETRIS).
    /// </summary>
    I
}
