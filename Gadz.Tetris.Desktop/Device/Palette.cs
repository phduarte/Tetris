using Gadz.Tetris.Domain.Models.Pieces;

namespace Gadz.Tetris.Desktop.Device;

/// <summary>
/// Cores do aparelho: carcaça grafite com detalhes em coral, moldura do LCD, botões e as cores das peças no estilo TETRIS.
/// </summary>
internal static class Palette
{
    /// <summary>
    /// Topo do degradê da carcaça.
    /// </summary>
    public static readonly Color CaseTop = ColorTranslator.FromHtml("#37383e");

    /// <summary>
    /// Base do degradê da carcaça.
    /// </summary>
    public static readonly Color CaseBottom = ColorTranslator.FromHtml("#25262a");

    /// <summary>
    /// Moldura escura em volta do LCD.
    /// </summary>
    public static readonly Color Bezel = ColorTranslator.FromHtml("#131417");

    /// <summary>
    /// Cor de destaque da marca (coral): botão grande, dicas de tecla e "TETRIS" no topo.
    /// </summary>
    public static readonly Color Accent = ColorTranslator.FromHtml("#ff7a59");

    /// <summary>
    /// Coral escuro, para o degradê e a sombra do botão grande.
    /// </summary>
    public static readonly Color AccentDark = ColorTranslator.FromHtml("#c9492c");

    /// <summary>
    /// Coral claro, para o brilho do botão grande.
    /// </summary>
    public static readonly Color AccentLight = ColorTranslator.FromHtml("#ffb199");

    /// <summary>
    /// "Chão" sob o botão grande, que dá a impressão de profundidade.
    /// </summary>
    public static readonly Color AccentShadow = ColorTranslator.FromHtml("#7a2c1a");

    /// <summary>
    /// Texto claro sobre a carcaça (marca e legendas dos botões).
    /// </summary>
    public static readonly Color CaseText = ColorTranslator.FromHtml("#e9e6df");

    /// <summary>
    /// Sombra projetada pelos botões sobre a carcaça.
    /// </summary>
    public static readonly Color ButtonShadow = ColorTranslator.FromHtml("#141518");

    /// <summary>
    /// Anel escuro em volta do botão grande.
    /// </summary>
    public static readonly Color ButtonRing = ColorTranslator.FromHtml("#1d1e22");

    /// <summary>
    /// Topo do degradê do direcional.
    /// </summary>
    public static readonly Color DpadTop = ColorTranslator.FromHtml("#3f4046");

    /// <summary>
    /// Base do degradê do direcional.
    /// </summary>
    public static readonly Color DpadBottom = ColorTranslator.FromHtml("#2b2c31");

    /// <summary>
    /// Miolo do direcional.
    /// </summary>
    public static readonly Color DpadCenter = ColorTranslator.FromHtml("#2f3035");

    /// <summary>
    /// Setas do direcional.
    /// </summary>
    public static readonly Color DpadArrow = ColorTranslator.FromHtml("#c9cbd0");

    /// <summary>
    /// Setas do direcional que não fazem nada na tela atual.
    /// </summary>
    public static readonly Color DpadArrowDisabled = ColorTranslator.FromHtml("#55575d");

    /// <summary>
    /// Topo do degradê dos botões pequenos.
    /// </summary>
    public static readonly Color PillTop = ColorTranslator.FromHtml("#4a4b52");

    /// <summary>
    /// Base do degradê dos botões pequenos.
    /// </summary>
    public static readonly Color PillBottom = ColorTranslator.FromHtml("#303137");

    /// <summary>
    /// Fundo do botão de fechar.
    /// </summary>
    public static readonly Color CloseButton = ColorTranslator.FromHtml("#1b1c20");

    /// <summary>
    /// O "X" do botão de fechar.
    /// </summary>
    public static readonly Color CloseMark = ColorTranslator.FromHtml("#8d9096");

    /// <summary>
    /// Cor de cada peça no estilo TETRIS, a mesma do jogo original (I=ciano, T=roxo, L=laranja...).
    /// </summary>
    private static readonly Dictionary<PieceColor, Color> _pieces = new()
    {
        [PieceColor.Cyan] = ColorTranslator.FromHtml("#35c6e8"),
        [PieceColor.Purple] = ColorTranslator.FromHtml("#a66cf0"),
        [PieceColor.Orange] = ColorTranslator.FromHtml("#f7953b"),
        [PieceColor.Blue] = ColorTranslator.FromHtml("#4a7cf7"),
        [PieceColor.Yellow] = ColorTranslator.FromHtml("#f5cc3c"),
        [PieceColor.Green] = ColorTranslator.FromHtml("#4cc27a"),
        [PieceColor.Red] = ColorTranslator.FromHtml("#ef5a6f"),
    };

    /// <summary>
    /// As cores das peças na ordem I, T, L, J, O, S, Z: usada no logotipo e nas animações.
    /// </summary>
    public static IReadOnlyList<PieceColor> PieceOrder { get; } =
        [PieceColor.Cyan, PieceColor.Purple, PieceColor.Orange, PieceColor.Blue, PieceColor.Yellow, PieceColor.Green, PieceColor.Red];

    /// <summary>
    /// A cor de tela de uma peça.
    /// </summary>
    /// <param name="color">A cor da peça no domínio.</param>
    /// <returns>A cor a pintar; transparente para <see cref="PieceColor.None"/>.</returns>
    public static Color Of(PieceColor color) => _pieces.GetValueOrDefault(color, Color.Transparent);
}
