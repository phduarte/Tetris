namespace Gadz.Tetris.Application.Commands;

internal class MoveLeftCommand(GameController game) : Command(game)
{
    public override Key Key => Key.Left;

    public override bool Control => false;

    public override void Execute() => _gameController.MoveLeft();
}