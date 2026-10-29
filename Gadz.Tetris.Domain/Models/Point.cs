namespace Gadz.Tetris.Domain.Models;

/// <summary>
/// Uma célula do tabuleiro, ou um deslocamento entre células. X cresce para a direita e Y para baixo.
/// </summary>
/// <param name="x">A coluna.</param>
/// <param name="y">A linha.</param>
public struct Point(int x, int y)
{
    /// <summary>
    /// A coluna (<see cref="X"/>) e a linha (<see cref="Y"/>).
    /// </summary>
    public int X = x, Y = y;

    /// <summary>
    /// O ponto no formato <c>XxY</c>, usado também para comparar peças.
    /// </summary>
    /// <returns>Por exemplo, <c>3x5</c>.</returns>
    public override string ToString()
    {
        return $"{X}x{Y}";
    }

    /// <summary>
    /// Dois pontos são iguais quando têm a mesma coluna e a mesma linha.
    /// </summary>
    /// <param name="obj">O objeto a comparar.</param>
    /// <returns><c>true</c> se <paramref name="obj"/> for um <see cref="Point"/> nas mesmas coordenadas.</returns>
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
    /// Informa se os dois pontos ocupam a mesma célula.
    /// </summary>
    /// <param name="ponto">O outro ponto.</param>
    /// <returns><c>true</c> se as coordenadas forem iguais.</returns>
    public bool ColideCom(Point ponto)
    {
        return Equals(ponto);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }
}
