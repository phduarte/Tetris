namespace Gadz.Tetris.Desktop.Device;

/// <summary>
/// Um botão da carcaça: a tecla que ele aperta e a legenda (ação em branco, tecla em coral).
/// </summary>
/// <param name="Key">A tecla equivalente. Clicar no botão tem o mesmo efeito de apertá-la, e apertá-la anima o botão.</param>
/// <param name="Label">A ação, na primeira linha da legenda.</param>
/// <param name="Hint">A tecla, na segunda linha da legenda.</param>
/// <param name="Pulse">Se o botão pulsa, chamando o jogador (por exemplo, PAUSA durante a pausa).</param>
/// <param name="Latched">Se o botão está travado agora (um toggle desligado, que fica afundado). Consultado a cada pintura.</param>
internal sealed record ConsoleButton(Keys Key, string Label, string Hint, bool Pulse = false, Func<bool>? Latched = null);

/// <summary>
/// A legenda e as setas ativas do direcional.
/// </summary>
/// <param name="Horizontal">Se ◀ ▶ fazem algo na tela.</param>
/// <param name="Vertical">Se ▲ ▼ fazem algo na tela.</param>
/// <param name="Label">As linhas da legenda (brancas).</param>
/// <param name="Hint">A linha final da legenda (coral).</param>
internal sealed record DpadSetup(bool Horizontal, bool Vertical, string Label, string Hint = "");

/// <summary>
/// Os botões da carcaça numa tela: o direcional, os botões pequenos do meio e o botão grande.
/// </summary>
/// <param name="Dpad">O direcional.</param>
/// <param name="Smalls">Os botões pequenos, da esquerda para a direita.</param>
/// <param name="Big">O botão grande (a ação principal da tela).</param>
internal sealed record ConsoleLayout(DpadSetup Dpad, IReadOnlyList<ConsoleButton> Smalls, ConsoleButton Big);
