namespace Gadz.Tetris.Application;

/// <summary>
/// Os sons do jogo. Cada interface entrega a sua implementação ao <see cref="GameController"/>
/// (no Windows, o <c>SoundPlayerAdapter</c>). Fica na Application, e não no Domain, porque som não é regra de jogo.
/// </summary>
public interface ISoundPlayer
{
    /// <summary>
    /// Se o som está desligado.
    /// </summary>
    bool Mute { get; }

    /// <summary>
    /// Liga ou desliga o som.
    /// </summary>
    void ToggleMute();

    /// <summary>
    /// Música de início da partida.
    /// </summary>
    void Start();

    /// <summary>
    /// Som da peça se movendo ou girando.
    /// </summary>
    void Move();

    /// <summary>
    /// Som de cada passo da peça deslizando.
    /// </summary>
    void Slide();

    /// <summary>
    /// Som da peça encaixando no fundo.
    /// </summary>
    void Dock();

    /// <summary>
    /// Som de linhas removidas.
    /// </summary>
    void Clear();

    /// <summary>
    /// Música de fim de jogo.
    /// </summary>
    void End();
}
