namespace Gadz.Tetris.Domain.Models.Pieces;

/// <summary>
/// Tetraminó O: um quadrado 2×2, igual em todas as rotações.
/// </summary>
/// <param name="config">Tipo, posição e rotação iniciais da peça.</param>
internal class TetraminoO(PieceConfiguration config) : Piece(config)
{
    /// <summary>
    /// O quadrado.
    /// </summary>
    private static readonly Point[] SQUARE = [new(0, 0), new(0, 1), new(1, 0), new(1, 1)];

    /// <summary>
    /// A forma de cada rotação, de 0 a 3.
    /// </summary>
    private static readonly Point[][] SHAPES = [SQUARE, SQUARE, SQUARE, SQUARE];

    /// <inheritdoc/>
    protected override Point[][] Shapes => SHAPES;
}
