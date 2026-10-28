namespace Gadz.Tetris.Application.Commands;

internal class MoveDownCommand(GameController game) : Command(game)
{
    public override Key Key => Key.Down;

    public override bool Control => false;

    public override void Execute() => _gameController.MoveDown();
}