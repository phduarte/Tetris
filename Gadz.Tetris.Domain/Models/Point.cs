namespace Gadz.Tetris.Domain.Models;

/// <summary>
/// Defines the <see cref="Point" />
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref=""/> class.
/// </remarks>
/// <param name="x">The x<see cref="int"/></param>
/// <param name="y">The y<see cref="int"/></param>
public struct Point(int x, int y)
{
    /// <summary>
    /// Defines the X, Y
    /// </summary>
    public int X = x, Y = y;

    /// <summary>
    /// The ToString
    /// </summary>
    /// <returns>The <see cref="string"/></returns>
    public override string ToString()
    {
        return $"{X}x{Y}";
    }

    /// <summary>
    /// The Equals
    /// </summary>
    /// <param name="obj">The obj<see cref="object"/></param>
    /// <returns>The <see cref="bool"/></returns>
    public override bool Equals(object? obj)
    {
        return obj is Point u && X == u.X && Y == u.Y;
    }

    public static bool operator ==(Point a, Point b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(Point a, Point b)
    {
        return !a.Equals(b);
    }
    /// <summary>
    /// The ColideCom
    /// </summary>
    /// <param name="ponto">The ponto<see cref="Point"/></param>
    /// <returns>The <see cref="bool"/></returns>
    public bool ColideCom(Point ponto)
    {
        return Equals(ponto);
    }

    /// <summary>
    /// The GetHashCode
    /// </summary>
    /// <returns>The <see cref="int"/></returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }
}
