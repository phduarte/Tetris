using Gadz.Tetris.Domain.Models.Pieces;

namespace Gadz.Tetris.Desktop.Device;

/// <summary>
/// A tela de LCD onde cada tela do jogo desenha: células, números em 7 segmentos e textos, sempre na "tinta" do estilo escolhido.
/// As coordenadas são em pixels a 96 DPI, com a origem no canto superior esquerdo do LCD.
/// </summary>
internal sealed class LcdCanvas : IDisposable
{
    /// <summary>
    /// Largura do LCD.
    /// </summary>
    public const float WIDTH = 350F;

    /// <summary>
    /// Altura do LCD.
    /// </summary>
    public const float HEIGHT = 382F;

    /// <summary>
    /// Margem interna do LCD.
    /// </summary>
    public const float PADDING = 12F;

    /// <summary>
    /// Tamanho dos rótulos (PONTOS, NÍVEL...).
    /// </summary>
    public const float LABEL_SIZE = 9F;

    /// <summary>
    /// O centro horizontal do LCD, para centralizar.
    /// </summary>
    public const float CENTER_X = WIDTH / 2;

    /// <summary>
    /// Espessura da borda de uma célula; o miolo fica a uma borda de distância dela.
    /// </summary>
    private const float CELL_BORDER = 2F;

    /// <summary>
    /// Espaçamento entre as letras dos rótulos (0,14 em).
    /// </summary>
    private const float LABEL_SPACING = LABEL_SIZE * 0.14F;

    /// <summary>
    /// Opacidade do relevo claro (topo e esquerda) de um bloco colorido.
    /// </summary>
    private const int BEVEL_LIGHT_ALPHA = 140;

    /// <summary>
    /// Opacidade do relevo escuro (base e direita) de um bloco colorido.
    /// </summary>
    private const int BEVEL_DARK_ALPHA = 102;

    /// <summary>
    /// Opacidade do brilho no miolo de um bloco colorido.
    /// </summary>
    private const int SHINE_ALPHA = 41;

    /// <summary>
    /// Multiplicador de uma fração para porcentagem.
    /// </summary>
    private const int PERCENT = 100;

    /// <summary>
    /// O símbolo ao lado das porcentagens.
    /// </summary>
    private const string PERCENT_SIGN = "%";

    /// <summary>
    /// Tamanho do símbolo de porcentagem.
    /// </summary>
    private const float UNIT_SIZE = 13F;

    /// <summary>
    /// Espaço entre os dígitos e o símbolo de porcentagem.
    /// </summary>
    private const float UNIT_GAP = 3F;

    /// <summary>
    /// Quanto o símbolo de porcentagem sobe para alinhar com a base dos dígitos.
    /// </summary>
    private const float UNIT_BASELINE = 2F;

    /// <summary>
    /// Altura máxima ao medir um parágrafo; com <see cref="float.MaxValue"/> o GDI+ mede só a primeira linha.
    /// </summary>
    private const float MAX_PARAGRAPH_HEIGHT = 10_000F;

    /// <summary>
    /// Pincel da tinta.
    /// </summary>
    private readonly SolidBrush _ink;

    /// <summary>
    /// Pincel das células e segmentos apagados.
    /// </summary>
    private readonly SolidBrush _ghost;

    /// <summary>
    /// Pincel do fundo do LCD.
    /// </summary>
    private readonly SolidBrush _background;

    /// <summary>
    /// Cria a tela sobre o <see cref="Graphics"/> já posicionado no canto do LCD.
    /// </summary>
    /// <param name="graphics">Onde desenhar.</param>
    /// <param name="theme">As cores do estilo escolhido.</param>
    /// <param name="blinkOn">Se os textos que piscam estão visíveis neste instante.</param>
    public LcdCanvas(Graphics graphics, LcdTheme theme, bool blinkOn)
    {
        Graphics = graphics;
        Theme = theme;
        BlinkOn = blinkOn;
        _ink = new SolidBrush(theme.Ink);
        _ghost = new SolidBrush(Color.FromArgb(theme.GhostAlpha, theme.Ink));
        _background = new SolidBrush(theme.Background);
    }

    /// <summary>
    /// Onde desenhar, para o que as primitivas daqui não cobrem.
    /// </summary>
    public Graphics Graphics { get; }

    /// <summary>
    /// As cores do estilo escolhido.
    /// </summary>
    public LcdTheme Theme { get; }

    /// <summary>
    /// Se os textos que piscam estão visíveis neste instante.
    /// </summary>
    public bool BlinkOn { get; }

    /// <summary>
    /// Pincel da tinta com a opacidade pedida. Quem chama descarta o pincel.
    /// </summary>
    /// <param name="opacity">De 0 a 1.</param>
    /// <returns>O pincel.</returns>
    public SolidBrush InkBrush(float opacity) => new(Color.FromArgb((int)(opacity * byte.MaxValue), Theme.Ink));

