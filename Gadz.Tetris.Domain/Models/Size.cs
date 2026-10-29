namespace Gadz.Tetris.Domain.Models;

/// <summary>
/// Dimensões de uma área retangular, em células.
/// </summary>
/// <param name="largura">Quantidade de colunas.</param>
/// <param name="altura">Quantidade de linhas.</param>
public struct Size(int largura, int altura)
{
    /// <summary>
    /// Quantidade de linhas (<see cref="Height"/>) e de colunas (<see cref="Width"/>).
    /// </summary>
    public int Height = altura, Width = largura;

    /// <summary>
    /// As dimensões no formato <c>largura:altura</c>.
    /// </summary>
    /// <returns>Por exemplo, <c>10:20</c>.</returns>
    public override string ToString()
    {
        return $"{Width}:{Height}";
    }
}
