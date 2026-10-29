using System.Diagnostics.CodeAnalysis;
using Gadz.Tetris.Domain.Models;
using Gadz.Tetris.Domain.Models.Statistics;

namespace Gadz.Tetris.Data;

/// <summary>
/// Guarda as estatísticas num arquivo texto separado por tab (<c>%USERPROFILE%\Tetris\stats.mc</c>), uma partida por linha.
/// O arquivo é lido uma vez, no construtor estático, e mantido em cache.
/// </summary>
[ExcludeFromCodeCoverage]
public class StatsRepository : IStatsRepository
{
    /// <summary>
    /// Caminho do arquivo de estatísticas na pasta do usuário.
    /// </summary>
    private static string StatsFilePath
    {
        get
        {
            var dir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Tetris\stats.mc";
            return dir;
        }
    }

    /// <summary>
    /// Separador das colunas de cada linha do arquivo.
    /// </summary>
    private const char DELIMITADOR = '\t';

    /// <summary>
    /// Índice da coluna da identidade da partida. A ordem das colunas é fixa: só acrescente novas ao final.
    /// </summary>
    private const int COL_ID = 0;

    /// <summary>
    /// Índice da coluna da pontuação.
    /// </summary>
    private const int COL_POINTS = 1;

    /// <summary>
    /// Índice da coluna do nível.
    /// </summary>
    private const int COL_LEVEL = 2;

    /// <summary>
    /// Índice da coluna das linhas removidas.
    /// </summary>
    private const int COL_LINES = 3;

    /// <summary>
    /// Índice da coluna da velocidade.
    /// </summary>
    private const int COL_SPEED = 4;

    /// <summary>
    /// Índice da coluna dos movimentos.
    /// </summary>
    private const int COL_MOVES = 5;

    /// <summary>
    /// Índice da coluna das peças assentadas.
    /// </summary>
    private const int COL_BLOCKS = 6;

    /// <summary>
    /// Índice da coluna da duração, gravada em ticks.
    /// </summary>
    private const int COL_TIME = 7;

    /// <summary>
    /// As partidas já lidas do arquivo ou salvas nesta execução.
    /// </summary>
    private static IList<Stats> _cache;

    /// <summary>
    /// Carrega o arquivo para o cache, criando-o se ainda não existir.
    /// </summary>
    static StatsRepository()
    {
        _cache = LoadCache();
    }

    /// <summary>
    /// Acrescenta a partida ao final do arquivo e ao cache.
    /// </summary>
    /// <param name="stats">As estatísticas da partida.</param>
    public async void Save(Stats stats)
    {
        if (!File.Exists(StatsFilePath))
            return;

        await Task.Factory.StartNew(() =>
        {
            using var file = new StreamWriter(StatsFilePath, true);
            var line = Map(stats);
            file.WriteLine(line);
        }).ConfigureAwait(false);

        _cache.Add(stats);
    }

    /// <summary>
    /// Busca uma partida guardada.
    /// </summary>
    /// <param name="id">A identidade da partida.</param>
    /// <returns>As estatísticas da partida.</returns>
    /// <exception cref="InvalidOperationException">Se não houver partida com essa identidade.</exception>
    public Stats Load(Identity id)
    {
        return All().First(x => x.Id.Equals(id));
    }

    /// <summary>
    /// A maior pontuação entre as partidas guardadas.
    /// </summary>
    /// <returns>O recorde, ou 0 se não houver partidas.</returns>
    public int MaxScore()
    {
        try
        {
            return All().Max(x => x.Score);
        }
        catch (InvalidOperationException)
        {
            return 0;
        }
    }

    /// <summary>
    /// Todas as partidas guardadas.
    /// </summary>
    /// <returns>As partidas em cache.</returns>
    public IEnumerable<Stats> All()
    {
        return _cache;
    }

    /// <summary>
    /// Lê todas as partidas do arquivo.
    /// </summary>
    /// <returns>As partidas lidas; vazia se o arquivo não existir.</returns>
    private static List<Stats> LoadCache()
    {
        var results = new List<Stats>();

        if (!StatsFileExists())
            return results;

        using (var file = new StreamReader(StatsFilePath))
        {
            string? line;
            while ((line = file.ReadLine()) != null)
            {
                var stats = Stats.Load(Map(line));
                results.Add(stats);
            }
        }

        return results;
    }

    /// <summary>
    /// Converte uma linha do arquivo num registro de partida.
    /// </summary>
    /// <param name="line">A linha, com as colunas separadas por <see cref="DELIMITADOR"/>.</param>
    /// <returns>O registro da partida.</returns>
    private static StatsRecord Map(string line)
    {
        var cols = line.Split(DELIMITADOR);

        return new StatsRecord
        {
            Id = Identity.Parse(cols[COL_ID]),
            Score = int.Parse(cols[COL_POINTS]),
            Level = int.Parse(cols[COL_LEVEL]),
            Lines = int.Parse(cols[COL_LINES]),
            Speed = int.Parse(cols[COL_SPEED]),
            Moves = int.Parse(cols[COL_MOVES]),
            Blocks = int.Parse(cols[COL_BLOCKS]),
            Seconds = long.Parse(cols[COL_TIME])
        };
    }

    /// <summary>
    /// Converte uma partida numa linha do arquivo, na ordem fixa das colunas.
    /// </summary>
    /// <param name="stats">As estatísticas da partida.</param>
    /// <returns>A linha a gravar.</returns>
    private static string Map(Stats stats)
    {
        var cols = new string[8];

        cols[COL_ID] = stats.Id;
        cols[COL_POINTS] = stats.Score.ToString();
        cols[COL_LEVEL] = stats.Level.ToString();
        cols[COL_LINES] = stats.Lines.ToString();
        cols[COL_SPEED] = stats.Speed.ToString();
        cols[COL_MOVES] = stats.Moves.ToString();
        cols[COL_BLOCKS] = stats.Blocks.ToString();
        cols[COL_TIME] = stats.Duration.Ticks.ToString();

        return string.Join(DELIMITADOR.ToString(), cols);
    }

    /// <summary>
    /// Garante que o arquivo exista, criando a pasta e um arquivo vazio se preciso.
    /// </summary>
    /// <returns><c>true</c> se o arquivo existia antes da chamada.</returns>
    private static bool StatsFileExists()
    {
        var fi = new FileInfo(StatsFilePath);

        if (!fi.Exists)
        {
            fi.Directory?.Create();
            fi.Create().Dispose();
        }

        return fi.Exists;
    }
}
