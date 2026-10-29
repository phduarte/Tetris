namespace Gadz.Tetris.Application.Commands;

internal class MoveLeftCommand(GameController game) : Command(game)
{
    public override Key Key => Key.Left;

    public override bool Control => false;

    public override void Execute() => _gameController.MoveLeft();

    /// <summary>
    /// Só no primeiro toque; segurando a tecla, quem age é o <see cref="HoldLeftCommand"/>.
    /// </summary>
    public override bool Match(Key key, bool control, bool repeat)
        => base.Match(key, control, repeat) && !repeat;
}