using System.Windows.Forms;

namespace Gadz.Tetris.Desktop.Commands
{
    internal class SmashDownCommand : Command
    {
        public SmashDownCommand(GameController game)
            : base(game)
        {
        }

        public override Keys Key => Keys.Space;

        public override bool Control => false;

        public override void Execute() => _gameController.SmashDown();

        public override bool Match(Keys key, bool control)
            => key == Keys.Space || (key == Keys.Down && control);

        public override string ToString()
        {
            return nameof(SmashDownCommand);
        }
    }
}