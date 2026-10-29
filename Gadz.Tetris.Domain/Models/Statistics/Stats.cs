namespace Gadz.Tetris.Domain.Models.Statistics;

/// <summary>
/// As estatísticas de uma partida: pontuação, linhas, nível, tempo jogado e contadores.
/// Concentra as regras de pontuação, de subida de nível e da velocidade de queda.
/// </summary>
/// <param name="id">A identidade da partida.</param>
public class Stats(Identity id) : Entity(id)
{
    /// <summary>
    /// Intervalo, em milissegundos, entre as descidas automáticas da peça no nível 1.
    /// </summary>
    private const int BASE_INTERVAL_MS = 1000;

    /// <summary>
    /// Quanto o intervalo diminui, em milissegundos, a cada nível.
    /// </summary>
    private const int INTERVAL_STEP_MS = 50;

    /// <summary>
    /// Menor intervalo possível, em milissegundos, para o jogo continuar jogável.
    /// </summary>
    private const int MIN_INTERVAL_MS = 100;

    /// <summary>
    /// Nível em que toda partida começa.
    /// </summary>
    private const int INITIAL_LEVEL = 1;

    /// <summary>
    /// Velocidade em que toda partida começa.
    /// </summary>
    private const int INITIAL_SPEED = 1;

    /// <summary>
    /// Linhas a completar para sair do primeiro nível.
    /// </summary>
    private const int INITIAL_LINE_GOAL = 20;

    /// <summary>
    /// Quanto a meta de linhas cresce a cada nível (25%).
    /// </summary>
    private const double GOAL_GROWTH_RATE = 0.25;

    /// <summary>
    /// Pontos por remover 1 linha numa jogada.
    /// </summary>
    private const int SCORE_SINGLE = 40;

    /// <summary>
    /// Pontos por remover 2 linhas numa jogada.
    /// </summary>
    private const int SCORE_DOUBLE = 100;

    /// <summary>
    /// Pontos por remover 3 linhas numa jogada.
    /// </summary>
    private const int SCORE_TRIPLE = 300;

    /// <summary>
    /// Pontos por remover 4 linhas numa jogada (TETRIS).
    /// </summary>
    private const int SCORE_TETRIS = 1200;

    /// <summary>
    /// Quantidade de linhas removidas de uma vez que caracteriza um TETRIS.
    /// </summary>
    private const int TETRIS_LINES = 4;

    /// <summary>
    /// Os trechos já jogados (início → duração em ticks); cada pausa fecha um trecho.
    /// </summary>
    private IDictionary<DateTime, long> _durationHistory = new Dictionary<DateTime, long>();

    /// <summary>
    /// Início do trecho em andamento, ou <c>null</c> se a partida estiver pausada ou encerrada.
    /// </summary>
    private DateTime? _startTime;

    /// <summary>
    /// Linhas removidas desde a última subida de nível.
    /// </summary>
    private int _stageLines;

    /// <summary>
    /// Linhas a completar no nível atual para subir de nível.
    /// </summary>
    private int _goal = INITIAL_LINE_GOAL;

    /// <summary>
    /// Linhas removidas na partida.
    /// </summary>
    public int Lines { get; private set; }

    /// <summary>
    /// Pontuação acumulada.
    /// </summary>
    public int Score { get; private set; }

    /// <summary>
    /// O nível atual; sobe a cada meta de linhas cumprida.
    /// </summary>
    public int Level { get; private set; }

    /// <summary>
    /// A velocidade atual; sobe junto com o nível.
    /// </summary>
    public int Speed { get; private set; }

    /// <summary>
    /// Movimentos feitos pelo jogador.
    /// </summary>
    public int Moves { get; private set; }

    /// <summary>
    /// Peças assentadas no tabuleiro.
    /// </summary>
    public int Blocks { get; private set; }

    /// <summary>
    /// Tempo jogado, somando os trechos fechados e o trecho em andamento (sem contar as pausas).
    /// </summary>
    public TimeSpan Duration => CalculateTime();

    /// <summary>
    /// Intervalo, em milissegundos, entre as descidas automáticas da peça.
    /// Diminui <see cref="INTERVAL_STEP_MS"/> a cada nível até o mínimo de <see cref="MIN_INTERVAL_MS"/>.
    /// </summary>
    public int Interval => Math.Max(MIN_INTERVAL_MS, BASE_INTERVAL_MS - ((Level - INITIAL_LEVEL) * INTERVAL_STEP_MS));

    /// <summary>
    /// Pontos obtidos com TETRIS (4 linhas de uma vez).
    /// </summary>
    public int Tetris { get; private set; }

    /// <summary>
    /// Fração da pontuação obtida com TETRIS.
    /// </summary>
    public float TetrisRate => Score > 0 ? Tetris / (float)Score : 0F;

