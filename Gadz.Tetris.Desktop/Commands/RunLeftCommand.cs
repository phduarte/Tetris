using System.Windows.Forms;

namespace Gadz.Tetris.Desktop.Commands
{
    internal class RunLeftCommand : Command
    {
        public RunLeftCommand(GameController game)
            : base(game)
        {
        }

        public override Keys Key => Keys.Left;

        public override bool Control => true;

        public override void Execute() => _gameController.RunLeft();
    }
}