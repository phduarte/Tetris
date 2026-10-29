using Gadz.Tetris.Application;
using Gadz.Tetris.Desktop.Device;
using Texto = Gadz.Tetris.Texts.Jogo;

namespace Gadz.Tetris.Desktop.Screens;

/// <summary>
/// Resultado da partida: pontos em destaque, o selo de NOVO RECORDE (decidido pelo domínio) e uma grade com o resto.
/// </summary>
internal sealed class ResultScreen : ConsoleScreen
{
    /// <summary>
    /// Topo do título.
    /// </summary>
    private const float HEAD_TOP = 18F;

    /// <summary>
    /// Tamanho do título.
    /// </summary>
    private const float HEAD_SIZE = 15F;

    /// <summary>
    /// Espaço vertical entre os blocos da tela.
    /// </summary>
    private const float GAP = 12F;

    /// <summary>
    /// Tamanho do texto do selo de recorde.
    /// </summary>
    private const float BADGE_SIZE = 9F;

    /// <summary>
    /// Margem horizontal do texto dentro do selo.
    /// </summary>
    private const float BADGE_PADDING_X = 7F;

    /// <summary>
    /// Margem vertical do texto dentro do selo.
    /// </summary>
    private const float BADGE_PADDING_Y = 4F;

    /// <summary>
    /// Espessura da borda do selo.
    /// </summary>
    private const float BADGE_BORDER = 2F;

    /// <summary>
    /// Altura dos dígitos da pontuação.
    /// </summary>
    private const float SCORE_HEIGHT = 36F;

    /// <summary>
    /// Posições dos displays de pontuação.
    /// </summary>
    private const int SCORE_DIGITS = 6;

    /// <summary>
    /// Altura dos dígitos da grade.
    /// </summary>
    private const float STAT_HEIGHT = 20F;

    /// <summary>
    /// Altura dos dígitos do tempo.
    /// </summary>
    private const float TIME_HEIGHT = 17F;

    /// <summary>
    /// Espaço entre o rótulo e o valor.
    /// </summary>
    private const float LABEL_GAP = 5F;

    /// <summary>
    /// Margem lateral da grade.
    /// </summary>
    private const float GRID_MARGIN = 34F;

    /// <summary>
    /// Posições do display do nível.
    /// </summary>
    private const int LEVEL_DIGITS = 2;

    /// <summary>
    /// Posições dos displays de linhas e de peças.
    /// </summary>
    private const int COUNT_DIGITS = 4;

    /// <summary>
    /// Posições do display da % de TETRIS.
    /// </summary>
    private const int RATE_DIGITS = 3;

    /// <summary>
    /// Distância entre o rodapé e a base do LCD.
    /// </summary>
    private const float FOOT_BOTTOM = 16F;

    /// <summary>
    /// Por quanto tempo, no máximo, o resultado fica na tela antes de voltar sozinho ao menu.
    /// </summary>
    private const int TIMEOUT_MS = 15_000;

    /// <summary>
    /// Tamanho da instrução no rodapé.
    /// </summary>
    private const float FOOT_SIZE = 11F;

    /// <summary>
    /// As estatísticas da partida.
    /// </summary>
    private readonly StatsSnapshot _stats;

    /// <summary>
    /// Se a partida bateu o recorde.
    /// </summary>
    private readonly bool _isNewRecord;

    /// <summary>
    /// Volta ao menu se o jogador não sair do resultado. Timer de WinForms: dispara na thread de UI.
    /// </summary>
    private readonly System.Windows.Forms.Timer _timeout = new() { Interval = TIMEOUT_MS };

    /// <summary>
    /// Cria o resultado.
    /// </summary>
    /// <param name="frame">O aparelho.</param>
    /// <param name="stats">As estatísticas da partida.</param>
    /// <param name="isNewRecord">Se a partida bateu o recorde, segundo o domínio.</param>
    public ResultScreen(ConsoleFrame frame, StatsSnapshot stats, bool isNewRecord) : base(frame)
    {
        _stats = stats;
        _isNewRecord = isNewRecord;

        KeyBindings = new Dictionary<Keys, Action>
        {
            [Keys.Enter] = BackToMenu,
            [Keys.Escape] = BackToMenu,
            [Keys.ShiftKey] = ToggleSound,
        };

        Layout = new ConsoleLayout(
            new DpadSetup(false, false, string.Empty),
            [SoundButton],
            new ConsoleButton(Keys.Enter, Texto.CapDone, Texto.KeyEnter));
    }

