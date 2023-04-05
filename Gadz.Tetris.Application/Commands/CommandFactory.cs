﻿namespace Gadz.Tetris.Application.Commands
{
    public class CommandFactory
    {
        readonly GameController _gameController;

        public CommandFactory(GameController gameController)
            => _gameController = gameController;

        public List<Command> Commands => new List<Command>
        {
            new SmashDownCommand(_gameController),
            new MoveDownCommand(_gameController),
            new RunLeftCommand(_gameController),
            new MoveLeftCommand(_gameController),
            new RunRightCommand(_gameController),
            new MoveRightCommand(_gameController),
            new RotateCommand(_gameController),
            new ExitCommand(_gameController),
            new PauseCommand(_gameController),
            new ContinueCommand(_gameController),
            new ToggleMuteCommand(_gameController)
        };

        public IEnumerable<Command> GetAll(Key key, bool control)
            => Commands.Where(x => x.Match(key, control));
    }
}
