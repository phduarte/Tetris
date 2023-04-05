using Gadz.Tetris.Application;

namespace Gadz.Tetris.Application.Commands
{
    internal class MoveRightCommand : Command
    {
        public MoveRightCommand(GameController game)
            : base(game)
        {
        }

        public override Key Key => Key.Right;

        public override bool Control => false;

        public override void Execute() => _gameController.MoveRight();
    }
}