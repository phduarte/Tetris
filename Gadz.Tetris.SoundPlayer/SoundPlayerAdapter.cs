using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;
using Gadz.Tetris.Application;

namespace Gadz.Tetris.SoundPlayer;

/// <summary>
/// Adapter da API nativa de áudio do Windows (<c>winmm.dll</c>) para o <see cref="ISoundPlayer"/>.
/// Os <c>.wav</c> vêm embutidos em <see cref="Sounds"/> e são extraídos para a pasta temporária ao criar o adapter.
/// Crie uma única instância por aplicação: cada uma abre os sons com os mesmos aliases.
/// </summary>
[ExcludeFromCodeCoverage]
public class SoundPlayerAdapter : ISoundPlayer
{
    /// <summary>
    /// Os sons embutidos, pelo alias usado nos comandos do <c>mciSendString</c>.
    /// </summary>
    private static readonly IReadOnlyDictionary<string, UnmanagedMemoryStream> _sounds =
        new Dictionary<string, UnmanagedMemoryStream> {
        { "clean", Sounds.Clean},
        { "intro", Sounds.Start },
        { "move", Sounds.Move},
        { "run", Sounds.Run },
        { "ending", Sounds.Ending },
        { "dock", Sounds.Dock }
    };

    /// <inheritdoc/>
    public bool Mute { get; private set; }

    /// <summary>
    /// Envia um comando à API MCI do Windows.
    /// </summary>
    [DllImport("winmm.dll")]
    private static extern Int32 mciSendString(string command, StringBuilder? buffer, int bufferSize, IntPtr hwndCallback);

    /// <summary>
    /// Extrai os sons para a pasta temporária (se ainda não estiverem lá) e os abre na API MCI.
    /// </summary>
    public SoundPlayerAdapter()
    {
        foreach (var sound in _sounds)
        {
            var filePath = Path.Combine(Path.GetTempPath(), sound.Key + ".wav");

            if (!File.Exists(filePath))
            {
                using var file = new StreamWriter(filePath, false);
                sound.Value.CopyTo(file.BaseStream);
            }

            mciSendString($@"open {filePath} type waveaudio alias {sound.Key}", null, 0, IntPtr.Zero);
        }
    }

    /// <inheritdoc/>
    public void Start()
    {
        if (!Mute)
        {
            mciSendString(@"stop ending", null, 0, IntPtr.Zero);
            mciSendString(@"play intro from 0", null, 0, IntPtr.Zero);
        }
    }

    /// <inheritdoc/>
    public void Clear()
    {
        if (!Mute)
        {
            mciSendString(@"play clean from 0", null, 0, IntPtr.Zero);
        }
    }

    /// <inheritdoc/>
    public void Move()
    {
        if (!Mute)
        {
            mciSendString(@"play move from 0", null, 0, IntPtr.Zero);
        }
    }

    /// <inheritdoc/>
    public void Slide()
    {
        if (!Mute)
        {
            mciSendString(@"play run from 0", null, 0, IntPtr.Zero);
        }
    }

    /// <inheritdoc/>
    public void End()
    {
        if (!Mute)
        {
            mciSendString(@"stop intro", null, 0, IntPtr.Zero);
            mciSendString(@"play ending from 0", null, 0, IntPtr.Zero);
        }
    }

    /// <inheritdoc/>
    public void ToggleMute()
    {
        Mute = !Mute;

        if (Mute)
        {
            StopAll();
        }
        else
        {
            Dock();
        }
    }

    /// <inheritdoc/>
    public void Dock()
    {
        if (!Mute)
        {
            mciSendString(@"play dock from 0", null, 0, IntPtr.Zero);
        }
    }

    /// <summary>
    /// Interrompe todos os sons que estiverem tocando.
    /// </summary>
    private static void StopAll()
    {
        foreach (var sound in _sounds)
        {
            mciSendString($@"stop {sound.Key}", null, 0, IntPtr.Zero);
        }
    }
}
