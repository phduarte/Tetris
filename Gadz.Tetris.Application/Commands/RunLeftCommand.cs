using Gadz.Tetris.Application;

namespace Gadz.Tetris.Application.Commands
{
    internal class RunLeftCommand : Command
    {
        public RunLeftCommand(GameController game)
            : base(game)
        {
        }

        public override Key Key => Key.Left;

        public override bool Control => true;

        public override void Execute() => _gameController.RunLeft();
    }
}