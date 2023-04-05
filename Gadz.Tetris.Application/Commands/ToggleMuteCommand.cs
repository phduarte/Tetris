using Gadz.Tetris.Application;
using Gadz.Tetris.SoundPlayer;

namespace Gadz.Tetris.Application.Commands
{
    internal class ToggleMuteCommand : Command
    {
        public override Key Key => Key.ShiftKey;

        public override bool Control => false;

        public ToggleMuteCommand(GameController game)
            : base(game)
        {
        }

        public override void Execute() => SoundPlayerAdapter.ToggleMute();
    }
}