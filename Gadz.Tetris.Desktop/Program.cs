using Gadz.Tetris.Data;
using Gadz.Tetris.Desktop.Device;
using Gadz.Tetris.SoundPlayer;

namespace Gadz.Tetris.Desktop;

internal static class Program
{
    /// <summary>
    /// Ponto de entrada e raiz de composição: o único lugar do Desktop que cria a infraestrutura
    /// (arquivo de estatísticas e som do Windows) e a entrega ao aparelho.
    /// </summary>
    [STAThread]
    private static void Main()
    {
        System.Windows.Forms.Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
        System.Windows.Forms.Application.EnableVisualStyles();
        System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
        System.Windows.Forms.Application.Run(new ConsoleFrame(new SoundPlayerAdapter(), new StatsRepository()));
    }
}
