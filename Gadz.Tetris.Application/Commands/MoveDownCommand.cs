using Gadz.Tetris.Application;

namespace Gadz.Tetris.Application.Commands
{
    internal class MoveDownCommand : Command
    {
        public MoveDownCommand(GameController game)
            : base(game)
        {
        }

        public override Key Key => Key.Down;

        public override bool Control => false;

        public override void Execute() => _gameController.MoveDown();
    }
}