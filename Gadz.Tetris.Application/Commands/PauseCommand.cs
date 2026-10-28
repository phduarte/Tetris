namespace Gadz.Tetris.Application.Commands;

internal class PauseCommand(GameController game) : Command(game)
{
    public override Key Key => Key.Enter;

    public override bool Control => false;

    public override void Execute() => _gameController.Pause();

    public override bool Match(Key key, bool control) => key == Key && _gameController.Playing;
}