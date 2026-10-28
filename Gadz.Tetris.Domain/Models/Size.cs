namespace Gadz.Tetris.Domain.Models;

/// <summary>
/// Defines the <see cref="Size" />
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref=""/> class.
/// </remarks>
/// <param name="largura">The largura<see cref="int"/></param>
/// <param name="altura">The altura<see cref="int"/></param>
public struct Size(int largura, int altura)
{
    /// <summary>
    /// Defines the Height, Width
    /// </summary>
    public int Height = altura, Width = largura;

    /// <summary>
    /// The ToString
    /// </summary>
    /// <returns>The <see cref="string"/></returns>
    public override string ToString()
    {
        return $"{Width}:{Height}";
    }
}
