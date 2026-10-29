namespace Gadz.Tetris.Application.Commands;

/// <summary>
/// Registro dos comandos de teclado do jogo. Novas ações entram aqui como novas subclasses de <see cref="Command"/>.
/// </summary>
/// <param name="gameController">O jogo sobre o qual os comandos atuam.</param>
public class CommandFactory(GameController gameController)
{
    /// <summary>
    /// Todos os comandos, criados uma única vez. A ordem importa: a UI executa o primeiro que casar com a tecla.
    /// </summary>
    public IReadOnlyList<Command> Commands { get; } =
    [
        new SmashDownCommand(gameController),
        new MoveDownCommand(gameController),
        new RunLeftCommand(gameController),
        new MoveLeftCommand(gameController),
        new RunRightCommand(gameController),
        new MoveRightCommand(gameController),
        new RotateCommand(gameController),
        new ExitCommand(gameController),
        new TogglePauseCommand(gameController),
        new ToggleMuteCommand(gameController)
    ];

    /// <summary>
    /// Os comandos associados à tecla, na ordem de registro.
    /// </summary>
    /// <param name="key">A tecla pressionada.</param>
    /// <param name="control">Se a tecla Ctrl estava pressionada.</param>
    /// <returns>Os comandos que casam com a tecla.</returns>
    public IEnumerable<Command> GetAll(Key key, bool control)
        => Commands.Where(x => x.Match(key, control));
}
