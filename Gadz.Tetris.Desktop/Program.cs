using System;
using System.Windows.Forms;

namespace Gadz.Tetris.Desktop {
    static class Program {

        public static bool ClassicMode { get; set; }
        public static Player Player { get; set; }

        [STAThread]
        static void Main() {
            ClassicMode = false;
            Player = new Player();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Inicio());
        }
    }
}
