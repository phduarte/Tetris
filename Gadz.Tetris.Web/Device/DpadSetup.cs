namespace Gadz.Tetris.Web.Device;

/// <summary>
/// A legenda e as setas ativas do direcional. Clicar numa seta equivale a apertar a seta do teclado.
/// </summary>
/// <param name="Horizontal">Se ◀ ▶ fazem algo na tela.</param>
/// <param name="Vertical">Se ▲ ▼ fazem algo na tela.</param>
/// <param name="Label">As linhas da legenda.</param>
/// <param name="Hint">A linha final da legenda, em destaque.</param>
public sealed record DpadSetup(bool Horizontal, bool Vertical, string Label, string Hint = "");
