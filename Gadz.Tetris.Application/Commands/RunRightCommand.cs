using Gadz.Tetris.Application;

namespace Gadz.Tetris.Application.Commands
{
    internal class RunRightCommand : Command
    {
        public RunRightCommand(GameController game)
            : base(game)
        {
        }

        public override Key Key => Key.Right;

        public override bool Control => true;

        public override void Execute() => _gameController.RunRight();
    }
}