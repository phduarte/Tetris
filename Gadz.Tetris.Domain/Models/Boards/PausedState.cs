namespace Gadz.Tetris.Domain.Models.Boards;

/// <summary>
/// Partida pausada (também é o estado do tabuleiro antes do primeiro <see cref="Board.Start"/>): nada se move até continuar.
/// </summary>
internal class PausedState : IBoardState
{
    /// <summary>
    /// Instância única: o estado não guarda dados, então todas as transições a reutilizam.
    /// </summary>
    public static readonly PausedState Instance = new();

    /// <summary>
    /// Impede novas instâncias; use <see cref="Instance"/>.
    /// </summary>
    private PausedState()
    {
    }

    /// <inheritdoc/>
    public bool IsPlaying => false;

    /// <inheritdoc/>
    public bool CanChangeStats => false;

    /// <inheritdoc/>
    public bool CanStart => false;

    /// <inheritdoc/>
    public bool CanMove => false;

    /// <inheritdoc/>
    public bool CanPause => false;

    /// <inheritdoc/>
    public bool CanContinue => true;

    /// <inheritdoc/>
    public bool CanRestart => true;

    /// <inheritdoc/>
    public bool CanFinish => true;

    /// <summary>
    /// O nome do estado, verificado nos testes.
    /// </summary>
    /// <returns>O nome do estado.</returns>
    public override string ToString() => "Parado";
}
