using Gadz.Tetris.Application;
using Gadz.Tetris.Desktop.Device;
using Gadz.Tetris.Domain.Models.Pieces;
using Texto = Gadz.Tetris.Desktop.Textos.Jogo;

namespace Gadz.Tetris.Desktop.Screens;

/// <summary>
/// Menu inicial: o recorde como convite, o título em blocos, a escolha do estilo (◄ ►) e o som.
/// Enter começa a partida; AJUDA e SOBRE são botões da carcaça.
/// </summary>
internal sealed class MenuScreen : ConsoleScreen
{
    /// <summary>
    /// Altura dos dígitos do recorde.
    /// </summary>
    private const float RECORD_HEIGHT = 18F;

    /// <summary>
    /// Posições do display do recorde.
    /// </summary>
    private const int RECORD_DIGITS = 6;

    /// <summary>
    /// Topo do título.
    /// </summary>
    private const float TITLE_TOP = 56F;

    /// <summary>
    /// Lado das células do título.
    /// </summary>
    private const float TITLE_CELL = 12F;

    /// <summary>
    /// Espaço entre as células.
    /// </summary>
    private const float CELL_GAP = 2F;

    /// <summary>
    /// Espaço entre o título e o subtítulo.
    /// </summary>
    private const float SUBTITLE_GAP = 10F;

    /// <summary>
    /// Tamanho do subtítulo.
    /// </summary>
    private const float SUBTITLE_SIZE = 10F;

    /// <summary>
    /// Espaço entre as letras do subtítulo (0,3 em).
    /// </summary>
    private const float SUBTITLE_SPACING = SUBTITLE_SIZE * 0.3F;

    /// <summary>
    /// Opacidade do subtítulo.
    /// </summary>
    private const float SUBTITLE_OPACITY = 0.7F;

    /// <summary>
    /// Topo do rótulo do estilo.
    /// </summary>
    private const float STYLE_TOP = 174F;

    /// <summary>
    /// Espaço entre o rótulo do estilo e o estilo escolhido.
    /// </summary>
    private const float STYLE_GAP = 8F;

    /// <summary>
    /// Tamanho do nome do estilo.
    /// </summary>
    private const float STYLE_SIZE = 17F;

    /// <summary>
    /// Altura das setas ◄ ► ao lado do estilo.
    /// </summary>
    private const float ARROW_SIZE = 10F;

    /// <summary>
    /// Espaço entre as setas e o nome do estilo.
    /// </summary>
    private const float ARROW_GAP = 14F;

    /// <summary>
    /// Topo da linha do som.
    /// </summary>
    private const float SOUND_TOP = 226F;

    /// <summary>
    /// Tamanho do estado do som.
    /// </summary>
    private const float SOUND_SIZE = 11F;

    /// <summary>
    /// Espaço entre o rótulo do som e o estado.
    /// </summary>
    private const float SOUND_GAP = 8F;

    /// <summary>
    /// Tamanho do convite para jogar.
    /// </summary>
    private const float PRESS_SIZE = 12F;

    /// <summary>
    /// Espaço entre o convite e o "chão" de blocos.
    /// </summary>
    private const float PRESS_GAP = 14F;

    /// <summary>
    /// Lado das células do "chão".
    /// </summary>
    private const float GROUND_CELL = 12F;

    /// <summary>
    /// Um "chão" de peças empilhadas no rodapé do LCD (letra = tipo da peça, ponto = vazio).
    /// </summary>
    private static readonly string[] _ground =
    [
        "..........T.........IIII",
        "OO.....J.TTT....S...LLL.",
        "OOZZ...JJJ.....SS..ZL...",
    ];

    /// <summary>
    /// A cor de cada letra do "chão".
    /// </summary>
    private static readonly Dictionary<char, PieceColor> _groundColors = new()
    {
        ['I'] = PieceColor.Cyan,
        ['T'] = PieceColor.Purple,
        ['L'] = PieceColor.Orange,
        ['J'] = PieceColor.Blue,
        ['O'] = PieceColor.Yellow,
        ['S'] = PieceColor.Green,
        ['Z'] = PieceColor.Red,
    };

    /// <summary>
    /// A próxima partida, já montada: é dela que vem o recorde mostrado no menu.
    /// </summary>
    private readonly GameController _nextGame;

    /// <summary>
    /// Cria o menu.
    /// </summary>
    /// <param name="frame">O aparelho.</param>
    public MenuScreen(ConsoleFrame frame) : base(frame)
    {
        _nextGame = PlayScreen.CreateGame(frame);
        KeyBindings = new Dictionary<Keys, Action>
        {
            [Keys.Left] = ToggleSkin,
            [Keys.Right] = ToggleSkin,
            [Keys.Enter] = () => Frame.Navigate(new PlayScreen(Frame, _nextGame)),
            [Keys.F1] = () => Frame.Navigate(new HelpScreen(Frame)),
            [Keys.F12] = () => Frame.Navigate(new AboutScreen(Frame)),
            [Keys.ShiftKey] = ToggleSound,
        };

        Layout = new ConsoleLayout(
            new DpadSetup(true, false, Texto.CapTheme),
            [
                new ConsoleButton(Keys.F1, Texto.Ajuda, Texto.KeyF1),
                new ConsoleButton(Keys.F12, Texto.Sobre, Texto.KeyF12),
                SoundButton,
            ],
            new ConsoleButton(Keys.Enter, Texto.CapPlay, Texto.KeyEnter));
    }

