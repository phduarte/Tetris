using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Gadz.Tetris.Desktop {


    public class SoundPlayer {
        
        public bool Mute { get; private set; }

        [DllImport("winmm.dll")]
        static extern Int32 mciSendString(string command, StringBuilder buffer, int bufferSize, IntPtr hwndCallback);

        public SoundPlayer() {
            mciSendString(@"open Sounds\Start.wav type waveaudio alias intro", null, 0, IntPtr.Zero);
            mciSendString(@"open Sounds\Clear.wav type waveaudio alias clean", null, 0, IntPtr.Zero);
            mciSendString(@"open Sounds\Move.wav type waveaudio alias move", null, 0, IntPtr.Zero);
            mciSendString(@"open Sounds\Run.wav type waveaudio alias run", null, 0, IntPtr.Zero);
            mciSendString(@"open Sounds\Ending.wav type waveaudio alias ending", null, 0, IntPtr.Zero);
            mciSendString(@"open Sounds\Dock.wav type waveaudio alias dock", null, 0, IntPtr.Zero);
        }

        public void Start() {
            if (!Mute) {
                mciSendString(@"stop ending", null, 0, IntPtr.Zero);
                mciSendString(@"play intro from 0", null, 0, IntPtr.Zero);
            }
        }

        public void Clear() {
            if (!Mute) {
                mciSendString(@"play clean from 0", null, 0, IntPtr.Zero);
            }
        }

        public void Move() {
            if (!Mute) {
                mciSendString(@"play move from 0", null, 0, IntPtr.Zero);
            }
        }

        public void Slide() {
            if (!Mute) {
                mciSendString(@"play run from 0", null, 0, IntPtr.Zero);
            }
        }

        public void End() {
            if (!Mute) {
                mciSendString(@"stop intro", null, 0, IntPtr.Zero);
                mciSendString(@"play ending from 0", null, 0, IntPtr.Zero);
            }
        }

        public void ToggleMute() {
            Mute = !Mute;

            if (Mute) {
                StopAll();
            }
            else {
                Dock();
            }
        }

        public void Dock() {
            if (!Mute) {
                mciSendString(@"play dock from 0", null, 0, IntPtr.Zero);
            }
        }

        void StopAll() {
            mciSendString(@"stop intro", null, 0, IntPtr.Zero);
            mciSendString(@"stop clean", null, 0, IntPtr.Zero);
            mciSendString(@"stop move", null, 0, IntPtr.Zero);
            mciSendString(@"stop run", null, 0, IntPtr.Zero);
            mciSendString(@"stop ending", null, 0, IntPtr.Zero);
        }
    }
}
