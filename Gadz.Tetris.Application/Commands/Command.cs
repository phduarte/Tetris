namespace Gadz.Tetris.Application.Commands;

public abstract class Command(GameController game)
{
    readonly protected GameController _gameController = game;

    public abstract Key Key { get; }
    public abstract bool Control { get; }

    public abstract void Execute();

    public virtual bool Match(Key key, bool control)
    {
        return Key == key && Control == control;
    }
}
