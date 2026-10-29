namespace Gadz.Tetris.Domain.Models.Pieces;

/// <summary>
/// Tetraminó S: dois pares de blocos desencontrados, com uma forma em pé e outra deitada.
/// </summary>
/// <param name="config">Tipo, posição e rotação iniciais da peça.</param>
internal class TetraminoS(PieceConfiguration config) : Piece(config)
{
    /// <summary>
    /// O S em pé.
    /// </summary>
    private static readonly Point[] VERTICAL = [new(0, 0), new(0, 1), new(1, 1), new(1, 2)];

    /// <summary>
    /// O S deitado.
    /// </summary>
    private static readonly Point[] HORIZONTAL = [new(0, 1), new(1, 1), new(1, 0), new(2, 0)];

    /// <summary>
    /// A forma de cada rotação, de 0 a 3.
    /// </summary>
    private static readonly Point[][] SHAPES = [VERTICAL, HORIZONTAL, VERTICAL, HORIZONTAL];

    /// <inheritdoc/>
    protected override Point[][] Shapes => SHAPES;
}
