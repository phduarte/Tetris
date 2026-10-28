namespace Gadz.Tetris.Application.Commands;

internal class RunRightCommand(GameController game) : Command(game)
{
    public override Key Key => Key.Right;

    public override bool Control => true;

    public override void Execute() => _gameController.RunRight();
}