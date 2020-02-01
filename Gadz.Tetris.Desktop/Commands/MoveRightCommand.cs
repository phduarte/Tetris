using System.Windows.Forms;

namespace Gadz.Tetris.Desktop.Commands
{
    internal class MoveRightCommand : Command
    {
        public MoveRightCommand(GameController game)
            : base(game)
        {
        }

        public override Keys Key => Keys.Right;

        public override bool Control => false;

        public override void Execute() => _gameController.MoveRight();
    }
}