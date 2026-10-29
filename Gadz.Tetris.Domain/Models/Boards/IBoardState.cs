namespace Gadz.Tetris.Domain.Models.Boards;

/// <summary>
/// Estado do tabuleiro (State). Cada estado responde o que é permitido nele, para que o <see cref="Board"/>
/// consulte as permissões em vez de espalhar <c>if</c>s sobre "jogando" ou "pausado".
/// </summary>
public interface IBoardState
{
    /// <summary>
    /// Se a partida está em andamento (a peça desce sozinha).
    /// </summary>
    bool IsPlaying { get; }

    /// <summary>
    /// Se é possível começar uma nova partida.
    /// </summary>
    bool CanStart { get; }

    /// <summary>
    /// Se é possível pausar.
    /// </summary>
    bool CanPause { get; }

    /// <summary>
    /// Se é possível continuar uma partida pausada.
    /// </summary>
    bool CanContinue { get; }

    /// <summary>
    /// Se é possível encerrar a partida.
    /// </summary>
    bool CanFinish { get; }

    /// <summary>
    /// Se é possível reiniciar a partida.
    /// </summary>
    bool CanRestart { get; }

    /// <summary>
    /// Se a peça atual pode ser movida ou girada.
    /// </summary>
    bool CanMove { get; }

    /// <summary>
    /// Se as estatísticas da partida podem mudar.
    /// </summary>
    bool CanChangeStats { get; }
}
