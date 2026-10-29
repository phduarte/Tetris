namespace Gadz.Tetris.Domain.Models.Statistics;

/// <summary>
/// Defines the <see cref="Stats" />
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="Stats"/> class.
/// </remarks>
/// <param name="id">The id<see cref="Identity"/></param>
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
    /// Defines the _durationHistory
    /// </summary>
    private IDictionary<DateTime, long> _durationHistory = new Dictionary<DateTime, long>();

    /// <summary>
    /// Defines the _startTime
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
    /// Gets the Lines
    /// </summary>
    public int Lines { get; private set; }

    /// <summary>
    /// Gets the Score
    /// </summary>
    public int Score { get; private set; }

    /// <summary>
    /// Gets the Level
    /// </summary>
    public int Level { get; private set; }

    /// <summary>
    /// Gets the Speed
    /// </summary>
    public int Speed { get; private set; }

    /// <summary>
    /// Gets the Moves
    /// </summary>
    public int Moves { get; private set; }

    /// <summary>
    /// Gets the Blocks
    /// </summary>
    public int Blocks { get; private set; }

    /// <summary>
    /// Gets the Duration
    /// </summary>
    public TimeSpan Duration => CalculateTime();

    /// <summary>
    /// Intervalo, em milissegundos, entre as descidas automáticas da peça.
    /// Diminui <see cref="INTERVAL_STEP_MS"/> a cada nível até o mínimo de <see cref="MIN_INTERVAL_MS"/>.
    /// </summary>
    public int Interval => Math.Max(MIN_INTERVAL_MS, BASE_INTERVAL_MS - ((Level - INITIAL_LEVEL) * INTERVAL_STEP_MS));

    /// <summary>
    /// 
    /// </summary>
    public int Tetris { get; private set; }

    public float TetrisRate => Score > 0 ? Tetris / (float)Score : 0F;

    /// <summary>
    /// Initializes a new instance of the <see cref="Stats"/> class.
    /// </summary>
    public Stats()
        : this(Identity.New())
    {
        Level = INITIAL_LEVEL;
        Speed = INITIAL_SPEED;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Stats"/> class.
    /// </summary>
    /// <param name="id">The id<see cref="Identity"/></param>
    /// <param name="points">The points<see cref="int"/></param>
    /// <param name="lines">The lines<see cref="int"/></param>
    /// <param name="level">The level<see cref="int"/></param>
    /// <param name="speed">The speed<see cref="int"/></param>
    /// <param name="moves">The moves<see cref="int"/></param>
    /// <param name="blocks">The blocks<see cref="int"/></param>
    /// <param name="duration">The duration<see cref="long"/></param>
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
    /// The Gain
    /// </summary>
    /// <param name="lines">The lines<see cref="int"/></param>
    public void Gain(int lines)
    {
        Lines += lines;
        Score += CalculateScore(lines);
        _stageLines += lines;
        CheckLevel();
    }

    /// <summary>
    /// The CountMovement
    /// </summary>
    public void CountMovement()
    {
        Moves++;
    }

    /// <summary>
    /// The Start
    /// </summary>
    public void Start()
    {
        _startTime = DateTime.Now;
    }

    /// <summary>
    /// The Pause
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
    /// The Continue
    /// </summary>
    public void Continue()
    {
        _startTime = DateTime.Now;
    }

    /// <summary>
    /// The Finish
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
    /// The IncludeBlock
    /// </summary>
    public void IncludeBlock()
    {
        Blocks++;
    }

    /// <summary>
    /// The CalculateTime
    /// </summary>
    /// <returns>The <see cref="TimeSpan"/></returns>
    private TimeSpan CalculateTime()
    {
        long duracao = 0;

        duracao += _durationHistory.Sum(_ => _.Value);
        if (_startTime.HasValue)
            duracao += (DateTime.Now - _startTime.Value).Ticks;

        return new TimeSpan(duracao);
    }

    /// <summary>
    /// The CheckLevel
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
    /// The LevelUp
    /// </summary>
    private void LevelUp()
    {
        Level++;
        Speed++;
    }

    /// <summary>
    /// The CalculateScore
    /// </summary>
    /// <param name="linhas">The linhas<see cref="int"/></param>
    /// <returns>The <see cref="int"/></returns>
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
    /// Carrega uma estatística baseada num registro de estatística salvo anteriormente.
    /// </summary>
    /// <param name="record"></param>
    /// <returns></returns>
    public static Stats Load(StatsRecord record)
    {
        return new Stats(record);
    }
}
