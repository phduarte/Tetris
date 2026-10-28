namespace Gadz.Tetris.Application.Commands;

internal class TogglePauseCommand(GameController game) : Command(game)
{
    public override Key Key => Key.Enter;

    public override bool Control => false;

    public override void Execute() => _gameController.TogglePause();
}
