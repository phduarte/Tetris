using Gadz.Tetris.Application;
using Gadz.Tetris.Desktop.Device;
using Texto = Gadz.Tetris.Desktop.Textos.Jogo;

namespace Gadz.Tetris.Desktop.Screens;

/// <summary>
/// Como jogar: as teclas desenhadas como teclas, a tabela de pontos (lida do domínio) e a regra de nível.
/// Apertar uma tecla destaca a linha dela, para o jogador testar enquanto lê.
/// </summary>
internal sealed class HelpScreen : ConsoleScreen
{
    /// <summary>
    /// Margem interna da ajuda (um pouco maior que a do LCD).
    /// </summary>
    private const float PADDING = 14F;

    /// <summary>
    /// Tamanho do título.
    /// </summary>
    private const float HEAD_SIZE = 13F;

    /// <summary>
    /// Espaço entre o título e as teclas.
    /// </summary>
    private const float HEAD_GAP = 8F;

    /// <summary>
    /// Altura de uma linha de tecla.
    /// </summary>
    private const float ROW_HEIGHT = 21F;

    /// <summary>
    /// Espaço entre as linhas de tecla.
    /// </summary>
    private const float ROW_GAP = 3F;

    /// <summary>
    /// Margem horizontal dentro de uma linha.
    /// </summary>
    private const float ROW_PADDING = 5F;

    /// <summary>
    /// Largura da coluna das teclas.
    /// </summary>
    private const float KEYS_COLUMN = 116F;

    /// <summary>
    /// Tamanho do texto das linhas.
    /// </summary>
    private const float ROW_TEXT_SIZE = 11.5F;

    /// <summary>
    /// Tamanho do texto dentro de uma tecla.
    /// </summary>
    private const float KEY_TEXT_SIZE = 8F;

    /// <summary>
    /// Altura de uma tecla.
    /// </summary>
    private const float KEY_HEIGHT = 15F;

    /// <summary>
    /// Margem horizontal do texto dentro de uma tecla.
    /// </summary>
    private const float KEY_PADDING = 4F;

    /// <summary>
    /// Espaço entre as teclas de uma linha.
    /// </summary>
    private const float KEY_GAP = 3F;

    /// <summary>
    /// Espessura da borda de uma tecla (a de baixo é o dobro, como uma tecla de verdade).
    /// </summary>
    private const float KEY_BORDER = 1.5F;

    /// <summary>
    /// Espaço antes e depois do rótulo da tabela de pontos.
    /// </summary>
    private const float SECTION_GAP = 8F;

    /// <summary>
    /// Altura de cada linha da tabela de pontos.
    /// </summary>
    private const float POINTS_ROW = 14F;

    /// <summary>
    /// Tamanho do texto da tabela de pontos.
    /// </summary>
    private const float POINTS_SIZE = 10F;

    /// <summary>
    /// Tamanho do texto da regra de nível.
    /// </summary>
    private const float FOOT_SIZE = 10F;

    /// <summary>
    /// Opacidade da regra de nível.
    /// </summary>
    private const float FOOT_OPACITY = 0.8F;

    /// <summary>
    /// Tamanho do convite para testar.
    /// </summary>
    private const float TRY_SIZE = 9F;

    /// <summary>
    /// Espaço entre a regra de nível e o convite para testar.
    /// </summary>
    private const float TRY_GAP = 6F;

    /// <summary>
    /// Margem inferior da ajuda.
    /// </summary>
    private const float BOTTOM = 10F;

    /// <summary>
    /// Por quanto tempo, em milissegundos, a linha da tecla fica destacada.
    /// </summary>
    private const long HIGHLIGHT_MS = 450;

