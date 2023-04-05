using Gadz.Tetris.Application;

namespace Gadz.Tetris.Application.Commands
{
    public abstract class Command
    {
        readonly protected GameController _gameController;

        public abstract Key Key { get; }
        public abstract bool Control { get; }

        protected Command(GameController game)
        {
            _gameController = game;
        }

        public abstract void Execute();

        public virtual bool Match(Key key, bool control)
        {
            return Key == key && Control == control;
        }
    }
}