    /// <inheritdoc/>
    public override ConsoleLayout Layout { get; }

    /// <inheritdoc/>
    protected override IReadOnlyDictionary<Keys, Action> KeyBindings { get; }

    /// <inheritdoc/>
    public override void Paint(LcdCanvas lcd)
    {
        const float LEFT = LcdCanvas.PADDING;
        const float RIGHT = LcdCanvas.WIDTH - LcdCanvas.PADDING;
        var top = LcdCanvas.PADDING;
        var center = LcdCanvas.CENTER_X;

        lcd.Label(Texto.Recorde, LEFT, top + ((RECORD_HEIGHT - LcdCanvas.LABEL_SIZE) / 2));
        lcd.Number(_nextGame.GetMaxScore(), RECORD_DIGITS, RIGHT - LcdCanvas.MeasureNumber(RECORD_DIGITS, RECORD_HEIGHT), top, RECORD_HEIGHT);

        TitleArt.Draw(lcd, center, TITLE_TOP, TITLE_CELL, CELL_GAP);

        using (var faded = lcd.InkBrush(SUBTITLE_OPACITY))
        {
            var subtitleTop = TITLE_TOP + TitleArt.Height(TITLE_CELL, CELL_GAP) + SUBTITLE_GAP;
            lcd.TextCentered(Texto.Subtitle.ToUpperInvariant(), Fonts.Pixel(SUBTITLE_SIZE), center, subtitleTop, SUBTITLE_SPACING, faded);
        }

        PaintStyle(lcd, center);
        PaintSound(lcd, center);

        var groundTop = LcdCanvas.HEIGHT - LcdCanvas.PADDING - LcdCanvas.GridSize(_ground.Length, GROUND_CELL, CELL_GAP);
        if (lcd.BlinkOn)
        {
            lcd.TextCentered(Texto.EnterToPlay.ToUpperInvariant(), Fonts.Pixel(PRESS_SIZE), center, groundTop - PRESS_GAP - PRESS_SIZE);
        }

        var groundLeft = center - (LcdCanvas.GridSize(_ground[0].Length, GROUND_CELL, CELL_GAP) / 2);
        lcd.Grid(groundLeft, groundTop, _ground[0].Length, _ground.Length, GROUND_CELL, CELL_GAP,
            (column, row) => _groundColors.GetValueOrDefault(_ground[row][column], PieceColor.None));
    }

    /// <summary>
    /// Desenha o seletor de tema: ◄ NOME ►.
    /// </summary>
    private void PaintStyle(LcdCanvas lcd, float center)
    {
        lcd.LabelCentered(Texto.Theme, center, STYLE_TOP);

        var name = (Frame.Skin == Skin.Tetris ? Texto.SkinTetris : Texto.SkinBrickGame).ToUpperInvariant();
        var font = Fonts.Pixel(STYLE_SIZE);
        var width = lcd.MeasureText(name, font);
        var top = STYLE_TOP + LcdCanvas.LABEL_SIZE + STYLE_GAP;
        var arrowTop = top + ((STYLE_SIZE - ARROW_SIZE) / 2);

        lcd.Text(name, font, center - (width / 2), top);
        lcd.Arrow(center - (width / 2) - ARROW_GAP - ARROW_SIZE, arrowTop, ARROW_SIZE, right: false);
        lcd.Arrow(center + (width / 2) + ARROW_GAP, arrowTop, ARROW_SIZE, right: true);
    }

    /// <summary>
    /// Desenha a linha do som: SOM ♪ LIGADO.
    /// </summary>
    private void PaintSound(LcdCanvas lcd, float center)
    {
        var state = (Frame.Sound.Mute ? Texto.SomDesligado : $"♪ {Texto.SomLigado}").ToUpperInvariant();
        var font = Fonts.Pixel(SOUND_SIZE);
        var labelWidth = lcd.MeasureLabel(Texto.Sound);
        var left = center - ((labelWidth + SOUND_GAP + lcd.MeasureText(state, font)) / 2);

        lcd.Label(Texto.Sound, left, SOUND_TOP + ((SOUND_SIZE - LcdCanvas.LABEL_SIZE) / 2));
        lcd.Text(state, font, left + labelWidth + SOUND_GAP, SOUND_TOP);
    }

    /// <summary>
    /// Alterna entre os temas BRICK GAME e TETRIS, com o som de peça encaixando como confirmação.
    /// </summary>
    private void ToggleSkin()
    {
        Frame.Skin = Frame.Skin == Skin.Tetris ? Skin.BrickGame : Skin.Tetris;
        Frame.Sound.Dock();
    }
}
