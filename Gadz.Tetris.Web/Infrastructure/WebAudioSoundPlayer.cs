using System.Diagnostics.CodeAnalysis;
using Gadz.Tetris.Application;
using Microsoft.JSInterop;

namespace Gadz.Tetris.Web.Infrastructure;

/// <summary>
/// Adapter da Web Audio API do navegador (<c>wwwroot/js/sound.js</c>) para o <see cref="ISoundPlayer"/>.
/// Toca os mesmos <c>.wav</c> do Desktop, linkados do <c>Gadz.Tetris.SoundPlayer</c> (servidos em <c>AudioFiles/</c>).
/// A preferência de mudo fica no <c>localStorage</c>.
/// </summary>
/// <param name="js">O runtime JavaScript do navegador.</param>
[ExcludeFromCodeCoverage]
public class WebAudioSoundPlayer(IJSInProcessRuntime js) : ISoundPlayer
{
    /// <summary>
    /// A chave da preferência de mudo no <c>localStorage</c>.
    /// </summary>
    private const string MUTE_KEY = "gadz-tetris:mute:v1";

    /// <summary>
    /// Música de início (<c>Start.wav</c>).
    /// </summary>
    private const string INTRO = "Start";

    /// <summary>
    /// Peça movendo ou girando (<c>Move.wav</c>).
    /// </summary>
    private const string MOVE = "Move";

    /// <summary>
    /// Peça deslizando (<c>Run.wav</c>).
    /// </summary>
    private const string RUN = "Run";

    /// <summary>
    /// Peça encaixando (<c>Dock.wav</c>).
    /// </summary>
    private const string DOCK = "Dock";

    /// <summary>
    /// Linhas removidas (<c>Clean.wav</c>).
    /// </summary>
    private const string CLEAN = "Clean";

    /// <summary>
    /// Música de fim de jogo (<c>Ending.wav</c>).
    /// </summary>
    private const string ENDING = "Ending";

    /// <summary>
    /// O módulo <c>sound.js</c>; nulo até <see cref="LoadAsync"/> terminar, e enquanto isso nada toca.
    /// </summary>
    private IJSInProcessObjectReference? _module;

    /// <inheritdoc/>
    public bool Mute { get; private set; } = js.Invoke<string?>("localStorage.getItem", MUTE_KEY) == bool.TrueString;

    /// <summary>
    /// Importa o módulo de som e começa a baixar os <c>.wav</c>. Chamado uma vez, no <c>Program.cs</c>.
    /// </summary>
    /// <returns>A tarefa da importação.</returns>
    public async Task LoadAsync()
    {
        _module = await js.InvokeAsync<IJSInProcessObjectReference>("import", "./js/sound.js");
        _module.InvokeVoid("load", (object)new[] { INTRO, MOVE, RUN, DOCK, CLEAN, ENDING });
    }

    /// <inheritdoc/>
    public void ToggleMute()
    {
        Mute = !Mute;
        js.InvokeVoid("localStorage.setItem", MUTE_KEY, Mute.ToString());

        if (Mute)
        {
            _module?.InvokeVoid("stopAll");
        }
        else
        {
            Dock();
        }
    }

    /// <inheritdoc/>
    public void Start()
    {
        Stop(ENDING);
        Play(INTRO);
    }

    /// <inheritdoc/>
    public void Move() => Play(MOVE);

    /// <inheritdoc/>
    public void Slide() => Play(RUN);

    /// <inheritdoc/>
    public void Dock() => Play(DOCK);

    /// <inheritdoc/>
    public void Clear() => Play(CLEAN);

    /// <inheritdoc/>
    public void End()
    {
        Stop(INTRO);
        Play(ENDING);
    }

    /// <summary>
    /// Toca o som do início, se o som estiver ligado.
    /// </summary>
    /// <param name="name">O nome do <c>.wav</c>, sem extensão.</param>
    private void Play(string name)
    {
        if (!Mute)
        {
            _module?.InvokeVoid("play", name);
        }
    }

    /// <summary>
    /// Interrompe o som, se estiver tocando.
    /// </summary>
    /// <param name="name">O nome do <c>.wav</c>, sem extensão.</param>
    private void Stop(string name) => _module?.InvokeVoid("stop", name);
}
