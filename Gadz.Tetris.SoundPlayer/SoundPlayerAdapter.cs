using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;

namespace Gadz.Tetris.SoundPlayer
{
    [ExcludeFromCodeCoverage]
    public class SoundPlayerAdapter
    {
        public static bool Mute { get; private set; }

        private static IDictionary<string, UnmanagedMemoryStream> _sounds =
            new Dictionary<string, UnmanagedMemoryStream> {
            { "clean", Sounds.Clean},
            { "intro", Sounds.Start },
            { "move", Sounds.Move},
            { "run", Sounds.Run },
            { "ending", Sounds.Ending },
            { "dock", Sounds.Dock }
        };

        [DllImport("winmm.dll")]
        private static extern Int32 mciSendString(string command, StringBuilder buffer, int bufferSize, IntPtr hwndCallback);

        static SoundPlayerAdapter()
        {
            foreach (var sound in _sounds)
            {
                var filePath = Path.Combine(Path.GetTempPath(), sound.Key + ".wav");

                if (!File.Exists(filePath))
                {
                    using (var file = new StreamWriter(filePath, false))
                    {
                        sound.Value.CopyTo(file.BaseStream);
                    }
                }

                mciSendString($@"open {filePath} type waveaudio alias {sound.Key}", null, 0, IntPtr.Zero);
            }
        }

        public static void Start()
        {
            if (!Mute)
            {
                mciSendString(@"stop ending", null, 0, IntPtr.Zero);
                mciSendString(@"play intro from 0", null, 0, IntPtr.Zero);
            }
        }

        public static void Clear()
        {
            if (!Mute)
            {
                mciSendString(@"play clean from 0", null, 0, IntPtr.Zero);
            }
        }

        public static void Move()
        {
            if (!Mute)
            {
                mciSendString(@"play move from 0", null, 0, IntPtr.Zero);
            }
        }

        public static void Slide()
        {
            if (!Mute)
            {
                mciSendString(@"play run from 0", null, 0, IntPtr.Zero);
            }
        }

        public static void End()
        {
            if (!Mute)
            {
                mciSendString(@"stop intro", null, 0, IntPtr.Zero);
                mciSendString(@"play ending from 0", null, 0, IntPtr.Zero);
            }
        }

        public static void ToggleMute()
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

        public static void TurnSoundOn()
        {
            Mute = false;
        }

        public static void TurnSoundOff()
        {
            Mute = true;
        }

        public static void Dock()
        {
            if (!Mute)
            {
                mciSendString(@"play dock from 0", null, 0, IntPtr.Zero);
            }
        }

        private static void StopAll()
        {
            foreach (var sound in _sounds)
            {
                mciSendString($@"stop {sound.Key}", null, 0, IntPtr.Zero);
            }
        }
    }
}