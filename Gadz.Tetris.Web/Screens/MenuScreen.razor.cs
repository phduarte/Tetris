using Gadz.Tetris.Application;
using Gadz.Tetris.Domain.Models.Pieces;
using Gadz.Tetris.Texts;
using Gadz.Tetris.Web.Device;
using Gadz.Tetris.Web.Input;

namespace Gadz.Tetris.Web.Screens;

/// <summary>
/// Menu inicial: o recorde como convite, o título em blocos e o estado do som.
/// Enter começa a partida; AJUDA (F1) e SOBRE (F2) são botões da carcaça.
/// </summary>
public partial class MenuScreen
{
    /// <summary>
    /// Posições do display do recorde.
    /// </summary>
    private const int RECORD_DIGITS = 6;

    /// <summary>
    /// O "chão" de peças encaixadas no rodapé do menu, linha a linha (letra = tipo da peça, ponto = vazio).
    /// </summary>
    private static readonly string[] _ground =
    [
        "..........T.........IIII",
        "OO.....J.TTT....S...LLL.",
        "OOZZ...JJJ.....SS..ZL...",
    ];

    /// <summary>
    /// A cor de cada letra do chão.
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
    /// A próxima partida, criada já no menu para mostrar o recorde (como no Desktop) e iniciada pelo Enter.
    /// </summary>
    private GameController _nextGame = default!;

    /// <summary>
    /// O recorde, formatado para o display.
    /// </summary>
    private string _record = string.Empty;

    /// <inheritdoc/>
    protected override IReadOnlyDictionary<string, Action> KeyBindings => new Dictionary<string, Action>
    {
        [KeyMap.ENTER] = () => Frame.Navigate<PlayScreen>(new Dictionary<string, object?> { [nameof(PlayScreen.Controller)] = _nextGame }),
        [KeyMap.F1] = () => Frame.Navigate<HelpScreen>(),
        [KeyMap.F2] = () => Frame.Navigate<AboutScreen>(),
        [KeyMap.SHIFT] = ToggleSound,
    };

    /// <summary>
    /// Os botões da carcaça: ajuda, sobre, som e jogar.
    /// </summary>
    private IReadOnlyList<ConsoleButton> Buttons =>
    [
        new(KeyMap.F1, Texts[nameof(Jogo.Ajuda)], Texts[nameof(Jogo.KeyF1)]),
        new(KeyMap.F2, Texts[nameof(Jogo.Sobre)], Texts[nameof(Jogo.KeyF2)]),
        SoundButton,
        new(KeyMap.ENTER, Texts[nameof(Jogo.CapPlay)], Texts[nameof(Jogo.KeyEnter)], Big: true),
    ];

    /// <summary>
    /// Cria a próxima partida e lê o recorde.
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();
        _nextGame = PlayScreen.CreateGame(Frame);
        _record = Digits.Number(_nextGame.GetMaxScore(), RECORD_DIGITS);
    }

    /// <summary>
    /// A cor de uma célula do chão.
    /// </summary>
    private static PieceColor GroundColor(int column, int row) => _groundColors.GetValueOrDefault(_ground[row][column], PieceColor.None);
}
