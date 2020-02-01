using System.Windows.Forms;

namespace Gadz.Tetris.Desktop.Commands
{
    internal class ToggleMuteCommand : Command
    {
        public override Keys Key => Keys.ShiftKey;

        public override bool Control => false;

        public ToggleMuteCommand(GameController game)
            : base(game)
        {
        }

        public override void Execute() => Program.SoundPlayer.ToggleMute();
    }
}