    /// <summary>
    /// Cobre a área com o fundo do LCD (para sobrepor mensagens ao tabuleiro).
    /// </summary>
    /// <param name="area">A área.</param>
    public void Clear(RectangleF area) => Graphics.FillRectangle(_background, area);

    /// <summary>
    /// Pinta a área com a tinta (linha destacada da ajuda, separadores).
    /// </summary>
    /// <param name="area">A área.</param>
    public void Fill(RectangleF area) => Graphics.FillRectangle(_ink, area);

    /// <summary>
    /// Desenha uma célula: acesa com a cor da peça (ou com a tinta, no estilo monocromático) ou apagada, se a cor for <see cref="PieceColor.None"/>.
    /// </summary>
    /// <param name="x">Esquerda.</param>
    /// <param name="y">Topo.</param>
    /// <param name="size">Lado da célula.</param>
    /// <param name="color">A cor da peça.</param>
    public void Cell(float x, float y, float size, PieceColor color)
    {
        var lit = color != PieceColor.None;

        if (lit && Theme.Colored)
        {
            DrawColoredBlock(new RectangleF(x, y, size, size), Palette.Of(color));
            return;
        }

        var brush = lit ? _ink : _ghost;
        var border = Math.Min(CELL_BORDER, size / 2);
        Graphics.FillRectangle(brush, x, y, size, border);
        Graphics.FillRectangle(brush, x, y + size - border, size, border);
        Graphics.FillRectangle(brush, x, y + border, border, size - (2 * border));
        Graphics.FillRectangle(brush, x + size - border, y + border, border, size - (2 * border));

        var inner = size - (4 * CELL_BORDER);
        if (inner > 0)
        {
            Graphics.FillRectangle(brush, x + (2 * CELL_BORDER), y + (2 * CELL_BORDER), inner, inner);
        }
    }

    /// <summary>
    /// Desenha uma grade de células.
    /// </summary>
    /// <param name="x">Esquerda.</param>
    /// <param name="y">Topo.</param>
    /// <param name="columns">Quantidade de colunas.</param>
    /// <param name="rows">Quantidade de linhas.</param>
    /// <param name="size">Lado de cada célula.</param>
    /// <param name="gap">Espaço entre as células.</param>
    /// <param name="colorAt">A cor da célula (coluna, linha).</param>
    public void Grid(float x, float y, int columns, int rows, float size, float gap, Func<int, int, PieceColor> colorAt)
    {
        for (var row = 0; row < rows; row++)
        {
            for (var column = 0; column < columns; column++)
            {
                Cell(x + (column * (size + gap)), y + (row * (size + gap)), size, colorAt(column, row));
            }
        }
    }

    /// <summary>
    /// Largura (ou altura) de uma grade de células.
    /// </summary>
    /// <param name="count">Quantidade de colunas (ou linhas).</param>
    /// <param name="size">Lado de cada célula.</param>
    /// <param name="gap">Espaço entre as células.</param>
    /// <returns>O tamanho.</returns>
    public static float GridSize(int count, float size, float gap) => (count * size) + ((count - 1) * gap);

    /// <summary>
    /// Desenha um número alinhado à direita em <paramref name="digits"/> posições de 7 segmentos; as que sobram ficam apagadas.
    /// </summary>
    /// <param name="value">O número.</param>
    /// <param name="digits">Quantidade de posições.</param>
    /// <param name="x">Esquerda.</param>
    /// <param name="y">Topo.</param>
    /// <param name="height">Altura dos dígitos.</param>
    /// <returns>A largura desenhada.</returns>
    public float Number(int value, int digits, float x, float y, float height) => Digits(NumberText(value, digits), x, y, height);

    /// <summary>
    /// Largura de um número em <paramref name="digits"/> posições de 7 segmentos.
    /// </summary>
    /// <param name="digits">Quantidade de posições.</param>
    /// <param name="height">Altura dos dígitos.</param>
    /// <returns>A largura.</returns>
    public static float MeasureNumber(int digits, float height) => SevenSegment.Measure(new string('8', digits), height);

    /// <summary>
    /// Desenha uma fração como porcentagem em 7 segmentos, com o símbolo "%" ao lado.
    /// </summary>
    /// <param name="fraction">A fração, de 0 a 1.</param>
    /// <param name="digits">Quantidade de posições.</param>
    /// <param name="x">Esquerda.</param>
    /// <param name="y">Topo.</param>
    /// <param name="height">Altura dos dígitos.</param>
    public void Percent(float fraction, int digits, float x, float y, float height)
    {
        var width = Number((int)Math.Round(fraction * PERCENT), digits, x, y, height);
        Text(PERCENT_SIGN, Fonts.Bold(UNIT_SIZE), x + width + UNIT_GAP, y + height - UNIT_SIZE - UNIT_BASELINE);
    }

