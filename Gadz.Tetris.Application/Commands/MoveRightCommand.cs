namespace Gadz.Tetris.Application.Commands;

internal class MoveRightCommand(GameController game) : Command(game)
{
    public override Key Key => Key.Right;

    public override bool Control => false;

    public override void Execute() => _gameController.MoveRight();

    /// <summary>
    /// Só no primeiro toque; segurando a tecla, quem age é o <see cref="HoldRightCommand"/>.
    /// </summary>
    public override bool Match(Key key, bool control, bool repeat)
        => base.Match(key, control, repeat) && !repeat;
}