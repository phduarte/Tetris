using Gadz.Tetris.Domain.Models.Pieces;

namespace Gadz.Tetris.Domain.Models.Boards;

/// <summary>
/// Grade com os blocos já assentados no tabuleiro. Cuida de colisão e da limpeza de linhas completas;
/// a peça em movimento fica fora daqui, sob controle do <see cref="Board"/>.
/// </summary>
internal class Playfield
{
    /// <summary>
    /// Máximo de linhas que uma única peça consegue completar (a peça I em pé ocupa 4 linhas).
    /// </summary>
    private const int MAX_LINES_PER_DROP = 4;

    /// <summary>
    /// Cor de cada célula; <see cref="PieceColor.None"/> indica célula vazia.
    /// </summary>
    private readonly PieceColor[,] _cells;

    /// <summary>
    /// Dimensão da grade.
    /// </summary>
    public Size Dimension { get; }

    /// <summary>
    /// Os blocos assentados, linha a linha de cima para baixo.
    /// </summary>
    public IReadOnlyList<Block> Blocks =>
    [
        .. from y in Enumerable.Range(0, Dimension.Height)
           from x in Enumerable.Range(0, Dimension.Width)
           where _cells[x, y] != PieceColor.None
           select new Block(x, y, _cells[x, y])
    ];

    /// <summary>
    /// Cria uma grade vazia.
    /// </summary>
    /// <param name="dimension">Largura e altura da grade.</param>
    public Playfield(Size dimension) : this(dimension, [])
    {
    }

    /// <summary>
    /// Cria uma grade com os blocos informados já assentados.
    /// </summary>
    /// <param name="dimension">Largura e altura da grade.</param>
    /// <param name="blocks">Os blocos assentados.</param>
    public Playfield(Size dimension, IEnumerable<Block> blocks)
    {
        Dimension = dimension;
        _cells = new PieceColor[dimension.Width, dimension.Height];
        Clear();

        foreach (var block in blocks)
        {
            Lock(block);
        }
    }

    /// <summary>
    /// Esvazia todas as células.
    /// </summary>
    public void Clear()
    {
        for (var y = 0; y < Dimension.Height; y++)
        {
            for (var x = 0; x < Dimension.Width; x++)
            {
                _cells[x, y] = PieceColor.None;
            }
        }
    }

    /// <summary>
    /// Informa se o bloco está dentro dos limites da grade.
    /// </summary>
    /// <param name="block">O bloco a verificar.</param>
    /// <returns><c>true</c> se o bloco está dentro da grade.</returns>
    public bool Contains(Block block)
    {
        return block.X >= 0 && block.X < Dimension.Width && block.Y >= 0 && block.Y < Dimension.Height;
    }

    /// <summary>
    /// Informa se algum bloco da peça ocupa uma célula já preenchida. Blocos fora da grade são ignorados.
    /// </summary>
    /// <param name="piece">A peça a verificar.</param>
    /// <returns><c>true</c> se houver sobreposição.</returns>
    public bool Collides(Piece piece)
    {
        return piece.Blocks.Any(block => Contains(block) && _cells[block.X, block.Y] != PieceColor.None);
    }

    /// <summary>
    /// Assenta os blocos da peça na grade.
    /// </summary>
    /// <param name="piece">A peça que terminou de cair.</param>
    public void Lock(Piece piece)
    {
        foreach (var block in piece.Blocks)
        {
            Lock(block);
        }
    }

    /// <summary>
    /// O bloco na célula informada; uma célula vazia devolve um bloco com <see cref="PieceColor.None"/>.
    /// </summary>
    /// <param name="x">A coluna.</param>
    /// <param name="y">A linha.</param>
    /// <returns>O bloco da célula.</returns>
    public Block BlockAt(int x, int y)
    {
        return new Block(x, y, _cells[x, y]);
    }

    /// <summary>
    /// Remove as linhas completas e faz descer as linhas de cima.
    /// </summary>
    /// <returns>A quantidade de linhas removidas.</returns>
    public int ClearFullLines()
    {
        var cleared = 0;
        var y = Dimension.Height - 1;

        // de baixo para cima; após remover uma linha, a mesma posição recebe a linha de cima e é verificada de novo
        while (y >= 0 && cleared < MAX_LINES_PER_DROP)
        {
            if (IsLineFull(y))
            {
                RemoveLine(y);
                cleared++;
            }
            else
            {
                y--;
            }
        }

        return cleared;
    }

    /// <summary>
    /// Preenche a célula do bloco com a cor dele. Blocos fora da grade são ignorados.
    /// </summary>
    /// <param name="block">O bloco a assentar.</param>
    private void Lock(Block block)
    {
        if (Contains(block))
        {
            _cells[block.X, block.Y] = block.Color;
        }
    }

    /// <summary>
    /// Informa se todas as células da linha estão preenchidas.
    /// </summary>
    /// <param name="y">A linha.</param>
    /// <returns><c>true</c> se a linha está completa.</returns>
    private bool IsLineFull(int y)
    {
        for (var x = 0; x < Dimension.Width; x++)
        {
            if (_cells[x, y] == PieceColor.None)
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Remove a linha, descendo uma posição todas as linhas acima dela.
    /// </summary>
    /// <param name="line">A linha a remover.</param>
    private void RemoveLine(int line)
    {
        for (var y = line; y > 0; y--)
        {
            for (var x = 0; x < Dimension.Width; x++)
            {
                _cells[x, y] = _cells[x, y - 1];
            }
        }

        for (var x = 0; x < Dimension.Width; x++)
        {
            _cells[x, 0] = PieceColor.None;
        }
    }
}
