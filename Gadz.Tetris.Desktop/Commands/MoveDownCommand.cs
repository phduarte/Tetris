using System.Windows.Forms;

namespace Gadz.Tetris.Desktop.Commands
{
    internal class MoveDownCommand : Command
    {
        public MoveDownCommand(GameController game) 
            : base(game)
        {
        }

        public override Keys Key => Keys.Down;

        public override bool Control => false;

        public override void Execute() => _gameController.MoveDown();
    }
}