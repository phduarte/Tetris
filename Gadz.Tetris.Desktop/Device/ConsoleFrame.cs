using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using Gadz.Tetris.Application;
using Gadz.Tetris.Domain.Models.Statistics;
using Gadz.Tetris.Desktop.Screens;
using Texto = Gadz.Tetris.Desktop.Textos.Jogo;

namespace Gadz.Tetris.Desktop.Device;

/// <summary>
/// O aparelho: a janela sem borda com a carcaça, o LCD e os botões, igual em todo o jogo. Só o conteúdo do LCD muda,
/// conforme a <see cref="ConsoleScreen"/> exibida. Tudo é desenhado em pixels a 96 DPI e escalado para o DPI do monitor.
/// </summary>
internal sealed class ConsoleFrame : Form
{
    /// <summary>
    /// Largura do aparelho.
    /// </summary>
    private const float WIDTH = 426F;

    /// <summary>
    /// Altura do aparelho.
    /// </summary>
    private const float HEIGHT = 646F;

    /// <summary>
    /// DPI em que as medidas foram desenhadas.
    /// </summary>
    private const float BASE_DPI = 96F;

    /// <summary>
    /// Raio dos cantos da carcaça.
    /// </summary>
    private const float CASE_RADIUS = 26F;

    /// <summary>
    /// Margem lateral da carcaça.
    /// </summary>
    private const float SIDE = 24F;

    /// <summary>
    /// Altura da faixa superior (logotipo, marca e botão de fechar), por onde a janela é arrastada.
    /// </summary>
    private const float TOP_BAR = 48F;

    /// <summary>
    /// Espessura da moldura em volta do LCD.
    /// </summary>
    private const float BEZEL_PADDING = 14F;

    /// <summary>
    /// Raio dos cantos da moldura.
    /// </summary>
    private const float BEZEL_RADIUS = 14F;

    /// <summary>
    /// Raio dos cantos do LCD.
    /// </summary>
    private const float LCD_RADIUS = 4F;

    /// <summary>
    /// Altura da sombra interna no topo do LCD.
    /// </summary>
    private const float LCD_SHADOW = 10F;

    /// <summary>
    /// Opacidade da sombra interna no topo do LCD.
    /// </summary>
    private const int LCD_SHADOW_ALPHA = 90;

    /// <summary>
    /// Lado dos quadradinhos coloridos do logotipo.
    /// </summary>
    private const float LOGO_SQUARE = 7F;

    /// <summary>
    /// Espaço entre os quadradinhos do logotipo.
    /// </summary>
    private const float LOGO_GAP = 3F;

    /// <summary>
    /// Espaço entre o logotipo e a marca.
    /// </summary>
    private const float BRAND_GAP = 10F;

    /// <summary>
    /// Tamanho da fonte da marca.
    /// </summary>
    private const float BRAND_SIZE = 12F;

    /// <summary>
    /// Espaço entre as letras da marca (0,32 em).
    /// </summary>
    private const float BRAND_SPACING = BRAND_SIZE * 0.32F;

    /// <summary>
    /// Nome do fabricante na marca, antes de "TETRIS".
    /// </summary>
    private const string BRAND_MAKER = "GADZ ";

    /// <summary>
    /// Diâmetro do botão de fechar.
    /// </summary>
    private const float CLOSE_SIZE = 20F;

    /// <summary>
    /// Metade do "X" do botão de fechar.
    /// </summary>
    private const float CLOSE_MARK = 3.5F;

    /// <summary>
    /// Opacidade do brilho na borda de cima da carcaça.
    /// </summary>
    private const int CASE_HIGHLIGHT_ALPHA = 20;

    /// <summary>
    /// Intervalo, em milissegundos, entre os quadros das animações (piscar, pulsar, botões apertados).
    /// </summary>
    private const int TICK_MS = 40;

    /// <summary>
    /// Período, em milissegundos, dos textos que piscam (metade aceso, metade apagado).
    /// </summary>
    private const long BLINK_PERIOD_MS = 1100;

