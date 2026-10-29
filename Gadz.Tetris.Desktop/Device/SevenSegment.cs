namespace Gadz.Tetris.Desktop.Device;

/// <summary>
/// Desenho de números em display de 7 segmentos, com os segmentos apagados ainda visíveis (o "8" fantasma de um LCD real).
/// A geometria é a de um dígito de 12×22 unidades, escalado para a altura pedida e inclinado como num relógio digital.
/// </summary>
internal static class SevenSegment
{
    /// <summary>
    /// Largura de um dígito, em unidades da geometria.
    /// </summary>
    private const float DIGIT_WIDTH = 12F;

    /// <summary>
    /// Largura dos dois-pontos, em unidades da geometria.
    /// </summary>
    private const float COLON_WIDTH = 4F;

    /// <summary>
    /// Altura de um dígito, em unidades da geometria.
    /// </summary>
    private const float DIGIT_HEIGHT = 22F;

    /// <summary>
    /// Espaço entre os dígitos, em pixels.
    /// </summary>
    private const float GAP = 2F;

    /// <summary>
    /// Inclinação dos dígitos (tangente de 6°), da base para o topo.
    /// </summary>
    private const float SLANT = 0.105F;

    /// <summary>
    /// Os segmentos acesos de cada caractere (a–g, no padrão dos displays).
    /// </summary>
    private static readonly Dictionary<char, string> _lit = new()
    {
        ['0'] = "abcdef",
        ['1'] = "bc",
        ['2'] = "abged",
        ['3'] = "abgcd",
        ['4'] = "fgbc",
        ['5'] = "afgcd",
        ['6'] = "afgedc",
        ['7'] = "abc",
        ['8'] = "abcdefg",
        ['9'] = "abcdfg",
        ['-'] = "g",
        [' '] = "",
    };

    /// <summary>
    /// O contorno de cada segmento, em unidades da geometria.
    /// </summary>
    private static readonly Dictionary<char, PointF[]> _segments = new()
    {
        ['a'] = Horizontal(2F, 1.5F),
        ['g'] = Horizontal(2F, 11F),
        ['d'] = Horizontal(2F, 20.5F),
        ['f'] = Vertical(1.5F, 2F),
        ['b'] = Vertical(10.5F, 2F),
        ['e'] = Vertical(1.5F, 11.5F),
        ['c'] = Vertical(10.5F, 11.5F),
    };

    /// <summary>
    /// A largura do texto no display, para alinhar.
    /// </summary>
    /// <param name="text">Os caracteres (dígitos, espaço, "-" ou ":").</param>
    /// <param name="height">A altura do display, em pixels.</param>
    /// <returns>A largura em pixels.</returns>
    public static float Measure(string text, float height)
    {
        var scale = height / DIGIT_HEIGHT;
        return text.Sum(c => (c == ':' ? COLON_WIDTH : DIGIT_WIDTH) * scale) + (GAP * Math.Max(0, text.Length - 1));
    }

    /// <summary>
    /// Desenha o texto no display a partir do canto superior esquerdo.
    /// </summary>
    /// <param name="g">Onde desenhar.</param>
    /// <param name="text">Os caracteres (dígitos, espaço, "-" ou ":").</param>
    /// <param name="x">Esquerda, em pixels.</param>
    /// <param name="y">Topo, em pixels.</param>
    /// <param name="height">Altura, em pixels.</param>
    /// <param name="lit">Pincel dos segmentos acesos.</param>
    /// <param name="ghost">Pincel dos segmentos apagados.</param>
    public static void Draw(Graphics g, string text, float x, float y, float height, Brush lit, Brush ghost)
    {
        var scale = height / DIGIT_HEIGHT;
        var middle = y + (height / 2);

        foreach (var c in text)
        {
            if (c == ':')
            {
                DrawColon(g, x, y, scale, middle, lit);
                x += (COLON_WIDTH * scale) + GAP;
                continue;
            }

            var on = _lit.GetValueOrDefault(c, string.Empty);

            foreach (var (name, outline) in _segments)
            {
                var points = outline.Select(p => Place(p, x, y, scale, middle)).ToArray();
                g.FillPolygon(on.Contains(name) ? lit : ghost, points);
            }

            x += (DIGIT_WIDTH * scale) + GAP;
        }
    }

    /// <summary>
    /// Desenha os dois-pontos do relógio, sempre acesos.
    /// </summary>
    private static void DrawColon(Graphics g, float x, float y, float scale, float middle, Brush lit)
    {
        foreach (var top in new[] { 6F, 14F })
        {
            var points = new[] { new PointF(1, top), new PointF(3, top), new PointF(3, top + 2), new PointF(1, top + 2) };
            g.FillPolygon(lit, points.Select(p => Place(p, x, y, scale, middle)).ToArray());
        }
    }

    /// <summary>
    /// Leva um ponto da geometria para a tela: escala, posiciona e inclina em torno do meio da altura.
    /// </summary>
    private static PointF Place(PointF point, float x, float y, float scale, float middle)
    {
        var top = y + (point.Y * scale);
        return new PointF(x + (point.X * scale) + ((middle - top) * SLANT), top);
    }

    /// <summary>
    /// Segmento horizontal (hexágono deitado) de 8 unidades começando em (x, y).
    /// </summary>
    private static PointF[] Horizontal(float x, float y) =>
        [new(x, y), new(x + 1, y - 1), new(x + 7, y - 1), new(x + 8, y), new(x + 7, y + 1), new(x + 1, y + 1)];

    /// <summary>
    /// Segmento vertical (hexágono em pé) de 8,5 unidades começando em (x, y).
    /// </summary>
    private static PointF[] Vertical(float x, float y) =>
        [new(x, y), new(x + 1, y + 1), new(x + 1, y + 7.5F), new(x, y + 8.5F), new(x - 1, y + 7.5F), new(x - 1, y + 1)];
}
