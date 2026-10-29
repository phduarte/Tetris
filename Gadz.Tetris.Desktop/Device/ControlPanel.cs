using System.Drawing.Drawing2D;

namespace Gadz.Tetris.Desktop.Device;

/// <summary>
/// A área de botões da carcaça: o direcional à esquerda, os botões pequenos no meio e o botão grande à direita.
/// Calcula onde fica cada botão (para desenhar e para saber qual foi clicado) a partir do <see cref="ConsoleLayout"/> da tela.
/// As coordenadas são as do aparelho, em pixels a 96 DPI.
/// </summary>
internal static class ControlPanel
{
    /// <summary>
    /// Topo da área de botões (logo abaixo da moldura do LCD).
    /// </summary>
    public const float TOP = 476F;

    /// <summary>
    /// Centro da coluna do direcional.
    /// </summary>
    private const float DPAD_CENTER_X = 80F;

    /// <summary>
    /// Centro da coluna dos botões pequenos.
    /// </summary>
    private const float SMALLS_CENTER_X = 227F;

    /// <summary>
    /// Centro da coluna do botão grande.
    /// </summary>
    private const float BIG_CENTER_X = 360F;

    /// <summary>
    /// Lado de cada tecla do direcional (ele é uma cruz de 3×3 teclas).
    /// </summary>
    private const float DPAD_KEY = 32F;

    /// <summary>
    /// Raio dos cantos externos das teclas do direcional.
    /// </summary>
    private const float DPAD_RADIUS = 6F;

    /// <summary>
    /// Altura da sombra do direcional.
    /// </summary>
    private const float DPAD_SHADOW = 3F;

    /// <summary>
    /// Lado do triângulo das setas do direcional.
    /// </summary>
    private const float DPAD_ARROW = 9F;

    /// <summary>
    /// Distância entre o topo da área e os botões pequenos.
    /// </summary>
    private const float SMALLS_OFFSET = 22F;

    /// <summary>
    /// Largura de um botão pequeno.
    /// </summary>
    private const float PILL_WIDTH = 38F;

    /// <summary>
    /// Altura de um botão pequeno.
    /// </summary>
    private const float PILL_HEIGHT = 14F;

    /// <summary>
    /// Altura da sombra de um botão pequeno.
    /// </summary>
    private const float PILL_SHADOW = 2F;

    /// <summary>
    /// Espessura do anel que pulsa em volta de um botão pequeno.
    /// </summary>
    private const float PULSE_RING = 4F;

    /// <summary>
    /// Opacidade máxima do anel que pulsa.
    /// </summary>
    private const int PULSE_ALPHA = 153;

    /// <summary>
    /// Espaço entre os botões pequenos.
    /// </summary>
    private const float SMALLS_GAP = 12F;

    /// <summary>
    /// Distância entre o topo da área e o botão grande.
    /// </summary>
    private const float BIG_OFFSET = 6F;

    /// <summary>
    /// Diâmetro do botão grande.
    /// </summary>
    private const float BIG_SIZE = 66F;

    /// <summary>
    /// Altura da sombra do botão grande.
    /// </summary>
    private const float BIG_SHADOW = 4F;

    /// <summary>
    /// Espessura do anel escuro em volta do botão grande.
    /// </summary>
    private const float BIG_RING = 4F;

    /// <summary>
    /// Quanto o botão afunda ao ser apertado.
    /// </summary>
    private const float PRESS_DEPTH = 2F;

    /// <summary>
    /// Quanto o botão clareia ao ser apertado.
    /// </summary>
    private const float PRESS_BRIGHTNESS = 1.25F;

    /// <summary>
    /// Quanto o botão escurece enquanto está travado (toggle desligado).
    /// </summary>
    private const float LATCHED_BRIGHTNESS = 0.7F;

    /// <summary>
    /// Espaço entre um botão e a sua legenda.
    /// </summary>
    private const float CAPTION_GAP = 6F;

    /// <summary>
    /// Tamanho da fonte das legendas.
    /// </summary>
    private const float CAPTION_SIZE = 9F;

    /// <summary>
    /// Altura de cada linha das legendas.
    /// </summary>
    private const float CAPTION_LINE = 12.15F;

    /// <summary>
    /// Espaço entre as letras das legendas (0,08 em).
    /// </summary>
    private const float CAPTION_SPACING = 0.72F;

