using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using Gadz.Tetris.Domain.Models;
using Gadz.Tetris.Domain.Models.Statistics;
using Microsoft.JSInterop;

namespace Gadz.Tetris.Web.Infrastructure;

/// <summary>
/// Guarda as estatísticas das partidas no <c>localStorage</c> do navegador, como JSON.
/// <para>A <see cref="IStatsRepository"/> é síncrona; no WebAssembly o interop com o JavaScript também pode ser
/// (<see cref="IJSInProcessRuntime"/>), então o domínio não precisa conhecer <c>async</c> de infraestrutura.
/// As partidas são lidas uma vez ao criar o repositório e mantidas em memória; cada <see cref="Save"/> regrava a lista.</para>
/// </summary>
/// <param name="js">O runtime JavaScript do navegador, chamado de forma síncrona.</param>
[ExcludeFromCodeCoverage]
public class LocalStorageStatsRepository(IJSInProcessRuntime js) : IStatsRepository
{
    /// <summary>
    /// A chave no <c>localStorage</c>. A versão no nome permite migrar o formato no futuro sem ler dados incompatíveis.
    /// </summary>
    private const string STORAGE_KEY = "gadz-tetris:stats:v1";

    /// <summary>
    /// As partidas guardadas, carregadas do navegador no primeiro uso.
    /// </summary>
    private List<StoredStats>? _cache;

    /// <summary>
    /// As partidas guardadas, carregadas do navegador se ainda não estiverem em memória.
    /// </summary>
    private List<StoredStats> Cache => _cache ??= Read();

    /// <inheritdoc/>
    public void Save(Stats stats)
    {
        Cache.Add(StoredStats.From(stats));
        js.InvokeVoid("localStorage.setItem", STORAGE_KEY, JsonSerializer.Serialize(Cache, StoredStatsContext.Default.ListStoredStats));
    }

    /// <inheritdoc/>
    public Stats Load(Identity id) => All().First(x => x.Id.ToString() == id.ToString());

    /// <inheritdoc/>
    public int MaxScore() => Cache.Count == 0 ? 0 : Cache.Max(x => x.Score);

    /// <inheritdoc/>
    public IEnumerable<Stats> All() => Cache.Select(x => Stats.Load(x.ToRecord()));

    /// <summary>
    /// Lê as partidas do navegador. Sem dados (primeira visita ou dados do site apagados) ou com dados ilegíveis,
    /// começa uma lista vazia: perder o histórico é melhor que impedir o jogo de abrir.
    /// </summary>
    /// <returns>As partidas guardadas.</returns>
    private List<StoredStats> Read()
    {
        var json = js.Invoke<string?>("localStorage.getItem", STORAGE_KEY);

        if (string.IsNullOrEmpty(json))
        {
            return [];
        }

        try
        {
            return JsonSerializer.Deserialize(json, StoredStatsContext.Default.ListStoredStats) ?? [];
        }
        catch (JsonException)
        {
            return [];
        }
    }
}

/// <summary>
/// Uma partida como fica guardada no navegador. Separada do <see cref="StatsRecord"/> porque a <see cref="Identity"/>
/// não é serializável e para o formato do JSON não mudar junto com o domínio.
/// </summary>
/// <param name="Id">A identidade da partida.</param>
/// <param name="Score">Pontuação final.</param>
/// <param name="Lines">Linhas removidas.</param>
/// <param name="Level">Nível atingido.</param>
/// <param name="Speed">Velocidade atingida.</param>
/// <param name="Moves">Movimentos feitos pelo jogador.</param>
/// <param name="Blocks">Peças assentadas no tabuleiro.</param>
/// <param name="Ticks">Duração da partida em ticks.</param>
/// <param name="Tetris">Pontos obtidos com TETRIS.</param>
internal sealed record StoredStats(string Id, int Score, int Lines, int Level, int Speed, int Moves, int Blocks, long Ticks, int Tetris)
{
    /// <summary>
    /// Converte as estatísticas de uma partida para o formato guardado.
    /// </summary>
    /// <param name="stats">As estatísticas.</param>
    /// <returns>A partida no formato guardado.</returns>
    public static StoredStats From(Stats stats)
        => new(stats.Id.ToString(), stats.Score, stats.Lines, stats.Level, stats.Speed, stats.Moves, stats.Blocks, stats.Duration.Ticks, stats.Tetris);

    /// <summary>
    /// Converte a partida guardada para o registro que o domínio sabe carregar.
    /// </summary>
    /// <returns>O registro da partida.</returns>
    public StatsRecord ToRecord() => new()
    {
        Id = Identity.Parse(Id),
        Score = Score,
        Lines = Lines,
        Level = Level,
        Speed = Speed,
        Moves = Moves,
        Blocks = Blocks,
        Seconds = Ticks,
        Tetris = Tetris,
    };
}

/// <summary>
/// Serialização gerada em tempo de compilação: dispensa reflexão, que o trimming da publicação WebAssembly pode remover.
/// </summary>
[JsonSerializable(typeof(List<StoredStats>))]
internal sealed partial class StoredStatsContext : JsonSerializerContext;
