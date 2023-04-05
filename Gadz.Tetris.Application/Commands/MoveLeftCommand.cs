using Gadz.Tetris.Application;

namespace Gadz.Tetris.Application.Commands
{
    internal class MoveLeftCommand : Command
    {
        public MoveLeftCommand(GameController game)
            : base(game)
        {
        }

        public override Key Key => Key.Left;

        public override bool Control => false;

        public override void Execute() => _gameController.MoveLeft();
    }
}