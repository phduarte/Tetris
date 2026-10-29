namespace Gadz.Tetris.Desktop.Device;

/// <summary>
/// Desenho de texto numa linha com espaçamento entre as letras, que o GDI+ não oferece.
/// As setas ◀ ▶ ▲ ▼ viram triângulos: nas fontes, cada uma vem de um fallback diferente e com um tamanho diferente.
/// </summary>
internal static class TextPainter
{
    /// <summary>
    /// As setas desenhadas como triângulos.
    /// </summary>
    private const string ARROWS = "◀▶▲▼";

    /// <summary>
    /// Lado do triângulo de uma seta, em relação ao tamanho da fonte.
    /// </summary>
    private const float ARROW_SCALE = 0.7F;

    /// <summary>
    /// Altura das maiúsculas, em relação ao tamanho da fonte, para centralizar as setas nelas.
    /// </summary>
    private const float CAP_HEIGHT = 0.7F;

    /// <summary>
    /// Formato sem as margens que o GDI+ acrescenta, para medir e posicionar com precisão; também mede os espaços no fim do texto.
    /// </summary>
    public static StringFormat Typographic { get; } = CreateTypographicFormat();

    /// <summary>
    /// Largura de um texto numa linha.
    /// </summary>
    /// <param name="g">Onde o texto será desenhado.</param>
    /// <param name="text">O texto.</param>
    /// <param name="font">A fonte.</param>
    /// <param name="spacing">Espaço extra entre as letras.</param>
    /// <returns>A largura.</returns>
    public static float Measure(Graphics g, string text, Font font, float spacing = 0)
    {
        if (IsPlain(text, spacing))
        {
            return g.MeasureString(text, font, PointF.Empty, Typographic).Width;
        }

        return text.Sum(c => MeasureChar(g, c, font)) + (spacing * Math.Max(0, text.Length - 1));
    }

    /// <summary>
    /// Desenha um texto numa linha a partir do canto superior esquerdo.
    /// </summary>
    /// <param name="g">Onde desenhar.</param>
    /// <param name="text">O texto.</param>
    /// <param name="font">A fonte.</param>
    /// <param name="brush">O pincel.</param>
    /// <param name="x">Esquerda.</param>
    /// <param name="y">Topo.</param>
    /// <param name="spacing">Espaço extra entre as letras.</param>
    public static void Draw(Graphics g, string text, Font font, Brush brush, float x, float y, float spacing = 0)
    {
        if (IsPlain(text, spacing))
        {
            g.DrawString(text, font, brush, x, y, Typographic);
            return;
        }

        foreach (var c in text)
        {
            if (ARROWS.Contains(c))
            {
                DrawArrow(g, c, font, brush, x, y);
            }
            else
            {
                g.DrawString(c.ToString(), font, brush, x, y, Typographic);
            }

            x += MeasureChar(g, c, font) + spacing;
        }
    }

    /// <summary>
    /// Desenha um texto numa linha, centralizado.
    /// </summary>
    /// <param name="g">Onde desenhar.</param>
    /// <param name="text">O texto.</param>
    /// <param name="font">A fonte.</param>
    /// <param name="brush">O pincel.</param>
    /// <param name="centerX">O centro horizontal.</param>
    /// <param name="y">Topo.</param>
    /// <param name="spacing">Espaço extra entre as letras.</param>
    public static void DrawCentered(Graphics g, string text, Font font, Brush brush, float centerX, float y, float spacing = 0)
        => Draw(g, text, font, brush, centerX - (Measure(g, text, font, spacing) / 2), y, spacing);

    /// <summary>
    /// Se o texto pode ser desenhado de uma vez: sem espaçamento extra e sem setas.
    /// </summary>
    private static bool IsPlain(string text, float spacing) => spacing <= 0 && text.IndexOfAny(ARROWS.ToCharArray()) < 0;

    /// <summary>
    /// Largura de um caractere; o espaço é medido pela diferença entre "a a" e "aa", porque sozinho o GDI+ o mede como zero.
    /// </summary>
    private static float MeasureChar(Graphics g, char c, Font font)
    {
        if (ARROWS.Contains(c))
        {
            return font.Size * ARROW_SCALE;
        }

        if (c == ' ')
        {
            return g.MeasureString("a a", font, PointF.Empty, Typographic).Width - g.MeasureString("aa", font, PointF.Empty, Typographic).Width;
        }

        return g.MeasureString(c.ToString(), font, PointF.Empty, Typographic).Width;
    }

    /// <summary>
    /// Desenha uma seta como um triângulo centralizado na altura das maiúsculas.
    /// </summary>
    private static void DrawArrow(Graphics g, char arrow, Font font, Brush brush, float x, float y)
    {
        var size = font.Size * ARROW_SCALE;
        var ascent = font.Size * font.FontFamily.GetCellAscent(font.Style) / font.FontFamily.GetEmHeight(font.Style);
        var top = y + ascent - (font.Size * CAP_HEIGHT / 2) - (size / 2);
        var middleX = x + (size / 2);
        var middleY = top + (size / 2);

        PointF[] points = arrow switch
        {
            '◀' => [new(x, middleY), new(x + size, top), new(x + size, top + size)],
            '▶' => [new(x, top), new(x + size, middleY), new(x, top + size)],
            '▲' => [new(middleX, top), new(x + size, top + size), new(x, top + size)],
            _ => [new(x, top), new(x + size, top), new(middleX, top + size)],
        };

        g.FillPolygon(brush, points);
    }

    /// <summary>
    /// Cria o formato tipográfico.
    /// </summary>
    /// <returns>O formato.</returns>
    private static StringFormat CreateTypographicFormat()
    {
        var format = new StringFormat(StringFormat.GenericTypographic);
        format.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;
        return format;
    }
}
