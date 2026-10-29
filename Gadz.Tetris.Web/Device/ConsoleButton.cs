namespace Gadz.Tetris.Web.Device;

/// <summary>
/// Um botão da carcaça. Clicar nele equivale a apertar a tecla dele.
/// </summary>
/// <param name="Key">A tecla que o botão aperta, como em <c>KeyboardEvent.key</c>.</param>
/// <param name="Caption">A legenda do botão.</param>
/// <param name="KeyCap">O nome da tecla, mostrado sob a legenda.</param>
/// <param name="Big">Se é o botão grande (a ação principal da tela).</param>
/// <param name="Latched">Se o botão fica afundado (ex.: SOM com o som desligado).</param>
public sealed record ConsoleButton(string Key, string Caption, string KeyCap, bool Big = false, bool Latched = false);
