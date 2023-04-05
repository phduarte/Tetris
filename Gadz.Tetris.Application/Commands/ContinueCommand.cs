using Gadz.Tetris.Application;

namespace Gadz.Tetris.Application.Commands
{
    internal class ContinueCommand : Command
    {
        public ContinueCommand(GameController game)
            : base(game)
        {
        }

        public override Key Key => Key.Enter;

        public override bool Control => false;

        public override void Execute() => _gameController.Continue();

        public override bool Match(Key key, bool control) => key == Key && !_gameController.Playing;
    }
}