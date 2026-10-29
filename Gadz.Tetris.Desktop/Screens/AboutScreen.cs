using Gadz.Tetris.Desktop.Device;
using Texto = Gadz.Tetris.Texts.Jogo;

namespace Gadz.Tetris.Desktop.Screens;

/// <summary>
/// Sobre: o título em blocos, o propósito do projeto numa frase, os temas do estudo como etiquetas
/// e a licença num quadro que rola com ▲ ▼.
/// </summary>
internal sealed class AboutScreen : ConsoleScreen
{
    /// <summary>
    /// Topo do título.
    /// </summary>
    private const float TITLE_TOP = 18F;

    /// <summary>
    /// Lado das células do título.
    /// </summary>
    private const float TITLE_CELL = 7F;

    /// <summary>
    /// Espaço entre as células do título.
    /// </summary>
    private const float CELL_GAP = 2F;

    /// <summary>
    /// Espaço vertical entre os blocos da tela.
    /// </summary>
    private const float GAP = 9F;

    /// <summary>
    /// Tamanho da assinatura.
    /// </summary>
    private const float BY_SIZE = 9F;

    /// <summary>
    /// Espaço entre as letras da assinatura (0,3 em).
    /// </summary>
    private const float BY_SPACING = BY_SIZE * 0.3F;

    /// <summary>
    /// Opacidade da assinatura.
    /// </summary>
    private const float BY_OPACITY = 0.75F;

    /// <summary>
    /// Tamanho do texto de propósito.
    /// </summary>
    private const float TEXT_SIZE = 11.5F;

    /// <summary>
    /// Margem lateral do texto de propósito.
    /// </summary>
    private const float TEXT_MARGIN = 26F;

    /// <summary>
    /// Tamanho do texto das etiquetas.
    /// </summary>
    private const float TAG_SIZE = 8F;

    /// <summary>
    /// Margem horizontal do texto dentro de uma etiqueta.
    /// </summary>
    private const float TAG_PADDING_X = 6F;

    /// <summary>
    /// Margem vertical do texto dentro de uma etiqueta.
    /// </summary>
    private const float TAG_PADDING_Y = 3F;

    /// <summary>
    /// Espessura da borda de uma etiqueta.
    /// </summary>
    private const float TAG_BORDER = 1.5F;

    /// <summary>
    /// Espaço entre as etiquetas.
    /// </summary>
    private const float TAG_GAP = 5F;

    /// <summary>
    /// Separador das etiquetas no recurso de texto.
    /// </summary>
    private const char TAG_SEPARATOR = '|';

    /// <summary>
    /// Margem lateral do quadro da licença.
    /// </summary>
    private const float LICENSE_MARGIN = 16F;

    /// <summary>
    /// Espaço entre o rótulo e o quadro da licença.
    /// </summary>
    private const float LICENSE_LABEL_GAP = 7F;

    /// <summary>
    /// Tamanho do texto da licença.
    /// </summary>
    private const float LICENSE_SIZE = 9.5F;

    /// <summary>
    /// Espaço entre o texto da licença e a barra de rolagem.
    /// </summary>
    private const float SCROLLBAR_GAP = 12F;

    /// <summary>
    /// Largura da barra de rolagem.
    /// </summary>
    private const float SCROLLBAR_WIDTH = 4F;

    /// <summary>
    /// Altura do indicador da barra de rolagem.
    /// </summary>
    private const float THUMB_HEIGHT = 30F;

    /// <summary>
    /// Opacidade do trilho da barra de rolagem.
    /// </summary>
    private const float TRACK_OPACITY = 0.15F;

    /// <summary>
    /// Quanto a licença rola a cada ▲ ▼.
    /// </summary>
    private const float SCROLL_STEP = 40F;

    /// <summary>
    /// Os indicadores de rolagem ao lado do rótulo da licença.
    /// </summary>
    private const string SCROLL_HINT = "▲▼";

    /// <summary>
    /// Quanto a licença está rolada, em pixels.
    /// </summary>
    private float _scroll;

    /// <summary>
    /// Cria a tela.
    /// </summary>
    /// <param name="frame">O aparelho.</param>
    public AboutScreen(ConsoleFrame frame) : base(frame)
    {
        KeyBindings = new Dictionary<Keys, Action>
        {
            [Keys.Up] = () => Scroll(-SCROLL_STEP),
            [Keys.Down] = () => Scroll(SCROLL_STEP),
            [Keys.Escape] = () => Frame.Navigate(new MenuScreen(Frame)),
            [Keys.ShiftKey] = ToggleSound,
        };

        Layout = new ConsoleLayout(
            new DpadSetup(false, true, Texto.CapScroll, Texto.CapLicense),
            [SoundButton],
            new ConsoleButton(Keys.Escape, Texto.CapBack, Texto.KeyEsc));
    }

