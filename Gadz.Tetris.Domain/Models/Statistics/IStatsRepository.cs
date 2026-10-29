namespace Gadz.Tetris.Domain.Models.Statistics;

/// <summary>
/// Onde as estatísticas das partidas são guardadas. A interface fica no domínio e a implementação, na infraestrutura.
/// </summary>
public interface IStatsRepository : IRepository
{
    /// <summary>
    /// Guarda as estatísticas de uma partida.
    /// </summary>
    /// <param name="stats">As estatísticas a guardar.</param>
    void Save(Stats stats);

    /// <summary>
    /// Carrega as estatísticas de uma partida guardada.
    /// </summary>
    /// <param name="id">A identidade da partida.</param>
    /// <returns>As estatísticas da partida.</returns>
    Stats Load(Identity id);

    /// <summary>
    /// A maior pontuação entre as partidas guardadas (o recorde).
    /// </summary>
    /// <returns>O recorde, ou 0 se não houver partidas.</returns>
    int MaxScore();

    /// <summary>
    /// Todas as partidas guardadas.
    /// </summary>
    /// <returns>As estatísticas de cada partida.</returns>
    IEnumerable<Stats> All();
}
