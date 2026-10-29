namespace Gadz.Tetris.Application.Commands;

internal class SmashDownCommand(GameController game) : Command(game)
{
    public override Key Key => Key.Space;

    public override bool Control => false;

    public override void Execute() => _gameController.SmashDown();

    public override bool Match(Key key, bool control, bool repeat)
        => key == Key.Space || key == Key.Down && control;

    public override string ToString()
    {
        return nameof(SmashDownCommand);
    }
}