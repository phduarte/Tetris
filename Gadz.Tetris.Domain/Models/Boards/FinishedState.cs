namespace Gadz.Tetris.Domain.Models.Boards;

/// <summary>
/// Defines the <see cref="FinishedState" />
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

    /// <summary>
    /// Gets a value indicating whether IsPlaying
    /// </summary>
    public bool IsPlaying => false;

    /// <summary>
    /// Gets a value indicating whether CanChangeStats
    /// </summary>
    public bool CanChangeStats => false;

    /// <summary>
    /// Gets a value indicating whether CanStart
    /// </summary>
    public bool CanStart => true;

    /// <summary>
    /// Gets a value indicating whether CanMove
    /// </summary>
    public bool CanMove => false;

    /// <summary>
    /// Gets a value indicating whether CanPause
    /// </summary>
    public bool CanPause => false;

    /// <summary>
    /// Gets a value indicating whether CanContinue
    /// </summary>
    public bool CanContinue => false;

    /// <summary>
    /// Gets a value indicating whether CanRestart
    /// </summary>
    public bool CanRestart => true;

    /// <summary>
    /// Gets a value indicating whether CanFinish
    /// </summary>
    public bool CanFinish => false;

    /// <summary>
    /// The ToString
    /// </summary>
    /// <returns>The <see cref="string"/></returns>
    public override string ToString() => "Finished";
}