    /// <summary>
    /// Largura de uma porcentagem desenhada por <see cref="Percent"/>.
    /// </summary>
    /// <param name="digits">Quantidade de posições.</param>
    /// <param name="height">Altura dos dígitos.</param>
    /// <returns>A largura.</returns>
    public float MeasurePercent(int digits, float height) => MeasureNumber(digits, height) + UNIT_GAP + MeasureText(PERCENT_SIGN, Fonts.Bold(UNIT_SIZE));

    /// <summary>
    /// Desenha um tempo como relógio de 7 segmentos (hh:mm:ss).
    /// </summary>
    /// <param name="duration">O tempo.</param>
    /// <param name="x">Esquerda.</param>
    /// <param name="y">Topo.</param>
    /// <param name="height">Altura dos dígitos.</param>
    /// <returns>A largura desenhada.</returns>
    public float Time(TimeSpan duration, float x, float y, float height) => Digits(duration.ToString(@"hh\:mm\:ss"), x, y, height);

    /// <summary>
    /// Desenha caracteres em 7 segmentos.
    /// </summary>
    /// <param name="text">Dígitos, espaços, "-" ou ":".</param>
    /// <param name="x">Esquerda.</param>
    /// <param name="y">Topo.</param>
    /// <param name="height">Altura dos dígitos.</param>
    /// <returns>A largura desenhada.</returns>
    public float Digits(string text, float x, float y, float height)
    {
        SevenSegment.Draw(Graphics, text, x, y, height, _ink, _ghost);
        return SevenSegment.Measure(text, height);
    }

    /// <summary>
    /// Desenha um rótulo pequeno em caixa alta (PONTOS, NÍVEL...).
    /// </summary>
    /// <param name="text">O texto.</param>
    /// <param name="x">Esquerda.</param>
    /// <param name="y">Topo.</param>
    public void Label(string text, float x, float y) => Text(text.ToUpperInvariant(), Fonts.Bold(LABEL_SIZE), x, y, LABEL_SPACING);

    /// <summary>
    /// Desenha um rótulo pequeno centralizado.
    /// </summary>
    /// <param name="text">O texto.</param>
    /// <param name="centerX">O centro horizontal.</param>
    /// <param name="y">Topo.</param>
    public void LabelCentered(string text, float centerX, float y)
        => TextCentered(text.ToUpperInvariant(), Fonts.Bold(LABEL_SIZE), centerX, y, LABEL_SPACING);

    /// <summary>
    /// Largura de um rótulo pequeno.
    /// </summary>
    /// <param name="text">O texto.</param>
    /// <returns>A largura.</returns>
    public float MeasureLabel(string text) => MeasureText(text.ToUpperInvariant(), Fonts.Bold(LABEL_SIZE), LABEL_SPACING);

    /// <summary>
    /// Desenha um texto numa linha, com espaçamento opcional entre as letras.
    /// </summary>
    /// <param name="content">O texto.</param>
    /// <param name="font">A fonte.</param>
    /// <param name="x">Esquerda.</param>
    /// <param name="y">Topo.</param>
    /// <param name="spacing">Espaço extra entre as letras.</param>
    /// <param name="brush">O pincel; se omitido, a tinta.</param>
    public void Text(string content, Font font, float x, float y, float spacing = 0, Brush? brush = null)
        => TextPainter.Draw(Graphics, content, font, brush ?? _ink, x, y, spacing);

    /// <summary>
    /// Desenha um texto numa linha, centralizado.
    /// </summary>
    /// <param name="text">O texto.</param>
    /// <param name="font">A fonte.</param>
    /// <param name="centerX">O centro horizontal.</param>
    /// <param name="y">Topo.</param>
    /// <param name="spacing">Espaço extra entre as letras.</param>
    /// <param name="brush">O pincel; se omitido, a tinta.</param>
    public void TextCentered(string text, Font font, float centerX, float y, float spacing = 0, Brush? brush = null)
        => TextPainter.DrawCentered(Graphics, text, font, brush ?? _ink, centerX, y, spacing);

    /// <summary>
    /// Largura de um texto numa linha.
    /// </summary>
    /// <param name="text">O texto.</param>
    /// <param name="font">A fonte.</param>
    /// <param name="spacing">Espaço extra entre as letras.</param>
    /// <returns>A largura.</returns>
    public float MeasureText(string text, Font font, float spacing = 0) => TextPainter.Measure(Graphics, text, font, spacing);

