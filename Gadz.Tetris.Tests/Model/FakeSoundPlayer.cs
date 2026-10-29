using Gadz.Tetris.Application;

namespace Gadz.Tetris.Tests.Model;

/// <summary>
/// Som de mentira para os testes: não toca nada e conta quantas vezes cada som foi pedido.
/// </summary>
internal class FakeSoundPlayer : ISoundPlayer
{
    /// <summary>
    /// Quantas vezes cada som foi pedido, pelo nome do método.
    /// </summary>
    private readonly Dictionary<string, int> _played = [];

    /// <inheritdoc/>
    public bool Mute { get; private set; }

    /// <summary>
    /// Quantas vezes o som foi pedido.
    /// </summary>
    /// <param name="sound">O nome do método do som (use <c>nameof</c>).</param>
    /// <returns>A quantidade de pedidos.</returns>
    public int Played(string sound)
    {
        lock (_played)
        {
            return _played.GetValueOrDefault(sound);
        }
    }

    /// <inheritdoc/>
    public void ToggleMute() => Mute = !Mute;

    /// <inheritdoc/>
    public void Start() => Register(nameof(Start));

    /// <inheritdoc/>
    public void Move() => Register(nameof(Move));

    /// <inheritdoc/>
    public void Slide() => Register(nameof(Slide));

    /// <inheritdoc/>
    public void Dock() => Register(nameof(Dock));

    /// <inheritdoc/>
    public void Clear() => Register(nameof(Clear));

    /// <inheritdoc/>
    public void End() => Register(nameof(End));

    /// <summary>
    /// Conta um pedido do som; o relógio do jogo pode pedir sons de outra thread.
    /// </summary>
    /// <param name="sound">O nome do som.</param>
    private void Register(string sound)
    {
        lock (_played)
        {
            _played[sound] = _played.GetValueOrDefault(sound) + 1;
        }
    }
}
