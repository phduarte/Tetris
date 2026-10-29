namespace Gadz.Tetris.Domain.Models.Boards;

/// <summary>
/// Partida em andamento: a peça desce sozinha e aceita comandos; pode ser pausada, reiniciada ou encerrada.
/// </summary>
internal class PlayingState : IBoardState
{
    /// <summary>
    /// Instância única: o estado não guarda dados, então todas as transições a reutilizam.
    /// </summary>
    public static readonly PlayingState Instance = new();

    /// <summary>
    /// Impede novas instâncias; use <see cref="Instance"/>.
    /// </summary>
    private PlayingState()
    {
    }

    /// <inheritdoc/>
    public bool IsPlaying => true;

    /// <inheritdoc/>
    public bool CanChangeStats => true;

    /// <inheritdoc/>
    public bool CanStart => false;

    /// <inheritdoc/>
    public bool CanMove => true;

    /// <inheritdoc/>
    public bool CanPause => true;

    /// <inheritdoc/>
    public bool CanContinue => false;

    /// <inheritdoc/>
    public bool CanRestart => true;

    /// <inheritdoc/>
    public bool CanFinish => true;

    /// <summary>
    /// O nome do estado, verificado nos testes.
    /// </summary>
    /// <returns>O nome do estado.</returns>
    public override string ToString() => "Playing";
}
