using System.Windows.Forms;

namespace Gadz.Tetris.Desktop.Commands
{
    internal class ExitCommand : Command
    {
        public ExitCommand(GameController game)
            : base(game)
        {
        }

        public override Keys Key => Keys.Escape;

        public override bool Control => false;

        public override void Execute() => _gameController.Exit();
    }
}