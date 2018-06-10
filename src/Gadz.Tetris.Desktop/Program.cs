using System;
using System.Windows.Forms;

namespace Gadz.Tetris.Desktop {
    static class Program {

        public static bool ClassicMode { get; set; }
        public static SoundPlayer SoundPlayer { get; set; }

        [STAThread]
        static void Main() {
            ClassicMode = false;
            SoundPlayer = new SoundPlayer();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Choose());
        }
    }
}
