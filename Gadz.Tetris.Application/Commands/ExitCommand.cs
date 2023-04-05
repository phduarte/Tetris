using Gadz.Tetris.Application;

namespace Gadz.Tetris.Application.Commands
{
    internal class ExitCommand : Command
    {
        public ExitCommand(GameController game)
            : base(game)
        {
        }

        public override Key Key => Key.Escape;

        public override bool Control => false;

        public override void Execute() => _gameController.Exit();
    }
}