    /// <summary>
    /// Período, em milissegundos, do anel dos botões que pulsam.
    /// </summary>
    private const double PULSE_PERIOD_MS = 1200;

    /// <summary>
    /// Por quanto tempo, em milissegundos, um botão fica afundado depois de apertado.
    /// </summary>
    private const long PRESS_MS = 120;

    /// <summary>
    /// A moldura do LCD.
    /// </summary>
    private static readonly RectangleF _bezel = new(SIDE, TOP_BAR, WIDTH - (2 * SIDE), LcdCanvas.HEIGHT + (2 * BEZEL_PADDING));

    /// <summary>
    /// O LCD.
    /// </summary>
    private static readonly RectangleF _lcd = new(SIDE + BEZEL_PADDING, TOP_BAR + BEZEL_PADDING, LcdCanvas.WIDTH, LcdCanvas.HEIGHT);

    /// <summary>
    /// O botão de fechar.
    /// </summary>
    private static readonly RectangleF _close = new(WIDTH - SIDE - CLOSE_SIZE, (TOP_BAR - CLOSE_SIZE) / 2, CLOSE_SIZE, CLOSE_SIZE);

    /// <summary>
    /// Relógio das animações. É um timer de WinForms: dispara na thread de UI, separado do loop do jogo.
    /// </summary>
    private readonly System.Windows.Forms.Timer _clock = new() { Interval = TICK_MS };

    /// <summary>
    /// Até quando (em <see cref="Environment.TickCount64"/>) cada botão fica afundado.
    /// </summary>
    private readonly Dictionary<Keys, long> _pressedUntil = [];

    /// <summary>
    /// As teclas seguradas agora (KeyDown sem o KeyUp), para saber quando um KeyDown é repetição automática.
    /// </summary>
    private readonly HashSet<Keys> _heldKeys = [];

    /// <summary>
    /// A tela exibida.
    /// </summary>
    private ConsoleScreen _screen;

    /// <summary>
    /// Fator entre o DPI do monitor e o de desenho.
    /// </summary>
    private float _scale = 1F;

    /// <summary>
    /// Se o texto que pisca estava aceso no último quadro, para só redesenhar quando ele muda.
    /// </summary>
    private bool _lastBlink;

    /// <summary>
    /// Onde o arraste da janela começou (posição do cursor e da janela), ou <c>null</c> fora de um arraste.
    /// </summary>
    private (Point Cursor, Point Window)? _drag;

    /// <summary>
    /// Monta o aparelho e liga no menu.
    /// </summary>
    /// <param name="sound">Os sons do aparelho, usados por todas as telas e partidas.</param>
    /// <param name="repository">Onde as estatísticas das partidas são guardadas.</param>
    public ConsoleFrame(ISoundPlayer sound, IStatsRepository repository)
    {
        Sound = sound;
        Repository = repository;
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
        FormBorderStyle = FormBorderStyle.None;
        AutoScaleMode = AutoScaleMode.None;
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = Palette.CaseBottom;
        Icon = Properties.Resources.Icone;
        Text = Texto.Nome.ToUpperInvariant();
        ApplyScale();

        _clock.Tick += OnTick;
        _clock.Start();

        _screen = new MenuScreen(this);
        _screen.Enter();
    }

    /// <summary>
    /// Os sons do aparelho. Uma única instância, para o som desligado valer em todas as telas e partidas.
    /// </summary>
    public ISoundPlayer Sound { get; }

    /// <summary>
    /// Onde as estatísticas das partidas são guardadas; repassado ao <see cref="GameController"/> de cada partida.
    /// </summary>
    public IStatsRepository Repository { get; }

    /// <summary>
    /// O estilo do LCD, escolhido no menu e aplicado a todas as telas.
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Skin Skin
    {
        get;
        set
        {
            field = value;
            Invalidate();
        }
    }

