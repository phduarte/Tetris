using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Gadz.Tetris.Desktop
{
    public class SoundPlayer
    {
        public bool Mute { get; private set; }

        private static IDictionary<string, UnmanagedMemoryStream> _sounds =
            new Dictionary<string, UnmanagedMemoryStream> {
            { "clean", Resources.Properties.Resources.Clean},
            { "intro", Resources.Properties.Resources.Start },
            { "move", Resources.Properties.Resources.Move},
            { "run", Resources.Properties.Resources.Run },
            { "ending", Resources.Properties.Resources.Ending },
            { "dock", Resources.Properties.Resources.Dock }
        };

        [DllImport("winmm.dll")]
        private static extern Int32 mciSendString(string command, StringBuilder buffer, int bufferSize, IntPtr hwndCallback);

        static SoundPlayer()
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

        public void Start()
        {
            if (!Mute)
            {
                mciSendString(@"stop ending", null, 0, IntPtr.Zero);
                mciSendString(@"play intro from 0", null, 0, IntPtr.Zero);
            }
        }

        public void Clear()
        {
            if (!Mute)
            {
                mciSendString(@"play clean from 0", null, 0, IntPtr.Zero);
            }
        }

        public void Move()
        {
            if (!Mute)
            {
                mciSendString(@"play move from 0", null, 0, IntPtr.Zero);
            }
        }

        public void Slide()
        {
            if (!Mute)
            {
                mciSendString(@"play run from 0", null, 0, IntPtr.Zero);
            }
        }

        public void End()
        {
            if (!Mute)
            {
                mciSendString(@"stop intro", null, 0, IntPtr.Zero);
                mciSendString(@"play ending from 0", null, 0, IntPtr.Zero);
            }
        }

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

        public void TurnSoundOn()
        {
            Mute = false;
        }

        public void TurnSoundOff()
        {
            Mute = true;
        }

        public void Dock()
        {
            if (!Mute)
            {
                mciSendString(@"play dock from 0", null, 0, IntPtr.Zero);
            }
        }

        private void StopAll()
        {
            foreach (var sound in _sounds)
            {
                mciSendString($@"stop {sound.Key}", null, 0, IntPtr.Zero);
            }
        }
    }
}