    /// <summary>
    /// Cria as estatísticas de uma partida nova, no nível e velocidade iniciais.
    /// </summary>
    public Stats()
        : this(Identity.New())
    {
        Level = INITIAL_LEVEL;
        Speed = INITIAL_SPEED;
    }

    /// <summary>
    /// Reconstrói as estatísticas de uma partida guardada.
    /// </summary>
    /// <param name="record">Os dados guardados da partida.</param>
    private Stats(StatsRecord record)
        : this(record.Id)
    {
        Score = record.Score;
        Lines = record.Lines;
        Level = record.Level;
        Speed = record.Speed;
        Moves = record.Moves;
        Blocks = record.Blocks;
        _durationHistory.Add(DateTime.Now, record.Seconds);
    }

    /// <summary>
    /// Pontua as linhas removidas numa jogada e sobe de nível se a meta for cumprida.
    /// </summary>
    /// <param name="lines">Quantidade de linhas removidas de uma vez.</param>
    public void Gain(int lines)
    {
        Lines += lines;
        Score += CalculateScore(lines);
        _stageLines += lines;
        CheckLevel();
    }

    /// <summary>
    /// Conta um movimento do jogador.
    /// </summary>
    public void CountMovement()
    {
        Moves++;
    }

    /// <summary>
    /// Começa a contar o tempo.
    /// </summary>
    public void Start()
    {
        _startTime = DateTime.Now;
    }

    /// <summary>
    /// Fecha o trecho em andamento e para de contar o tempo.
    /// </summary>
    public void Pause()
    {
        if (!_startTime.HasValue)
            return;

        long milisecondsDuration = DateTime.Now.Ticks - _startTime.Value.Ticks;
        _durationHistory.Add(new KeyValuePair<DateTime, long>(_startTime.Value, milisecondsDuration));
        _startTime = null;
    }

    /// <summary>
    /// Abre um novo trecho e volta a contar o tempo.
    /// </summary>
    public void Continue()
    {
        _startTime = DateTime.Now;
    }

    /// <summary>
    /// Fecha o trecho em andamento, se houver, ao fim da partida.
    /// </summary>
    public void Finish()
    {
        if (_startTime.HasValue)
        {
            long milisecondsDuration = DateTime.Now.Ticks - _startTime.Value.Ticks;
            _durationHistory.Add(new KeyValuePair<DateTime, long>(_startTime.Value, milisecondsDuration));
            _startTime = null;
        }
    }

    /// <summary>
    /// Conta uma peça assentada.
    /// </summary>
    public void IncludeBlock()
    {
        Blocks++;
    }

    /// <summary>
    /// Soma o tempo dos trechos fechados e do trecho em andamento.
    /// </summary>
    /// <returns>O tempo jogado.</returns>
    private TimeSpan CalculateTime()
    {
        long duracao = 0;

        duracao += _durationHistory.Sum(_ => _.Value);
        if (_startTime.HasValue)
            duracao += (DateTime.Now - _startTime.Value).Ticks;

        return new TimeSpan(duracao);
    }

    /// <summary>
    /// Sobe de nível quando as linhas do nível atual atingem a meta, e aumenta a meta do próximo nível.
    /// </summary>
    private void CheckLevel()
    {
        if (_stageLines >= _goal)
        {
            LevelUp();
            _stageLines = 0;
            _goal += (int)(_goal * GOAL_GROWTH_RATE);
        }
    }

    /// <summary>
    /// Avança o nível e a velocidade.
    /// </summary>
    private void LevelUp()
    {
        Level++;
        Speed++;
    }

    /// <summary>
    /// Os pontos de uma jogada, pela tabela de pontuação. Os pontos de um TETRIS também são somados em <see cref="Tetris"/>.
    /// </summary>
    /// <param name="linhas">Quantidade de linhas removidas de uma vez.</param>
    /// <returns>Os pontos da jogada, ou 0 se a quantidade de linhas não estiver na tabela.</returns>
    private int CalculateScore(int linhas)
    {
        var score = linhas switch
        {
            1 => SCORE_SINGLE,
            2 => SCORE_DOUBLE,
            3 => SCORE_TRIPLE,
            TETRIS_LINES => SCORE_TETRIS,
            _ => 0
        };

        if (linhas == TETRIS_LINES)
            Tetris += score;

        return score;
    }

    /// <summary>
    /// Carrega as estatísticas de uma partida a partir do registro guardado.
    /// </summary>
    /// <param name="record">Os dados guardados da partida.</param>
    /// <returns>As estatísticas da partida.</returns>
    public static Stats Load(StatsRecord record)
    {
        return new Stats(record);
    }
}
