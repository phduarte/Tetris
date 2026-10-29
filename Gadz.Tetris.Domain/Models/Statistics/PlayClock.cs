namespace Gadz.Tetris.Domain.Models.Statistics;

/// <summary>
/// O cronômetro de uma partida: soma o tempo dos trechos jogados, sem contar as pausas.
/// </summary>
/// <param name="elapsed">Tempo já jogado antes de o cronômetro ser criado (de uma partida guardada).</param>
internal sealed class PlayClock(TimeSpan elapsed)
{
    /// <summary>
    /// Tempo dos trechos já fechados.
    /// </summary>
    private TimeSpan _closed = elapsed;

    /// <summary>
    /// Início do trecho em andamento, ou <c>null</c> se o cronômetro estiver parado.
    /// </summary>
    private DateTime? _startTime;

    /// <summary>
    /// Cria um cronômetro zerado e parado.
    /// </summary>
    public PlayClock()
        : this(TimeSpan.Zero)
    {
    }

    /// <summary>
    /// Tempo jogado: os trechos fechados mais o trecho em andamento.
    /// </summary>
    public TimeSpan Elapsed => _startTime.HasValue ? _closed + (DateTime.Now - _startTime.Value) : _closed;

    /// <summary>
    /// Abre um trecho e começa a contar o tempo. Não faz nada se já estiver contando.
    /// </summary>
    public void Start()
    {
        _startTime ??= DateTime.Now;
    }

    /// <summary>
    /// Fecha o trecho em andamento e para de contar o tempo. Não faz nada se já estiver parado.
    /// </summary>
    public void Stop()
    {
        if (!_startTime.HasValue)
            return;

        _closed += DateTime.Now - _startTime.Value;
        _startTime = null;
    }
}
