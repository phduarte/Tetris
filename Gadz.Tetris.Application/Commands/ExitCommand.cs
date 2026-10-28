namespace Gadz.Tetris.Application.Commands;

internal class ExitCommand(GameController game) : Command(game)
{
    public override Key Key => Key.Escape;

    public override bool Control => false;

    public override void Execute() => _gameController.Exit();
}