    /// <summary>
    /// As linhas da ajuda: as teclas desenhadas, o que fazem e as teclas que destacam a linha.
    /// </summary>
    private static readonly (string[] Caps, string Text, Keys[] Keys)[] _rows =
    [
        (["◀", "▶"], Texto.HelpMove, [Keys.Left, Keys.Right]),
        ([Texto.KeyCtrl, "◀", "▶"], Texto.HelpSlide, [Keys.ControlKey]),
        (["▼"], Texto.HelpSoftDrop, [Keys.Down]),
        (["▲"], Texto.HelpRotate, [Keys.Up]),
        ([Texto.KeySpace], Texto.HelpHardDrop, [Keys.Space]),
        ([Texto.KeyEnter], Texto.HelpPause, [Keys.Enter]),
        ([Texto.KeyShift], Texto.HelpSound, [Keys.ShiftKey]),
        ([Texto.KeyEsc], Texto.HelpQuit, [Keys.Escape]),
    ];

    /// <summary>
    /// Até quando (em <see cref="Environment.TickCount64"/>) cada linha fica destacada.
    /// </summary>
    private readonly Dictionary<int, long> _highlightUntil = [];

    /// <summary>
    /// Cria a ajuda.
    /// </summary>
    /// <param name="frame">O aparelho.</param>
    public HelpScreen(ConsoleFrame frame) : base(frame)
    {
        KeyBindings = new Dictionary<Keys, Action>
        {
            [Keys.Escape] = () => Frame.Navigate(new MenuScreen(Frame)),
            [Keys.ShiftKey] = ToggleSound,
        };

        Layout = new ConsoleLayout(
            new DpadSetup(true, true, Texto.CapTest, Texto.KeyAny),
            [SoundButton],
            new ConsoleButton(Keys.Escape, Texto.CapBack, Texto.KeyEsc));
    }

    /// <inheritdoc/>
    public override ConsoleLayout Layout { get; }

    /// <summary>
    /// Enquanto houver linha destacada, a tela é redesenhada (inclusive uma última vez, para apagar o destaque).
    /// </summary>
    public override bool IsAnimating => _highlightUntil.Count > 0;

    /// <inheritdoc/>
    protected override IReadOnlyDictionary<Keys, Action> KeyBindings { get; }

    /// <summary>
    /// Destaca a linha da tecla e executa a ação dela nesta tela (Esc volta, Shift liga/desliga o som).
    /// </summary>
    /// <param name="key">A tecla.</param>
    /// <param name="control">Se o Ctrl estava apertado.</param>
    /// <param name="repeat">Se é uma repetição da tecla segurada (a linha continua destacada).</param>
    /// <returns><c>true</c> se a tecla aparece na ajuda.</returns>
    public override bool HandleKey(Keys key, bool control, bool repeat)
    {
        var until = Environment.TickCount64 + HIGHLIGHT_MS;
        var matched = false;

        for (var i = 0; i < _rows.Length; i++)
        {
            if (_rows[i].Keys.Contains(key))
            {
                _highlightUntil[i] = until;
                matched = true;
            }
        }

        return base.HandleKey(key, control, repeat) || matched;
    }

    /// <inheritdoc/>
    public override void Paint(LcdCanvas lcd)
    {
        var now = Environment.TickCount64;
        foreach (var expired in _highlightUntil.Where(h => h.Value <= now).Select(h => h.Key).ToList())
        {
            _highlightUntil.Remove(expired);
        }

        var left = PADDING;
        var width = LcdCanvas.WIDTH - (2 * PADDING);
        var y = PADDING;

        lcd.Text(Texto.HowToPlay.ToUpperInvariant(), Fonts.Pixel(HEAD_SIZE), left, y);
        y += HEAD_SIZE + HEAD_GAP;

        for (var i = 0; i < _rows.Length; i++)
        {
            PaintRow(lcd, _rows[i].Caps, _rows[i].Text, new RectangleF(left, y, width, ROW_HEIGHT), _highlightUntil.ContainsKey(i));
            y += ROW_HEIGHT + ROW_GAP;
        }

        y += SECTION_GAP;
        lcd.Label(Texto.PointsPerDrop, left, y);
        y += LcdCanvas.LABEL_SIZE + SECTION_GAP;
        PaintScoreTable(lcd, left + ROW_PADDING, left + width - ROW_PADDING, y);

        PaintFoot(lcd, left, width);
    }

