using Gadz.Tetris.Domain.Models;
using Gadz.Tetris.Domain.Models.Statistics;

namespace Gadz.Tetris.Tests.Model;

/// <summary>
/// Repositório em memória para testar o <see cref="Domain.Models.Boards.Board"/> sem gravar em disco.
/// </summary>
internal class FakeStatsRepository : IStatsRepository
{
    /// <summary>
    /// Estatísticas salvas durante o teste.
    /// </summary>
    private readonly List<Stats> _saved = [];

    /// <summary>
    /// Guarda as estatísticas em memória.
    /// </summary>
    /// <param name="stats">As estatísticas a guardar.</param>
    public void Save(Stats stats) => _saved.Add(stats);

    /// <summary>
    /// Recupera as estatísticas guardadas com o id informado.
    /// </summary>
    /// <param name="id">O id das estatísticas.</param>
    /// <returns>As estatísticas encontradas.</returns>
    public Stats Load(Identity id) => _saved.First(s => s.Id.Equals(id));

    /// <summary>
    /// Maior pontuação guardada, ou zero se não houver nenhuma.
    /// </summary>
    /// <returns>A maior pontuação.</returns>
    public int MaxScore() => _saved.Count == 0 ? 0 : _saved.Max(s => s.Score);

    /// <summary>
    /// Todas as estatísticas guardadas.
    /// </summary>
    /// <returns>As estatísticas guardadas.</returns>
    public IEnumerable<Stats> All() => _saved;
}
