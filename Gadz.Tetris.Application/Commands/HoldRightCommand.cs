namespace Gadz.Tetris.Application.Commands;

/// <summary>
/// Seta para a direita segurada: a cada repetição da tecla a peça anda mais de um bloco, para chegar longe sem
/// precisar bater na tecla, mas sem ir direto até a borda como o Ctrl + seta (<see cref="RunRightCommand"/>).
/// </summary>
/// <param name="game">O jogo.</param>
internal class HoldRightCommand(GameController game) : Command(game)
{
    /// <summary>
    /// Quantos blocos a peça anda a cada repetição da tecla segurada.
    /// </summary>
    private const int STEPS_PER_REPEAT = 2;

    public override Key Key => Key.Right;

    public override bool Control => false;

    public override void Execute()
    {
        for (var i = 0; i < STEPS_PER_REPEAT; i++)
        {
            _gameController.MoveRight();
        }
    }

    /// <summary>
    /// Só nas repetições da tecla segurada; o primeiro toque é do <see cref="MoveRightCommand"/>.
    /// </summary>
    public override bool Match(Key key, bool control, bool repeat)
        => base.Match(key, control, repeat) && repeat;
}
