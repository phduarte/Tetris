using System.Windows.Forms;

namespace Gadz.Tetris.Desktop.Commands
{
    internal class MoveLeftCommand : Command
    {
        public MoveLeftCommand(GameController game) 
            : base(game)
        {
        }

        public override Keys Key => Keys.Left;

        public override bool Control => false;

        public override void Execute() => _gameController.MoveLeft();
    }
}