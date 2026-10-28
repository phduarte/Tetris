namespace Gadz.Tetris.Application.Commands;

internal class ContinueCommand(GameController game) : Command(game)
{
    public override Key Key => Key.Enter;

    public override bool Control => false;

    public override void Execute() => _gameController.Continue();

    public override bool Match(Key key, bool control) => key == Key && !_gameController.Playing;
}