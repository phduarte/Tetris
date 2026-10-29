namespace Gadz.Tetris.Application.Commands;

internal class ToggleMuteCommand(GameController game) : Command(game)
{
    public override Key Key => Key.ShiftKey;

    public override bool Control => false;

    public override void Execute() => _gameController.ToggleMute();
}
