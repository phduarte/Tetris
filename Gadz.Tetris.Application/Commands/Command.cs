namespace Gadz.Tetris.Application.Commands;

public abstract class Command(GameController game)
{
    readonly protected GameController _gameController = game;

    public abstract Key Key { get; }
    public abstract bool Control { get; }

    public abstract void Execute();

    /// <summary>
    /// Se o comando responde à tecla. Por padrão, tanto ao apertar quanto às repetições de uma tecla segurada.
    /// </summary>
    /// <param name="key">A tecla pressionada.</param>
    /// <param name="control">Se a tecla Ctrl estava pressionada.</param>
    /// <param name="repeat">Se é uma repetição automática da tecla segurada.</param>
    /// <returns><c>true</c> se o comando deve ser executado.</returns>
    public virtual bool Match(Key key, bool control, bool repeat)
    {
        return Key == key && Control == control;
    }
}