    /// <summary>
    /// Superfície só para medir as legendas fora da pintura (no clique, por exemplo).
    /// </summary>
    private static readonly Graphics _measure = Graphics.FromImage(new Bitmap(1, 1));

    /// <summary>
    /// O que fica sob o ponto: a tecla e a ação própria do botão, se houver.
    /// </summary>
    /// <param name="layout">Os botões da tela.</param>
    /// <param name="point">O ponto, nas coordenadas do aparelho.</param>
    /// <returns>O botão sob o ponto, ou <c>null</c>.</returns>
    public static ConsoleButton? HitTest(ConsoleLayout layout, PointF point)
    {
        foreach (var (key, area) in DpadKeys(layout.Dpad))
        {
            if (area.Contains(point))
            {
                return new ConsoleButton(key, string.Empty, string.Empty);
            }
        }

        foreach (var (button, area, _) in SmallSlots(layout))
        {
            if (area.Contains(point))
            {
                return button;
            }
        }

        return BigArea().Contains(point) ? layout.Big : null;
    }

    /// <summary>
    /// Desenha todos os botões e legendas da tela.
    /// </summary>
    /// <param name="g">Onde desenhar.</param>
    /// <param name="layout">Os botões da tela.</param>
    /// <param name="isPressed">Se a tecla está sendo apertada agora (o botão afunda).</param>
    /// <param name="pulse">Intensidade, de 0 a 1, do anel dos botões que pulsam.</param>
    public static void Paint(Graphics g, ConsoleLayout layout, Func<Keys, bool> isPressed, float pulse)
    {
        PaintDpad(g, layout.Dpad, isPressed);

        foreach (var (button, area, captionX) in SmallSlots(layout))
        {
            PaintPill(g, area, isPressed(button.Key), button.Latched?.Invoke() ?? false, button.Pulse ? pulse : 0);
            PaintCaption(g, captionX, area.Bottom + CAPTION_GAP, button.Label, button.Hint);
        }

        var big = BigArea();
        PaintBig(g, big, isPressed(layout.Big.Key));
        PaintCaption(g, BIG_CENTER_X, big.Bottom + CAPTION_GAP, layout.Big.Label, layout.Big.Hint);
    }

    /// <summary>
    /// As teclas ativas do direcional e onde ficam.
    /// </summary>
    private static IEnumerable<(Keys Key, RectangleF Area)> DpadKeys(DpadSetup dpad)
    {
        var left = DPAD_CENTER_X - (1.5F * DPAD_KEY);

        if (dpad.Vertical)
        {
            yield return (Keys.Up, new RectangleF(left + DPAD_KEY, TOP, DPAD_KEY, DPAD_KEY));
            yield return (Keys.Down, new RectangleF(left + DPAD_KEY, TOP + (2 * DPAD_KEY), DPAD_KEY, DPAD_KEY));
        }

        if (dpad.Horizontal)
        {
            yield return (Keys.Left, new RectangleF(left, TOP + DPAD_KEY, DPAD_KEY, DPAD_KEY));
            yield return (Keys.Right, new RectangleF(left + (2 * DPAD_KEY), TOP + DPAD_KEY, DPAD_KEY, DPAD_KEY));
        }
    }

    /// <summary>
    /// Onde fica cada botão pequeno: lado a lado, centralizados, cada um na largura da sua legenda.
    /// </summary>
    private static IEnumerable<(ConsoleButton Button, RectangleF Area, float CaptionX)> SmallSlots(ConsoleLayout layout)
    {
        var widths = layout.Smalls.Select(b => Math.Max(PILL_WIDTH, MeasureCaption(b.Label, b.Hint))).ToList();
        var x = SMALLS_CENTER_X - ((widths.Sum() + (SMALLS_GAP * Math.Max(0, widths.Count - 1))) / 2);

        for (var i = 0; i < widths.Count; i++)
        {
            var center = x + (widths[i] / 2);
            yield return (layout.Smalls[i], new RectangleF(center - (PILL_WIDTH / 2), TOP + SMALLS_OFFSET, PILL_WIDTH, PILL_HEIGHT), center);
            x += widths[i] + SMALLS_GAP;
        }
    }

    /// <summary>
    /// Onde fica o botão grande.
    /// </summary>
    private static RectangleF BigArea() => new(BIG_CENTER_X - (BIG_SIZE / 2), TOP + BIG_OFFSET, BIG_SIZE, BIG_SIZE);

