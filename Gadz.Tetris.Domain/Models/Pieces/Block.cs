namespace Gadz.Tetris.Domain.Models.Pieces;

/// <summary>
/// Uma célula colorida do tabuleiro: um dos 4 blocos de uma peça, ou um bloco já assentado.
/// </summary>
/// <param name="x">A coluna.</param>
/// <param name="y">A linha.</param>
/// <param name="color">A cor do bloco.</param>
public struct Block(int x, int y, PieceColor color)
{
    /// <summary>
    /// A célula que o bloco ocupa.
    /// </summary>
    public Point Point { get; private set; } = new Point(x, y);

    /// <summary>
    /// A cor do bloco (a cor da peça a que ele pertence).
    /// </summary>
    public PieceColor Color { get; private set; } = color;

    /// <summary>
    /// A coluna do bloco.
    /// </summary>
    public int X
    {
        get { return Point.X; }
    }

    /// <summary>
    /// A linha do bloco.
    /// </summary>
    public int Y
    {
        get { return Point.Y; }
    }

    /// <summary>
    /// Informa se os dois blocos ocupam a mesma célula, independentemente da cor.
    /// </summary>
    /// <param name="block">O outro bloco.</param>
    /// <returns><c>true</c> se estiverem na mesma célula.</returns>
    public bool CollideWith(Block block)
    {
        return Point.ColideCom(block.Point);
    }

    /// <summary>
    /// A célula do bloco no formato <c>XxY</c>.
    /// </summary>
    /// <returns>Por exemplo, <c>3x5</c>.</returns>
    public override string ToString()
    {
        return Point.ToString();
    }
}