    /// <summary>
    /// Se os textos que piscam estão acesos neste instante.
    /// </summary>
    private static bool BlinkOn => Environment.TickCount64 % BLINK_PERIOD_MS < BLINK_PERIOD_MS / 2;

    /// <summary>
    /// Troca a tela exibida no LCD.
    /// </summary>
    /// <param name="screen">A nova tela.</param>
    public void Navigate(ConsoleScreen screen)
    {
        _screen.Leave();
        _screen = screen;
        _screen.Enter();
        Invalidate();
    }

    /// <summary>
    /// Desenha o aparelho inteiro: carcaça, faixa superior, moldura, LCD e botões.
    /// </summary>
    /// <param name="e">Dados da pintura.</param>
    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.PixelOffsetMode = PixelOffsetMode.Half;
        // sem GridFit: o ajuste à grade de pixels deixa o espaçamento entre as letras irregular nos textos pequenos
        g.TextRenderingHint = TextRenderingHint.AntiAlias;
        g.ScaleTransform(_scale, _scale);

        PaintCase(g);
        PaintTopBar(g);
        PaintLcd(g);
        ControlPanel.Paint(g, _screen.Layout, IsPressed, Pulse());
    }

    /// <summary>
    /// A pintura é toda feita em <see cref="OnPaint"/>; pular o fundo evita piscar.
    /// </summary>
    /// <param name="e">Dados da pintura.</param>
    protected override void OnPaintBackground(PaintEventArgs e)
    {
        // vazio de propósito: o OnPaint cobre a janela inteira, e apagar o fundo antes faria a tela piscar
    }

    /// <summary>
    /// Todas as teclas (setas, Enter, Esc, espaço) vão para <see cref="OnKeyDown"/>, e não para a navegação entre controles.
    /// </summary>
    /// <param name="keyData">A tecla.</param>
    /// <returns>Sempre <c>true</c>.</returns>
    protected override bool IsInputKey(Keys keyData) => true;

    /// <summary>
    /// Afunda o botão da tecla e repassa a tecla à tela, dizendo se é uma repetição da tecla segurada.
    /// </summary>
    /// <param name="e">A tecla.</param>
    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);

        // o Windows repete o KeyDown enquanto a tecla está segurada; só o primeiro chega com ela ainda solta
        var repeat = !_heldKeys.Add(e.KeyCode);
        Press(e.KeyCode);
        e.Handled = _screen.HandleKey(e.KeyCode, e.Control, repeat);
        Invalidate();
    }

    /// <summary>
    /// A tecla foi solta: o próximo KeyDown dela é um toque novo.
    /// </summary>
    /// <param name="e">A tecla.</param>
    protected override void OnKeyUp(KeyEventArgs e)
    {
        base.OnKeyUp(e);
        _heldKeys.Remove(e.KeyCode);
    }

    /// <summary>
    /// Clique: fecha, começa a arrastar a janela pela faixa superior ou aperta o botão sob o cursor.
    /// </summary>
    /// <param name="e">O clique.</param>
    protected override void OnMouseDown(MouseEventArgs e)
    {
        base.OnMouseDown(e);

        if (e.Button != MouseButtons.Left)
        {
            return;
        }

        var point = new PointF(e.X / _scale, e.Y / _scale);

        if (_close.Contains(point))
        {
            _screen.Close();
            return;
        }

        if (point.Y < TOP_BAR)
        {
            _drag = (Cursor.Position, Location);
            return;
        }

        var button = ControlPanel.HitTest(_screen.Layout, point);
        if (button is null)
        {
            return;
        }

        Press(button.Key);
        _screen.HandleKey(button.Key, false, repeat: false);
        Invalidate();
    }

    /// <summary>
    /// Arrasta a janela enquanto o botão do mouse estiver apertado na faixa superior.
    /// </summary>
    /// <param name="e">O movimento.</param>
    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);

        if (_drag is { } drag)
        {
            Location = new Point(drag.Window.X + Cursor.Position.X - drag.Cursor.X, drag.Window.Y + Cursor.Position.Y - drag.Cursor.Y);
        }
    }

    /// <summary>
    /// Termina o arraste da janela.
    /// </summary>
    /// <param name="e">O clique.</param>
    protected override void OnMouseUp(MouseEventArgs e)
    {
        base.OnMouseUp(e);
        _drag = null;
    }

    /// <summary>
    /// Avisa a tela que a janela perdeu o foco (a partida pausa sozinha).
    /// </summary>
    /// <param name="e">Dados do evento.</param>
    protected override void OnDeactivate(EventArgs e)
    {
        base.OnDeactivate(e);

        // fora de foco o KeyUp não chega: sem limpar, a próxima tecla seria tomada por repetição
        _heldKeys.Clear();
        _screen.Deactivate();
    }

    /// <summary>
    /// Recalcula o tamanho quando a janela vai para um monitor com outro DPI.
    /// </summary>
    /// <param name="e">Os DPIs antigo e novo.</param>
    protected override void OnDpiChanged(DpiChangedEventArgs e)
    {
        base.OnDpiChanged(e);
        ApplyScale();
    }

    /// <summary>
    /// Aplica a escala do monitor assim que a janela existe.
    /// </summary>
    /// <param name="e">Dados do evento.</param>
    protected override void OnHandleCreated(EventArgs e)
    {
        base.OnHandleCreated(e);
        ApplyScale();
    }

    /// <summary>
    /// Desliga a tela exibida e o relógio das animações.
    /// </summary>
    /// <param name="e">Dados do evento.</param>
    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        _screen.Leave();
        _clock.Dispose();
        base.OnFormClosed(e);
    }

    /// <summary>
    /// Ajusta o tamanho da janela e o recorte arredondado ao DPI do monitor.
    /// </summary>
    private void ApplyScale()
    {
        _scale = DeviceDpi / BASE_DPI;
        ClientSize = new Size((int)Math.Ceiling(WIDTH * _scale), (int)Math.Ceiling(HEIGHT * _scale));

        using var outline = Shapes.RoundedRect(new RectangleF(0, 0, WIDTH, HEIGHT), CASE_RADIUS);
        using var matrix = new Matrix();
        matrix.Scale(_scale, _scale);
        outline.Transform(matrix);
        Region = new Region(outline);
        Invalidate();
    }

    /// <summary>
    /// A cada quadro, redesenha só se algo estiver se mexendo: texto piscando, botão afundado ou pulsando, ou animação da tela.
    /// </summary>
    private void OnTick(object? sender, EventArgs e)
    {
        var now = Environment.TickCount64;
        var released = _pressedUntil.Where(p => p.Value <= now).Select(p => p.Key).ToList();
        released.ForEach(key => _pressedUntil.Remove(key));

        var blink = BlinkOn;
        var pulsing = _screen.Layout.Smalls.Any(b => b.Pulse);

        if (blink != _lastBlink || released.Count > 0 || _pressedUntil.Count > 0 || pulsing || _screen.IsAnimating)
        {
            _lastBlink = blink;
            Invalidate();
        }
    }

    /// <summary>
    /// Afunda por um instante o botão da tecla.
    /// </summary>
    private void Press(Keys key) => _pressedUntil[key] = Environment.TickCount64 + PRESS_MS;

    /// <summary>
    /// Se o botão da tecla está afundado.
    /// </summary>
    private bool IsPressed(Keys key) => _pressedUntil.ContainsKey(key);

    /// <summary>
    /// Intensidade atual, de 0 a 1, do anel dos botões que pulsam.
    /// </summary>
    private static float Pulse() => (float)((1 - Math.Cos(2 * Math.PI * (Environment.TickCount64 % (long)PULSE_PERIOD_MS) / PULSE_PERIOD_MS)) / 2);

    /// <summary>
    /// Desenha a carcaça grafite, com um brilho fino na borda de cima.
    /// </summary>
    private static void PaintCase(Graphics g)
    {
        var area = new RectangleF(0, 0, WIDTH, HEIGHT);
        using (var path = Shapes.RoundedRect(area, CASE_RADIUS))
        using (var fill = new LinearGradientBrush(area, Palette.CaseTop, Palette.CaseBottom, LinearGradientMode.Vertical))
        {
            g.FillPath(fill, path);
        }

        using var highlight = new Pen(Color.FromArgb(CASE_HIGHLIGHT_ALPHA, Color.White));
        g.DrawLine(highlight, CASE_RADIUS, 1, WIDTH - CASE_RADIUS, 1);

        using var bezelPath = Shapes.RoundedRect(_bezel, BEZEL_RADIUS);
        using var bezel = new SolidBrush(Palette.Bezel);
        g.FillPath(bezel, bezelPath);
    }

    /// <summary>
    /// Desenha a faixa superior: o logotipo com as cores das 7 peças, a marca e o botão de fechar.
    /// </summary>
    private static void PaintTopBar(Graphics g)
    {
        var x = SIDE;
        var y = (TOP_BAR - LOGO_SQUARE) / 2;

        foreach (var color in Palette.PieceOrder)
        {
            using var brush = new SolidBrush(Palette.Of(color));
            g.FillRectangle(brush, x, y, LOGO_SQUARE, LOGO_SQUARE);
            x += LOGO_SQUARE + LOGO_GAP;
        }

        var font = Fonts.Bold(BRAND_SIZE);
        var textY = (TOP_BAR - font.GetHeight(g)) / 2;
        x += BRAND_GAP - LOGO_GAP;

        using var maker = new SolidBrush(Palette.CaseText);
        using var accent = new SolidBrush(Palette.Accent);
        TextPainter.Draw(g, BRAND_MAKER, font, maker, x, textY, BRAND_SPACING);
        x += TextPainter.Measure(g, BRAND_MAKER, font, BRAND_SPACING) + BRAND_SPACING;
        TextPainter.Draw(g, Texto.Nome.ToUpperInvariant(), font, accent, x, textY, BRAND_SPACING);

        using var closeBrush = new SolidBrush(Palette.CloseButton);
        g.FillEllipse(closeBrush, _close);

        var cx = _close.X + (_close.Width / 2);
        var cy = _close.Y + (_close.Height / 2);
        using var mark = new Pen(Palette.CloseMark, 1.5F);
        g.DrawLine(mark, cx - CLOSE_MARK, cy - CLOSE_MARK, cx + CLOSE_MARK, cy + CLOSE_MARK);
        g.DrawLine(mark, cx - CLOSE_MARK, cy + CLOSE_MARK, cx + CLOSE_MARK, cy - CLOSE_MARK);
    }

    /// <summary>
    /// Desenha o LCD: fundo do estilo, o conteúdo da tela (recortado aos cantos do LCD) e uma sombra interna no topo.
    /// </summary>
    private void PaintLcd(Graphics g)
    {
        var theme = LcdTheme.For(Skin);
        var state = g.Save();

        using (var outline = Shapes.RoundedRect(_lcd, LCD_RADIUS))
        using (var background = new SolidBrush(theme.Background))
        {
            g.FillPath(background, outline);
            g.SetClip(outline);
        }

        g.TranslateTransform(_lcd.X, _lcd.Y);
        using (var lcd = new LcdCanvas(g, theme, BlinkOn))
        {
            _screen.Paint(lcd);
        }

        // o pincel é um pouco maior que a área pintada: o degradê se repete na borda dele e deixaria uma linha escura
        var shadowArea = new RectangleF(0, 0, LcdCanvas.WIDTH, LCD_SHADOW);
        var brushArea = RectangleF.Inflate(shadowArea, 0, 1);
        using var shadow = new LinearGradientBrush(brushArea, Color.FromArgb(LCD_SHADOW_ALPHA, Color.Black), Color.Transparent, LinearGradientMode.Vertical);
        g.FillRectangle(shadow, shadowArea);

        g.Restore(state);
    }
}
