namespace Gadz.Tetris.Application.Commands;

internal class MoveRightCommand(GameController game) : Command(game)
{
    public override Key Key => Key.Right;

    public override bool Control => false;

    public override void Execute() => _gameController.MoveRight();
}