    /// <inheritdoc/>
    public override ConsoleLayout Layout { get; }

    /// <inheritdoc/>
    protected override IReadOnlyDictionary<Keys, Action> KeyBindings { get; }

    /// <inheritdoc/>
    public override void Paint(LcdCanvas lcd)
    {
        var center = LcdCanvas.CENTER_X;
        var y = TITLE_TOP;

        TitleArt.Draw(lcd, center, y, TITLE_CELL, CELL_GAP);
        y += TitleArt.Height(TITLE_CELL, CELL_GAP) + GAP;

        using (var faded = lcd.InkBrush(BY_OPACITY))
        {
            lcd.TextCentered(Texto.By.ToUpperInvariant(), Fonts.Pixel(BY_SIZE), center, y, BY_SPACING, faded);
        }

        y += BY_SIZE + GAP;

        var textFont = Fonts.Regular(TEXT_SIZE);
        var textWidth = LcdCanvas.WIDTH - (2 * TEXT_MARGIN);
        var textHeight = lcd.MeasureParagraph(Texto.AboutText, textFont, textWidth);
        lcd.Paragraph(Texto.AboutText, textFont, new RectangleF(TEXT_MARGIN, y, textWidth, textHeight), centered: true);
        y += textHeight + GAP;

        y = PaintTags(lcd, center, y) + GAP;
        PaintLicense(lcd, y);
    }

    /// <summary>
    /// Desenha as etiquetas dos temas do estudo, centralizadas numa linha.
    /// </summary>
    /// <returns>A base das etiquetas.</returns>
    private static float PaintTags(LcdCanvas lcd, float center, float top)
    {
        var font = Fonts.Pixel(TAG_SIZE);
        var tags = Texto.Tags.Split(TAG_SEPARATOR).Select(t => t.ToUpperInvariant()).ToList();
        var widths = tags.Select(t => lcd.MeasureText(t, font) + (2 * TAG_PADDING_X)).ToList();
        var x = center - ((widths.Sum() + (TAG_GAP * (tags.Count - 1))) / 2);
        var height = TAG_SIZE + (2 * TAG_PADDING_Y);

        for (var i = 0; i < tags.Count; i++)
        {
            lcd.Frame(new RectangleF(x, top, widths[i], height), TAG_BORDER);
            lcd.Text(tags[i], font, x + TAG_PADDING_X, top + TAG_PADDING_Y);
            x += widths[i] + TAG_GAP;
        }

        return top + height;
    }

    /// <summary>
    /// Desenha a licença: o rótulo, o texto rolado dentro do quadro e a barra de rolagem.
    /// </summary>
    private void PaintLicense(LcdCanvas lcd, float top)
    {
        var left = LICENSE_MARGIN;
        var right = LcdCanvas.WIDTH - LICENSE_MARGIN;

        lcd.Label(Texto.MitLicense, left, top);
        lcd.Label(SCROLL_HINT, right - lcd.MeasureLabel(SCROLL_HINT), top);

        var box = new RectangleF(left, top + LcdCanvas.LABEL_SIZE + LICENSE_LABEL_GAP, right - left - SCROLLBAR_GAP, 0);
        box.Height = LcdCanvas.HEIGHT - LcdCanvas.PADDING - box.Y;

        var font = Fonts.Regular(LICENSE_SIZE);
        var contentHeight = lcd.MeasureParagraph(Texto.LicenseText, font, box.Width);
        var maxScroll = Math.Max(0, contentHeight - box.Height);
        _scroll = Math.Clamp(_scroll, 0, maxScroll);

        var state = lcd.Graphics.Save();
        lcd.Graphics.IntersectClip(box);
        lcd.Paragraph(Texto.LicenseText, font, new RectangleF(box.X, box.Y - _scroll, box.Width, contentHeight));
        lcd.Graphics.Restore(state);

        var track = new RectangleF(right - SCROLLBAR_WIDTH, box.Y, SCROLLBAR_WIDTH, box.Height);
        using (var trackBrush = lcd.InkBrush(TRACK_OPACITY))
        {
            lcd.Graphics.FillRectangle(trackBrush, track);
        }

        var progress = maxScroll > 0 ? _scroll / maxScroll : 0;
        lcd.Fill(new RectangleF(track.X, track.Y + (progress * (track.Height - THUMB_HEIGHT)), SCROLLBAR_WIDTH, THUMB_HEIGHT));
    }

    /// <summary>
    /// Rola a licença; o limite é aplicado na pintura, que conhece a altura do texto.
    /// </summary>
    /// <param name="delta">Quanto rolar (negativo sobe).</param>
    private void Scroll(float delta)
    {
        _scroll = Math.Max(0, _scroll + delta);
        Frame.Invalidate();
    }
}
