namespace Gadz.Tetris.Desktop;

/// <summary>
/// Tela "Sobre" do jogo.
/// </summary>
public partial class About : Form
{
    /// <summary>
    /// Cria a tela.
    /// </summary>
    public About()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Fecha a tela.
    /// </summary>
    /// <param name="sender">O botão clicado.</param>
    /// <param name="e">Dados do clique.</param>
    private void button2_Click(object sender, EventArgs e)
    {
        Close();
    }
}
