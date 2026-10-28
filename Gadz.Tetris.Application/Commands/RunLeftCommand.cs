namespace Gadz.Tetris.Application.Commands;

internal class RunLeftCommand(GameController game) : Command(game)
{
    public override Key Key => Key.Left;

    public override bool Control => true;

    public override void Execute() => _gameController.RunLeft();
}