    /// <summary>
    /// Desenha uma linha: as teclas à esquerda e o que fazem à direita; destacada, inverte as cores.
    /// </summary>
    private static void PaintRow(LcdCanvas lcd, string[] caps, string text, RectangleF area, bool highlighted)
    {
        using var lcdColor = new SolidBrush(lcd.Theme.Background);
        using var ink = lcd.InkBrush(1);
        var brush = highlighted ? lcdColor : ink;

        if (highlighted)
        {
            lcd.Fill(area);
        }

        var x = area.X + ROW_PADDING;
        var capFont = Fonts.Pixel(KEY_TEXT_SIZE);
        var capTop = area.Y + ((area.Height - KEY_HEIGHT) / 2);

        foreach (var cap in caps)
        {
            var label = cap.ToUpperInvariant();
            var capWidth = lcd.MeasureText(label, capFont) + (2 * KEY_PADDING);
            PaintKeyCap(lcd, new RectangleF(x, capTop, capWidth, KEY_HEIGHT), highlighted ? lcd.Theme.Background : lcd.Theme.Ink);
            lcd.Text(label, capFont, x + KEY_PADDING, capTop + ((KEY_HEIGHT - KEY_TEXT_SIZE) / 2) - KEY_BORDER, brush: brush);
            x += capWidth + KEY_GAP;
        }

        var textFont = Fonts.Pixel(ROW_TEXT_SIZE);
        lcd.Text(text.ToUpperInvariant(), textFont, area.X + KEYS_COLUMN, area.Y + ((area.Height - ROW_TEXT_SIZE) / 2), brush: brush);
    }

    /// <summary>
    /// Desenha o contorno de uma tecla, com a borda de baixo mais grossa.
    /// </summary>
    private static void PaintKeyCap(LcdCanvas lcd, RectangleF area, Color color)
    {
        using var pen = new Pen(color, KEY_BORDER);
        using var path = Shapes.RoundedRect(new RectangleF(area.X, area.Y, area.Width, area.Height - KEY_BORDER), KEY_BORDER * 2);
        lcd.Graphics.DrawPath(pen, path);
        lcd.Graphics.DrawLine(pen, area.X + KEY_BORDER, area.Bottom - KEY_BORDER, area.Right - KEY_BORDER, area.Bottom - KEY_BORDER);
    }

    /// <summary>
    /// Desenha a tabela de pontos por jogada, com os valores do domínio.
    /// </summary>
    private static void PaintScoreTable(LcdCanvas lcd, float left, float right, float top)
    {
        var font = Fonts.Pixel(POINTS_SIZE);
        var table = GameController.ScoreTable;

        for (var i = 0; i < table.Count; i++)
        {
            var lines = i + 1;
            var format = lines == 1 ? Texto.OneLine : lines == table.Count ? Texto.TetrisLines : Texto.ManyLines;
            var points = table[i].ToString();

            lcd.Text(string.Format(format, lines).ToUpperInvariant(), font, left, top);
            lcd.Text(points, font, right - lcd.MeasureText(points, font), top);
            top += POINTS_ROW;
        }
    }

    /// <summary>
    /// Desenha a regra de nível e o convite para testar, no rodapé.
    /// </summary>
    private static void PaintFoot(LcdCanvas lcd, float left, float width)
    {
        var tryTop = LcdCanvas.HEIGHT - BOTTOM - TRY_SIZE;
        if (lcd.BlinkOn)
        {
            lcd.TextCentered(Texto.PressKeyToTest.ToUpperInvariant(), Fonts.Pixel(TRY_SIZE), LcdCanvas.CENTER_X, tryTop);
        }

        var font = Fonts.Regular(FOOT_SIZE);
        var textWidth = width - (2 * ROW_PADDING);
        var height = lcd.MeasureParagraph(Texto.LevelRule, font, textWidth);
        using var faded = lcd.InkBrush(FOOT_OPACITY);
        lcd.Paragraph(Texto.LevelRule, font, new RectangleF(left + ROW_PADDING, tryTop - TRY_GAP - height, textWidth, height), brush: faded);
    }
}
