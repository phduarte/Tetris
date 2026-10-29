namespace Gadz.Tetris.Domain.Models.Pieces;

/// <summary>
/// Tetraminó Z: o espelho do S, com uma forma em pé e outra deitada.
/// </summary>
/// <param name="config">Tipo, posição e rotação iniciais da peça.</param>
internal class TetraminoZ(PieceConfiguration config) : Piece(config)
{
    /// <summary>
    /// O Z em pé.
    /// </summary>
    private static readonly Point[] VERTICAL = [new(1, 0), new(1, 1), new(0, 1), new(0, 2)];

    /// <summary>
    /// O Z deitado.
    /// </summary>
    private static readonly Point[] HORIZONTAL = [new(0, 0), new(1, 0), new(1, 1), new(2, 1)];

    /// <summary>
    /// A forma de cada rotação, de 0 a 3.
    /// </summary>
    private static readonly Point[][] SHAPES = [VERTICAL, HORIZONTAL, VERTICAL, HORIZONTAL];

    /// <inheritdoc/>
    protected override Point[][] Shapes => SHAPES;
}
