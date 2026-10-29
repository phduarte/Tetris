using Gadz.Tetris.Domain.Models.Pieces;

namespace Gadz.Tetris.Desktop.Device;

/// <summary>
/// O título "TETRIS" desenhado com as próprias células do LCD; no estilo TETRIS, cada letra ganha a cor de uma peça.
/// </summary>
internal static class TitleArt
{
    /// <summary>
    /// Altura de uma letra, em células.
    /// </summary>
    private const int LETTER_ROWS = 5;

    /// <summary>
    /// Largura de uma letra, em células.
    /// </summary>
    private const int LETTER_COLUMNS = 3;

    /// <summary>
    /// Colunas apagadas entre as letras.
    /// </summary>
    private const int LETTER_GAP = 1;

    /// <summary>
    /// O desenho de cada letra, linha a linha ('1' = célula acesa).
    /// </summary>
    private static readonly Dictionary<char, string[]> _letters = new()
    {
        ['T'] = ["111", "010", "010", "010", "010"],
        ['E'] = ["111", "100", "110", "100", "111"],
        ['R'] = ["110", "101", "110", "101", "101"],
        ['I'] = ["111", "010", "010", "010", "111"],
        ['S'] = ["011", "100", "010", "001", "110"],
    };

    /// <summary>
    /// A palavra e a cor de cada letra (T roxo, E vermelho, T roxo, R laranja, I ciano, S verde).
    /// </summary>
    private static readonly (char Letter, PieceColor Color)[] _word =
    [
        ('T', PieceColor.Purple), ('E', PieceColor.Red), ('T', PieceColor.Purple),
        ('R', PieceColor.Orange), ('I', PieceColor.Cyan), ('S', PieceColor.Green),
    ];

    /// <summary>
    /// Quantidade de colunas de células do título.
    /// </summary>
    private static int Columns => (_word.Length * (LETTER_COLUMNS + LETTER_GAP)) - LETTER_GAP;

    /// <summary>
    /// Largura do título.
    /// </summary>
    /// <param name="cell">Lado de cada célula.</param>
    /// <param name="gap">Espaço entre as células.</param>
    /// <returns>A largura.</returns>
    public static float Width(float cell, float gap) => LcdCanvas.GridSize(Columns, cell, gap);

    /// <summary>
    /// Altura do título.
    /// </summary>
    /// <param name="cell">Lado de cada célula.</param>
    /// <param name="gap">Espaço entre as células.</param>
    /// <returns>A altura.</returns>
    public static float Height(float cell, float gap) => LcdCanvas.GridSize(LETTER_ROWS, cell, gap);

    /// <summary>
    /// Desenha o título centralizado; as células fora das letras ficam apagadas, como no resto do LCD.
    /// </summary>
    /// <param name="lcd">O LCD.</param>
    /// <param name="centerX">O centro horizontal.</param>
    /// <param name="y">Topo.</param>
    /// <param name="cell">Lado de cada célula.</param>
    /// <param name="gap">Espaço entre as células.</param>
    public static void Draw(LcdCanvas lcd, float centerX, float y, float cell, float gap)
        => lcd.Grid(centerX - (Width(cell, gap) / 2), y, Columns, LETTER_ROWS, cell, gap, ColorAt);

    /// <summary>
    /// A cor da célula (coluna, linha) do título.
    /// </summary>
    private static PieceColor ColorAt(int column, int row)
    {
        var index = column / (LETTER_COLUMNS + LETTER_GAP);
        var offset = column % (LETTER_COLUMNS + LETTER_GAP);

        if (offset >= LETTER_COLUMNS)
        {
            return PieceColor.None;
        }

        var (letter, color) = _word[index];
        return _letters[letter][row][offset] == '1' ? color : PieceColor.None;
    }
}
