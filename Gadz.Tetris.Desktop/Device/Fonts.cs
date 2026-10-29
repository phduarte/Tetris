using System.Drawing.Text;
using System.Runtime.InteropServices;

namespace Gadz.Tetris.Desktop.Device;

/// <summary>
/// Fontes do aparelho: a Silkscreen (pixel, licença OFL, embutida nos recursos) para o LCD e a Segoe UI para a carcaça.
/// Os tamanhos são em pixels de tela a 96 DPI; o <see cref="ConsoleFrame"/> escala o desenho para o DPI do monitor.
/// </summary>
internal static class Fonts
{
    /// <summary>
    /// Fonte da carcaça e dos rótulos pequenos do LCD.
    /// </summary>
    private static readonly FontFamily _ui = new("Segoe UI");

    /// <summary>
    /// A coleção com a Silkscreen carregada da memória.
    /// </summary>
    private static readonly PrivateFontCollection _collection = LoadPixelFont();

    /// <summary>
    /// Fontes já criadas, para não recriar um <see cref="Font"/> a cada pintura.
    /// </summary>
    private static readonly Dictionary<(string Family, float Size, FontStyle Style), Font> _cache = [];

    /// <summary>
    /// Fonte pixel do LCD (sempre em caixa alta no desenho).
    /// </summary>
    /// <param name="size">Tamanho em pixels.</param>
    /// <returns>A fonte.</returns>
    public static Font Pixel(float size) => Get(_collection.Families[0], size, FontStyle.Regular);

    /// <summary>
    /// Fonte em negrito da carcaça e dos rótulos do LCD.
    /// </summary>
    /// <param name="size">Tamanho em pixels.</param>
    /// <returns>A fonte.</returns>
    public static Font Bold(float size) => Get(_ui, size, FontStyle.Bold);

    /// <summary>
    /// Fonte regular para textos corridos no LCD (descrição e licença).
    /// </summary>
    /// <param name="size">Tamanho em pixels.</param>
    /// <returns>A fonte.</returns>
    public static Font Regular(float size) => Get(_ui, size, FontStyle.Regular);

    /// <summary>
    /// Busca a fonte no cache ou a cria.
    /// </summary>
    /// <param name="family">A família.</param>
    /// <param name="size">Tamanho em pixels.</param>
    /// <param name="style">O estilo.</param>
    /// <returns>A fonte.</returns>
    private static Font Get(FontFamily family, float size, FontStyle style)
    {
        var key = (family.Name, size, style);

        if (!_cache.TryGetValue(key, out var font))
        {
            font = new Font(family, size, style, GraphicsUnit.Pixel);
            _cache[key] = font;
        }

        return font;
    }

    /// <summary>
    /// Carrega a Silkscreen dos recursos. A memória fica alocada enquanto o jogo roda, como exige o GDI+.
    /// </summary>
    /// <returns>A coleção com a fonte.</returns>
    private static PrivateFontCollection LoadPixelFont()
    {
        var data = Properties.Resources.Silkscreen;
        var memory = Marshal.AllocCoTaskMem(data.Length);
        Marshal.Copy(data, 0, memory, data.Length);

        var collection = new PrivateFontCollection();
        collection.AddMemoryFont(memory, data.Length);
        return collection;
    }
}