    /// <summary>
    /// Desenha o direcional: a cruz, as setas (apagadas quando não fazem nada na tela) e a legenda.
    /// </summary>
    private static void PaintDpad(Graphics g, DpadSetup dpad, Func<Keys, bool> isPressed)
    {
        var left = DPAD_CENTER_X - (1.5F * DPAD_KEY);
        var keys = new (Keys Key, RectangleF Area, float[] Radii, bool Enabled)[]
        {
            (Keys.Up, new(left + DPAD_KEY, TOP, DPAD_KEY, DPAD_KEY), [DPAD_RADIUS, DPAD_RADIUS, 0, 0], dpad.Vertical),
            (Keys.Left, new(left, TOP + DPAD_KEY, DPAD_KEY, DPAD_KEY), [DPAD_RADIUS, 0, 0, DPAD_RADIUS], dpad.Horizontal),
            (Keys.Right, new(left + (2 * DPAD_KEY), TOP + DPAD_KEY, DPAD_KEY, DPAD_KEY), [0, DPAD_RADIUS, DPAD_RADIUS, 0], dpad.Horizontal),
            (Keys.Down, new(left + DPAD_KEY, TOP + (2 * DPAD_KEY), DPAD_KEY, DPAD_KEY), [0, 0, DPAD_RADIUS, DPAD_RADIUS], dpad.Vertical),
        };

        using var shadow = new SolidBrush(Palette.ButtonShadow);
        foreach (var (_, area, radii, _) in keys)
        {
            var shadowArea = area;
            shadowArea.Offset(0, DPAD_SHADOW);
            using var path = Shapes.RoundedRect(shadowArea, radii[0], radii[1], radii[2], radii[3]);
            g.FillPath(shadow, path);
        }

        foreach (var (key, area, radii, enabled) in keys)
        {
            var pressed = enabled && isPressed(key);
            var face = area;
            face.Offset(0, pressed ? PRESS_DEPTH : 0);

            using var path = Shapes.RoundedRect(face, radii[0], radii[1], radii[2], radii[3]);
            using var fill = new LinearGradientBrush(face, Brighten(Palette.DpadTop, pressed), Brighten(Palette.DpadBottom, pressed), LinearGradientMode.Vertical);
            g.FillPath(fill, path);
            PaintDpadArrow(g, key, face, enabled ? Palette.DpadArrow : Palette.DpadArrowDisabled);
        }

        var center = new RectangleF(left + DPAD_KEY, TOP + DPAD_KEY, DPAD_KEY, DPAD_KEY);
        using var centerBrush = new SolidBrush(Palette.DpadCenter);
        g.FillRectangle(centerBrush, center);

        PaintCaption(g, DPAD_CENTER_X, TOP + (3 * DPAD_KEY) + CAPTION_GAP, dpad.Label, dpad.Hint);
    }

    /// <summary>
    /// Desenha a seta de uma tecla do direcional.
    /// </summary>
    private static void PaintDpadArrow(Graphics g, Keys key, RectangleF face, Color color)
    {
        var cx = face.X + (face.Width / 2);
        var cy = face.Y + (face.Height / 2);
        var h = DPAD_ARROW / 2;

        PointF[] points = key switch
        {
            Keys.Up => [new(cx, cy - h), new(cx + h, cy + h), new(cx - h, cy + h)],
            Keys.Down => [new(cx - h, cy - h), new(cx + h, cy - h), new(cx, cy + h)],
            Keys.Left => [new(cx - h, cy), new(cx + h, cy - h), new(cx + h, cy + h)],
            _ => [new(cx - h, cy - h), new(cx + h, cy), new(cx - h, cy + h)],
        };

        using var brush = new SolidBrush(color);
        g.FillPolygon(brush, points);
    }

