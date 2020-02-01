using System.Windows.Forms;

namespace Gadz.Tetris.Desktop.Commands
{
    internal class RotateCommand : Command
    {
        public RotateCommand(GameController game)
            : base(game)
        {
        }

        public override Keys Key => Keys.Up;

        public override bool Control => false;

        public override void Execute() => _gameController.Rotate();
    }
}