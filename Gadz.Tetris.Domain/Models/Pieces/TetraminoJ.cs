namespace Gadz.Tetris.Domain.Models.Pieces;

/// <summary>
/// Tetraminó J: uma linha de três blocos com um quarto bloco dobrado numa das pontas.
/// </summary>
/// <param name="config">Tipo, posição e rotação iniciais da peça.</param>
internal class TetraminoJ(PieceConfiguration config) : Piece(config)
{
    /// <summary>
    /// A forma de cada rotação, de 0 a 3.
    /// </summary>
    private static readonly Point[][] SHAPES =
    [
        [new(1, 0), new(1, 1), new(1, 2), new(0, 2)],
        [new(0, 0), new(1, 0), new(2, 0), new(2, 1)],
        [new(0, 0), new(0, 1), new(0, 2), new(1, 0)],
        [new(0, 0), new(0, 1), new(1, 1), new(2, 1)]
    ];

    /// <inheritdoc/>
    protected override Point[][] Shapes => SHAPES;
}