    /// <summary>
    /// Desenha um botão pequeno, com o anel que pulsa quando ele chama o jogador.
    /// Travado (um toggle desligado, como o SOM sem som), ele fica afundado e mais escuro.
    /// </summary>
    private static void PaintPill(Graphics g, RectangleF area, bool pressed, bool latched, float pulse)
    {
        var radius = PILL_HEIGHT / 2;
        var face = area;
        face.Offset(0, pressed || latched ? PRESS_DEPTH : 0);
        var shade = pressed ? PRESS_BRIGHTNESS : latched ? LATCHED_BRIGHTNESS : 1F;

        if (pulse > 0)
        {
            var ring = face;
            ring.Inflate(PULSE_RING / 2, PULSE_RING / 2);
            using var ringPath = Shapes.RoundedRect(ring, radius + (PULSE_RING / 2));
            using var pen = new Pen(Color.FromArgb((int)(PULSE_ALPHA * pulse), Palette.Accent), PULSE_RING);
            g.DrawPath(pen, ringPath);
        }

        var shadowArea = area;
        shadowArea.Offset(0, PILL_SHADOW);
        using (var shadowPath = Shapes.RoundedRect(shadowArea, radius))
        using (var shadow = new SolidBrush(Palette.ButtonShadow))
        {
            g.FillPath(shadow, shadowPath);
        }

        using var path = Shapes.RoundedRect(face, radius);
        using var fill = new LinearGradientBrush(face, Shade(Palette.PillTop, shade), Shade(Palette.PillBottom, shade), LinearGradientMode.Vertical);
        g.FillPath(fill, path);
    }

    /// <summary>
    /// Desenha o botão grande: anel escuro, "chão" coral e a face com brilho no canto superior esquerdo.
    /// </summary>
    private static void PaintBig(Graphics g, RectangleF area, bool pressed)
    {
        var ring = area;
        ring.Inflate(BIG_RING, BIG_RING);
        using (var ringBrush = new SolidBrush(Palette.ButtonRing))
        {
            g.FillEllipse(ringBrush, ring);
        }

        var floor = area;
        floor.Offset(0, BIG_SHADOW);
        using (var floorBrush = new SolidBrush(Palette.AccentShadow))
        {
            g.FillEllipse(floorBrush, floor);
        }

        var face = area;
        face.Offset(0, pressed ? PRESS_DEPTH : 0);
        using var path = new GraphicsPath();
        path.AddEllipse(face);
        using var fill = new PathGradientBrush(path)
        {
            CenterPoint = new PointF(face.X + (face.Width * 0.35F), face.Y + (face.Height * 0.3F)),
            InterpolationColors = new ColorBlend
            {
                Colors = [Brighten(Palette.AccentDark, pressed), Brighten(Palette.Accent, pressed), Brighten(Palette.AccentLight, pressed)],
                Positions = [0F, 0.55F, 1F],
            },
        };
        g.FillEllipse(fill, face);
    }

    /// <summary>
    /// Desenha a legenda de um botão: a ação em branco (uma ou mais linhas) e a tecla em coral.
    /// </summary>
    private static void PaintCaption(Graphics g, float centerX, float y, string label, string hint)
    {
        var font = Fonts.Bold(CAPTION_SIZE);
        using var text = new SolidBrush(Palette.CaseText);
        using var accent = new SolidBrush(Palette.Accent);

        foreach (var line in CaptionLines(label))
        {
            TextPainter.DrawCentered(g, line, font, text, centerX, y, CAPTION_SPACING);
            y += CAPTION_LINE;
        }

        if (hint.Length > 0)
        {
            TextPainter.DrawCentered(g, hint.ToUpperInvariant(), font, accent, centerX, y, CAPTION_SPACING);
        }
    }

    /// <summary>
    /// Largura da legenda mais larga entre as linhas da ação e a da tecla.
    /// </summary>
    private static float MeasureCaption(string label, string hint)
    {
        var font = Fonts.Bold(CAPTION_SIZE);
        return CaptionLines(label).Append(hint.ToUpperInvariant()).Max(line => TextPainter.Measure(_measure, line, font, CAPTION_SPACING));
    }

    /// <summary>
    /// As linhas da ação, em caixa alta.
    /// </summary>
    private static IEnumerable<string> CaptionLines(string label) => label.ToUpperInvariant().Split('\n', StringSplitOptions.RemoveEmptyEntries);

    /// <summary>
    /// A cor clareada, se o botão estiver apertado.
    /// </summary>
    private static Color Brighten(Color color, bool pressed) => pressed ? Shade(color, PRESS_BRIGHTNESS) : color;

    /// <summary>
    /// A cor com o brilho multiplicado pelo fator (acima de 1 clareia, abaixo escurece).
    /// </summary>
    private static Color Shade(Color color, float factor)
        => Color.FromArgb(color.A, ShadeChannel(color.R, factor), ShadeChannel(color.G, factor), ShadeChannel(color.B, factor));

    /// <summary>
    /// Um canal de cor (0–255) multiplicado pelo fator, sem passar de 255.
    /// </summary>
    private static int ShadeChannel(int value, float factor) => Math.Min(byte.MaxValue, (int)(value * factor));
}
