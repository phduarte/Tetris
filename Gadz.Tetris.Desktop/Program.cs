namespace Gadz.Tetris.Desktop
{
    internal static class Program
    {
        public static bool ClassicMode { get; set; }

        [STAThread]
        private static void Main()
        {
            ClassicMode = false;
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
            System.Windows.Forms.Application.Run(new Choose());
        }
    }
}