    /// <summary>
    /// Desenha um texto que quebra linhas dentro da área.
    /// </summary>
    /// <param name="text">O texto.</param>
    /// <param name="font">A fonte.</param>
    /// <param name="area">A área.</param>
    /// <param name="centered">Se as linhas são centralizadas.</param>
    /// <param name="brush">O pincel; se omitido, a tinta.</param>
    public void Paragraph(string text, Font font, RectangleF area, bool centered = false, Brush? brush = null)
    {
        using var format = new StringFormat(TextPainter.Typographic);
        format.Alignment = centered ? StringAlignment.Center : StringAlignment.Near;

        // com LineLimit, um arredondamento na altura medida basta para a última linha sumir
        format.FormatFlags &= ~StringFormatFlags.LineLimit;
        Graphics.DrawString(text, font, brush ?? _ink, area, format);
    }

    /// <summary>
    /// Altura de um texto que quebra linhas na largura informada.
    /// </summary>
    /// <param name="text">O texto.</param>
    /// <param name="font">A fonte.</param>
    /// <param name="width">A largura disponível.</param>
    /// <returns>A altura.</returns>
    public float MeasureParagraph(string text, Font font, float width)
        => Graphics.MeasureString(text, font, new SizeF(width, MAX_PARAGRAPH_HEIGHT), TextPainter.Typographic).Height;

    /// <summary>
    /// Desenha um triângulo apontando para a esquerda ou para a direita (as setas ◄ ►).
    /// </summary>
    /// <param name="x">Esquerda.</param>
    /// <param name="y">Topo.</param>
    /// <param name="size">Altura do triângulo.</param>
    /// <param name="right">Se aponta para a direita.</param>
    public void Arrow(float x, float y, float size, bool right)
    {
        var width = size * 0.8F;
        PointF[] points = right
            ? [new(x, y), new(x + width, y + (size / 2)), new(x, y + size)]
            : [new(x + width, y), new(x, y + (size / 2)), new(x + width, y + size)];
        Graphics.FillPolygon(_ink, points);
    }

    /// <summary>
    /// Desenha um retângulo vazado na tinta (selos e etiquetas).
    /// </summary>
    /// <param name="area">A área.</param>
    /// <param name="thickness">Espessura da borda.</param>
    public void Frame(RectangleF area, float thickness)
    {
        using var pen = new Pen(Theme.Ink, thickness);
        var half = thickness / 2;
        Graphics.DrawRectangle(pen, area.X + half, area.Y + half, area.Width - thickness, area.Height - thickness);
    }

    /// <summary>
    /// Descarta os pincéis.
    /// </summary>
    public void Dispose()
    {
        _ink.Dispose();
        _ghost.Dispose();
        _background.Dispose();
    }

    /// <summary>
    /// O número com espaços à esquerda até ocupar as posições; se não couber, ficam os dígitos finais.
    /// </summary>
    /// <param name="value">O número.</param>
    /// <param name="digits">Quantidade de posições.</param>
    /// <returns>O texto para o display.</returns>
    private static string NumberText(int value, int digits) => value.ToString().PadLeft(digits)[^digits..];

    /// <summary>
    /// Desenha um bloco colorido com relevo: claro em cima e à esquerda, escuro embaixo e à direita, e um brilho no miolo.
    /// </summary>
    /// <param name="cell">A área da célula.</param>
    /// <param name="color">A cor da peça.</param>
    private void DrawColoredBlock(RectangleF cell, Color color)
    {
        using var fill = new SolidBrush(color);
        using var light = new SolidBrush(Color.FromArgb(BEVEL_LIGHT_ALPHA, Color.White));
        using var dark = new SolidBrush(Color.FromArgb(BEVEL_DARK_ALPHA, Color.Black));
        using var shine = new SolidBrush(Color.FromArgb(SHINE_ALPHA, Color.White));

        Graphics.FillRectangle(fill, cell);
        Graphics.FillRectangle(light, cell.X, cell.Y, cell.Width, CELL_BORDER);
        Graphics.FillRectangle(light, cell.X, cell.Y + CELL_BORDER, CELL_BORDER, cell.Height - CELL_BORDER);
        Graphics.FillRectangle(dark, cell.X + CELL_BORDER, cell.Bottom - CELL_BORDER, cell.Width - CELL_BORDER, CELL_BORDER);
        Graphics.FillRectangle(dark, cell.Right - CELL_BORDER, cell.Y + CELL_BORDER, CELL_BORDER, cell.Height - (2 * CELL_BORDER));

        var inner = cell.Width - (4 * CELL_BORDER);
        if (inner > 0)
        {
            Graphics.FillRectangle(shine, cell.X + (2 * CELL_BORDER), cell.Y + (2 * CELL_BORDER), inner, inner);
        }
    }
}
