namespace Gadz.Tetris.Domain.Models.Pieces;

/// <summary>
/// Tetraminó I: quatro blocos em linha, na vertical nas rotações pares e na horizontal nas ímpares.
/// </summary>
/// <param name="config">Tipo, posição e rotação iniciais da peça.</param>
internal class TetraminoI(PieceConfiguration config) : Piece(config)
{
    /// <summary>
    /// A linha em pé.
    /// </summary>
    private static readonly Point[] VERTICAL = [new(0, 0), new(0, 1), new(0, 2), new(0, 3)];

    /// <summary>
    /// A linha deitada.
    /// </summary>
    private static readonly Point[] HORIZONTAL = [new(0, 0), new(1, 0), new(2, 0), new(3, 0)];

    /// <summary>
    /// A forma de cada rotação, de 0 a 3.
    /// </summary>
    private static readonly Point[][] SHAPES = [VERTICAL, HORIZONTAL, VERTICAL, HORIZONTAL];

    /// <inheritdoc/>
    protected override Point[][] Shapes => SHAPES;
}
