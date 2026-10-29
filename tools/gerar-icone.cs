#:property TargetFramework=net10.0-windows
#:property UseWindowsForms=true
#:property PublishAot=false

// Gera o ícone do jogo (Gadz.Tetris.Desktop/tetris.ico) desenhando-o em cada tamanho, para ficar nítido também em 16 px.
// Uso: dotnet run tools/gerar-icone.cs -- Gadz.Tetris.Desktop/tetris.ico

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

int[] sizes = [16, 24, 32, 48, 64, 128, 256];
var output = args.Length > 0 ? args[0] : "tetris.ico";

// Mesmas cores da carcaça e do estilo TETRIS do jogo (Device/Palette.cs).
var caseTop = ColorTranslator.FromHtml("#37383e");
var caseBottom = ColorTranslator.FromHtml("#25262a");
var lcd = ColorTranslator.FromHtml("#11161a");
var ghost = ColorTranslator.FromHtml("#dfe8e1");
var colors = new Dictionary<char, Color>
{
    ['T'] = ColorTranslator.FromHtml("#ff7a59"),
    ['L'] = ColorTranslator.FromHtml("#f7953b"),
    ['O'] = ColorTranslator.FromHtml("#f5cc3c"),
};

// Um "LCD" 4×4: a peça T no alto (na cor da marca) e uma pilha embaixo.
string[] grid =
[
    ".TTT",
    "..T.",
    "L.OO",
    "LLOO",
];

var images = sizes.Select(size => (size, png: Render(size))).ToList();
using (var file = File.Create(output))
using (var writer = new BinaryWriter(file))
{
    writer.Write((short)0);
    writer.Write((short)1);
    writer.Write((short)images.Count);

    var offset = 6 + (16 * images.Count);
    foreach (var (size, png) in images)
    {
        writer.Write((byte)(size >= 256 ? 0 : size));
        writer.Write((byte)(size >= 256 ? 0 : size));
        writer.Write((byte)0);
        writer.Write((byte)0);
        writer.Write((short)1);
        writer.Write((short)32);
        writer.Write(png.Length);
        writer.Write(offset);
        offset += png.Length;
    }

    foreach (var (_, png) in images)
    {
        writer.Write(png);
    }
}

Console.WriteLine($"{output}: {string.Join(", ", sizes)} px");

byte[] Render(int size)
{
    using var bitmap = new Bitmap(size, size, PixelFormat.Format32bppArgb);
    using var g = Graphics.FromImage(bitmap);
    g.SmoothingMode = SmoothingMode.AntiAlias;
    g.PixelOffsetMode = PixelOffsetMode.HighQuality;

    // nos tamanhos pequenos a carcaça vira só uma borda fina, para sobrar espaço para as peças
    var border = Math.Max(1f, size * 0.09f);
    var radius = size * 0.22f;
    using (var path = RoundedRect(new RectangleF(0, 0, size, size), radius))
    using (var brush = new LinearGradientBrush(new PointF(0, 0), new PointF(0, size), caseTop, caseBottom))
    {
        g.FillPath(brush, path);
    }

    var screen = new RectangleF(border, border, size - (2 * border), size - (2 * border));
    using (var path = RoundedRect(screen, Math.Max(1f, radius - border)))
    using (var brush = new SolidBrush(lcd))
    {
        g.FillPath(brush, path);
    }

    var gap = Math.Max(1f, size / 40f);
    var inner = screen.Width * 0.08f;
    var cell = (screen.Width - (2 * inner) - (3 * gap)) / 4f;
    g.SmoothingMode = SmoothingMode.None;

    for (var y = 0; y < 4; y++)
    {
        for (var x = 0; x < 4; x++)
        {
            var rect = new RectangleF(screen.X + inner + (x * (cell + gap)), screen.Y + inner + (y * (cell + gap)), cell, cell);
            var type = grid[y][x];

            if (type == '.')
            {
                using var off = new SolidBrush(Color.FromArgb(18, ghost));
                g.FillRectangle(off, rect);
                continue;
            }

            using (var fill = new SolidBrush(colors[type]))
            {
                g.FillRectangle(fill, rect);
            }

            // relevo do bloco: claro em cima e à esquerda, escuro embaixo e à direita
            if (size >= 32)
            {
                var bevel = Math.Max(1f, cell * 0.14f);
                using var light = new SolidBrush(Color.FromArgb(120, Color.White));
                using var dark = new SolidBrush(Color.FromArgb(90, Color.Black));
                g.FillRectangle(light, rect.X, rect.Y, rect.Width, bevel);
                g.FillRectangle(light, rect.X, rect.Y, bevel, rect.Height);
                g.FillRectangle(dark, rect.X, rect.Bottom - bevel, rect.Width, bevel);
                g.FillRectangle(dark, rect.Right - bevel, rect.Y, bevel, rect.Height);
            }
        }
    }

    using var stream = new MemoryStream();
    bitmap.Save(stream, ImageFormat.Png);
    return stream.ToArray();
}

static GraphicsPath RoundedRect(RectangleF r, float radius)
{
    var d = radius * 2;
    var path = new GraphicsPath();
    path.AddArc(r.X, r.Y, d, d, 180, 90);
    path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
    path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
    path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
    path.CloseFigure();
    return path;
}
