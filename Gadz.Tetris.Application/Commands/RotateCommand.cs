using Gadz.Tetris.Application;

namespace Gadz.Tetris.Application.Commands
{
    internal class RotateCommand : Command
    {
        public RotateCommand(GameController game)
            : base(game)
        {
        }

        public override Key Key => Key.Up;

        public override bool Control => false;

        public override void Execute() => _gameController.Rotate();
    }
}