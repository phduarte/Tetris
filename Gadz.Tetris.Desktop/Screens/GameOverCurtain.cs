using Gadz.Tetris.Desktop.Device;
using Gadz.Tetris.Domain.Models.Pieces;

namespace Gadz.Tetris.Desktop.Screens;

/// <summary>
/// A animação de fim de jogo dos minigames: uma cortina de blocos que sobe linha a linha e depois desce apagando o tabuleiro.
/// Usa um timer de WinForms (thread de UI), separado do loop do jogo.
/// </summary>
internal sealed class GameOverCurtain : IDisposable
{
    /// <summary>
    /// Altura do tabuleiro, em linhas.
    /// </summary>
    private readonly int _rows;

    /// <summary>
    /// O timer que avança a cortina.
    /// </summary>
    private readonly System.Windows.Forms.Timer _timer;

    /// <summary>
    /// Quantos passos já foram dados: na subida, as linhas cobertas; na descida, a altura mais as linhas apagadas.
    /// </summary>
    private int _step;

    /// <summary>
    /// Começa a cortina depois de uma pausa, para o jogador ver como a partida terminou.
    /// </summary>
    /// <param name="rows">Altura do tabuleiro, em linhas.</param>
    /// <param name="holdMs">Espera, em milissegundos, antes da cortina.</param>
    /// <param name="stepMs">Intervalo, em milissegundos, entre as linhas.</param>
    /// <param name="onRise">Chamado quando a cortina começa a subir (para o som acompanhar a animação).</param>
    /// <param name="onStep">Chamado a cada passo, para redesenhar.</param>
    public GameOverCurtain(int rows, int holdMs, int stepMs, Action onRise, Action onStep)
    {
        _rows = rows;
        _timer = new System.Windows.Forms.Timer { Interval = holdMs };
        _timer.Tick += (_, _) =>
        {
            _timer.Interval = stepMs;
            _step++;

            if (_step == 1)
            {
                onRise();
            }

            if (Finished)
            {
                _timer.Stop();
            }

            onStep();
        };
        _timer.Start();
    }

    /// <summary>
    /// Se a cortina já subiu e desceu.
    /// </summary>
    public bool Finished => _step >= 2 * _rows;

    /// <summary>
    /// A cor da linha na cortina: coberta (uma cor de peça por linha), já apagada, ou <c>null</c> se a cortina não chegou nela.
    /// </summary>
    /// <param name="row">A linha, de cima para baixo.</param>
    /// <returns>A cor, ou <c>null</c> para mostrar o tabuleiro.</returns>
    public PieceColor? ColorAt(int row)
    {
        var covered = row >= _rows - Math.Min(_step, _rows);
        var cleared = row < _step - _rows;

        if (cleared)
        {
            return PieceColor.None;
        }

        return covered ? Palette.PieceOrder[(_rows - 1 - row) % Palette.PieceOrder.Count] : null;
    }

    /// <summary>
    /// Para o timer.
    /// </summary>
    public void Dispose() => _timer.Dispose();
}
