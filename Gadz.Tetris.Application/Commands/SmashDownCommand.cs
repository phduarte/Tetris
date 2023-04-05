using Gadz.Tetris.Application;

namespace Gadz.Tetris.Application.Commands
{
    internal class SmashDownCommand : Command
    {
        public SmashDownCommand(GameController game)
            : base(game)
        {
        }

        public override Key Key => Key.Space;

        public override bool Control => false;

        public override void Execute() => _gameController.SmashDown();

        public override bool Match(Key key, bool control)
            => key == Key.Space || key == Key.Down && control;

        public override string ToString()
        {
            return nameof(SmashDownCommand);
        }
    }
}