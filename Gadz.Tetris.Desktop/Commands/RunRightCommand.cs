using System.Windows.Forms;

namespace Gadz.Tetris.Desktop.Commands
{
    internal class RunRightCommand : Command
    {
        public RunRightCommand(GameController game)
            : base(game)
        {
        }

        public override Keys Key => Keys.Right;

        public override bool Control => true;

        public override void Execute() => _gameController.RunRight();
    }
}