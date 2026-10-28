namespace Gadz.Tetris.Domain.Models.Pieces;

/// <summary>
/// Bloco
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref=""/> class.
/// </remarks>
/// <param name="x">The x<see cref="int"/></param>
/// <param name="y">The y<see cref="int"/></param>
/// <param name="color">The color<see cref="PieceColor"/></param>
public struct Block(int x, int y, PieceColor color)
{
    /// <summary>
    /// Gets the Point
    /// </summary>
    public Point Point { get; private set; } = new Point(x, y);

    /// <summary>
    /// Gets the Color
    /// </summary>
    public PieceColor Color { get; private set; } = color;

    /// <summary>
    /// Gets the X
    /// </summary>
    public int X
    {
        get { return Point.X; }
    }

    /// <summary>
    /// Gets the Y
    /// </summary>
    public int Y
    {
        get { return Point.Y; }
    }

    /// <summary>
    /// The CollideWith
    /// </summary>
    /// <param name="block">The block<see cref="Block"/></param>
    /// <returns>The <see cref="bool"/></returns>
    public bool CollideWith(Block block)
    {
        return Point.ColideCom(block.Point);
    }

    /// <summary>
    /// The ToString
    /// </summary>
    /// <returns>The <see cref="string"/></returns>
    public override string ToString()
    {
        return Point.ToString();
    }
}
