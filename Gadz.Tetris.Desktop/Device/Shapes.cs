using System.Drawing.Drawing2D;

namespace Gadz.Tetris.Desktop.Device;

/// <summary>
/// Contornos arredondados usados na carcaça e nos botões.
/// </summary>
internal static class Shapes
{
    /// <summary>
    /// Retângulo com o mesmo raio em todos os cantos.
    /// </summary>
    /// <param name="area">A área.</param>
    /// <param name="radius">O raio dos cantos.</param>
    /// <returns>O contorno; quem chama o descarta.</returns>
    public static GraphicsPath RoundedRect(RectangleF area, float radius) => RoundedRect(area, radius, radius, radius, radius);

    /// <summary>
    /// Retângulo com um raio para cada canto (0 deixa o canto reto).
    /// </summary>
    /// <param name="area">A área.</param>
    /// <param name="topLeft">Raio do canto superior esquerdo.</param>
    /// <param name="topRight">Raio do canto superior direito.</param>
    /// <param name="bottomRight">Raio do canto inferior direito.</param>
    /// <param name="bottomLeft">Raio do canto inferior esquerdo.</param>
    /// <returns>O contorno; quem chama o descarta.</returns>
    public static GraphicsPath RoundedRect(RectangleF area, float topLeft, float topRight, float bottomRight, float bottomLeft)
    {
        var path = new GraphicsPath();
        AddCorner(path, area.X, area.Y, topLeft, 180);
        AddCorner(path, area.Right - (2 * topRight), area.Y, topRight, 270);
        AddCorner(path, area.Right - (2 * bottomRight), area.Bottom - (2 * bottomRight), bottomRight, 0);
        AddCorner(path, area.X, area.Bottom - (2 * bottomLeft), bottomLeft, 90);
        path.CloseFigure();
        return path;
    }

    /// <summary>
    /// Acrescenta um canto: um arco, ou só o ponto do vértice se o raio for 0.
    /// </summary>
    private static void AddCorner(GraphicsPath path, float x, float y, float radius, float startAngle)
    {
        // com raio 0, (x, y) já é o próprio vértice do canto
        if (radius <= 0)
        {
            path.AddLine(x, y, x, y);
            return;
        }

        path.AddArc(x, y, 2 * radius, 2 * radius, startAngle, 90);
    }
}
