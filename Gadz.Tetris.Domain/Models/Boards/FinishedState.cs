namespace Gadz.Tetris.Domain.Models.Boards;

/// <summary>
/// Partida encerrada, por desistência ou derrota: só é possível começar outra.
/// </summary>
internal class FinishedState : IBoardState
{
    /// <summary>
    /// Instância única: o estado não guarda dados, então todas as transições a reutilizam.
    /// </summary>
    public static readonly FinishedState Instance = new();

    /// <summary>
    /// Impede novas instâncias; use <see cref="Instance"/>.
    /// </summary>
    private FinishedState()
    {
    }

    /// <inheritdoc/>
    public bool IsPlaying => false;

    /// <inheritdoc/>
    public bool CanChangeStats => false;

    /// <inheritdoc/>
    public bool CanStart => true;

    /// <inheritdoc/>
    public bool CanMove => false;

    /// <inheritdoc/>
    public bool CanPause => false;

    /// <inheritdoc/>
    public bool CanContinue => false;

    /// <inheritdoc/>
    public bool CanRestart => true;

    /// <inheritdoc/>
    public bool CanFinish => false;

    /// <summary>
    /// O nome do estado, verificado nos testes.
    /// </summary>
    /// <returns>O nome do estado.</returns>
    public override string ToString() => "Finished";
}
