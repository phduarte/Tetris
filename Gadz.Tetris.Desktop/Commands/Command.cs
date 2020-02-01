using System.Windows.Forms;

namespace Gadz.Tetris.Desktop.Commands
{
    abstract class Command
    {
        readonly protected GameController _gameController;

        public abstract Keys Key { get; }
        public abstract bool Control { get; }

        protected Command(GameController game)
        {
            _gameController = game;
        }

        public abstract void Execute();

        public virtual bool Match(Keys key, bool control)
        {
            return Key == key && Control == control;
        }
    }
}