    /// <inheritdoc/>
    public override ConsoleLayout Layout { get; }

    /// <inheritdoc/>
    protected override IReadOnlyDictionary<Keys, Action> KeyBindings { get; }

    /// <inheritdoc/>
    public override void Paint(LcdCanvas lcd)
    {
        var center = LcdCanvas.CENTER_X;
        var y = HEAD_TOP;

        lcd.TextCentered(Texto.Result.ToUpperInvariant(), Fonts.Pixel(HEAD_SIZE), center, y);
        y += HEAD_SIZE + GAP;

        // o selo ocupa o espaço mesmo quando escondido, para a tela não "pular" entre partidas
        var badgeHeight = BADGE_SIZE + (2 * BADGE_PADDING_Y);
        if (_isNewRecord && lcd.BlinkOn)
        {
            var text = Texto.NewRecord.ToUpperInvariant();
            var font = Fonts.Pixel(BADGE_SIZE);
            var width = lcd.MeasureText(text, font) + (2 * BADGE_PADDING_X);
            lcd.Frame(new RectangleF(center - (width / 2), y, width, badgeHeight), BADGE_BORDER);
            lcd.TextCentered(text, font, center, y + BADGE_PADDING_Y);
        }

        y += badgeHeight + GAP;

        lcd.LabelCentered(Texto.Pontuacao, center, y);
        y += LcdCanvas.LABEL_SIZE + LABEL_GAP;
        lcd.Number(_stats.Score, SCORE_DIGITS, center - (LcdCanvas.MeasureNumber(SCORE_DIGITS, SCORE_HEIGHT) / 2), y, SCORE_HEIGHT);
        y += SCORE_HEIGHT + (2 * GAP);

        var left = GRID_MARGIN;
        var middle = LcdCanvas.CENTER_X + (GAP / 2);
        var rowHeight = LcdCanvas.LABEL_SIZE + LABEL_GAP + STAT_HEIGHT + GAP;

        PaintStat(lcd, Texto.Nivel, left, y, (x, top) => lcd.Number(_stats.Level, LEVEL_DIGITS, x, top, STAT_HEIGHT));
        PaintStat(lcd, Texto.Linhas, middle, y, (x, top) => lcd.Number(_stats.Lines, COUNT_DIGITS, x, top, STAT_HEIGHT));
        y += rowHeight;

        PaintStat(lcd, Texto.Pieces, left, y, (x, top) => lcd.Number(_stats.Pieces, COUNT_DIGITS, x, top, STAT_HEIGHT));
        PaintStat(lcd, Texto.TaxaTetris, middle, y, (x, top) => lcd.Percent(_stats.TetrisRate, RATE_DIGITS, x, top, STAT_HEIGHT));
        y += rowHeight;

        PaintStat(lcd, Texto.Tempo, left, y, (x, top) => lcd.Time(_stats.Duration, x, top, TIME_HEIGHT));

        if (lcd.BlinkOn)
        {
            lcd.TextCentered(Texto.EnterDone.ToUpperInvariant(), Fonts.Pixel(FOOT_SIZE), center, LcdCanvas.HEIGHT - FOOT_BOTTOM - FOOT_SIZE);
        }
    }

    /// <summary>
    /// Desenha um item da grade: rótulo e valor.
    /// </summary>
    private static void PaintStat(LcdCanvas lcd, string label, float left, float top, Action<float, float> paintValue)
    {
        lcd.Label(label, left, top);
        paintValue(left, top + LcdCanvas.LABEL_SIZE + LABEL_GAP);
    }

    /// <summary>
    /// Volta ao menu, que já mostra o recorde atualizado.
    /// </summary>
    private void BackToMenu() => Frame.Navigate(new MenuScreen(Frame));

    /// <summary>
    /// Começa a contar o tempo máximo na tela.
    /// </summary>
    public override void Enter()
    {
        _timeout.Tick += (_, _) => BackToMenu();
        _timeout.Start();
    }

    /// <summary>
    /// Descarta o timer, para ele não disparar depois que a tela saiu (pela tecla ou pelo próprio tempo).
    /// </summary>
    public override void Leave() => _timeout.Dispose();
}
