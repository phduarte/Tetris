namespace Gadz.Tetris.Application;

/// <summary>
/// Retrato somente leitura das estatísticas de uma partida, para a interface exibir sem acesso às entidades do domínio.
/// </summary>
/// <param name="Score">Pontuação acumulada.</param>
/// <param name="Level">Nível atingido.</param>
/// <param name="Lines">Quantidade de linhas removidas.</param>
/// <param name="Pieces">Quantidade de peças assentadas no tabuleiro.</param>
/// <param name="Duration">Tempo jogado, sem contar as pausas.</param>
/// <param name="TetrisRate">Fração da pontuação obtida com TETRIS (4 linhas de uma vez).</param>
public readonly record struct StatsSnapshot(int Score, int Level, int Lines, int Pieces, TimeSpan Duration, float TetrisRate);
