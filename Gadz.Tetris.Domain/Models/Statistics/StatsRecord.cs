namespace Gadz.Tetris.Domain.Models.Statistics;

/// <summary>
/// Os dados de uma partida como ficam guardados no repositório; <see cref="Stats.Load"/> reconstrói a entidade a partir dele.
/// </summary>
public struct StatsRecord
{
    /// <summary>
    /// A identidade da partida.
    /// </summary>
    public Identity Id;
    /// <summary>
    /// Pontuação final.
    /// </summary>
    public int Score;
    /// <summary>
    /// Linhas removidas.
    /// </summary>
    public int Lines;
    /// <summary>
    /// Nível atingido.
    /// </summary>
    public int Level;
    /// <summary>
    /// Velocidade atingida.
    /// </summary>
    public int Speed;
    /// <summary>
    /// Movimentos feitos pelo jogador.
    /// </summary>
    public int Moves;
    /// <summary>
    /// Peças assentadas no tabuleiro.
    /// </summary>
    public int Blocks;
    /// <summary>
    /// Duração da partida em ticks (apesar do nome, é o valor gravado em <c>COL_TIME</c>).
    /// </summary>
    public long Seconds;
    /// <summary>
    /// Pontos obtidos com TETRIS (4 linhas de uma vez).
    /// </summary>
    public int Tetris { get; set; }
    /// <summary>
    /// Fração da pontuação obtida com TETRIS.
    /// </summary>
    public float TetrisRate { get; set; }
}
