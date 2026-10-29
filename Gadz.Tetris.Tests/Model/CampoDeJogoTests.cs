using Gadz.Tetris.Domain.Models;
using Gadz.Tetris.Domain.Models.Boards;
using Gadz.Tetris.Domain.Models.Pieces;
using NUnit.Framework;

namespace Gadz.Tetris.Tests.Model;

/// <summary>
/// Testes do <see cref="Playfield"/>: colisão e limpeza de linhas dos blocos assentados.
/// </summary>
public class CampoDeJogoTests
{
    /// <summary>
    /// Largura da grade usada nos testes.
    /// </summary>
    private const int WIDTH = 10;

    /// <summary>
    /// Altura da grade usada nos testes.
    /// </summary>
    private const int HEIGHT = 20;

    /// <summary>
    /// Dimensão da grade usada nos testes.
    /// </summary>
    private static readonly Size _dimension = new(WIDTH, HEIGHT);

    /// <summary>
    /// Remover de 1 a 4 linhas completas devolve a quantidade removida e desce os blocos de cima.
    /// </summary>
    /// <param name="lines">Quantidade de linhas completas no fundo da grade.</param>
    [TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]
    [TestCase(4)]
    public void DeveLimparLinhasCompletas(int lines)
    {
        var above = new Block(2, HEIGHT - lines - 1, PieceColor.Red);
        var playfield = new Playfield(_dimension, [.. FullLines(HEIGHT - lines, lines), above]);

        var cleared = playfield.ClearFullLines();

        Assert.AreEqual(lines, cleared);
        Assert.AreEqual(1, playfield.Blocks.Count);
        Assert.AreEqual(new Point(2, HEIGHT - 1), playfield.Blocks[0].Point, "O bloco de cima não desceu.");
        Assert.AreEqual(PieceColor.Red, playfield.Blocks[0].Color);
    }

    /// <summary>
    /// Linhas completas separadas por uma incompleta são removidas e a incompleta desce até o fundo.
    /// </summary>
    [Test]
    public void DeveLimparLinhasCompletasNaoAdjacentes()
    {
        var partial = new Block(5, HEIGHT - 2, PieceColor.Green);
        var playfield = new Playfield(_dimension, [.. FullLines(HEIGHT - 1, 1), partial, .. FullLines(HEIGHT - 3, 1)]);

        var cleared = playfield.ClearFullLines();

        Assert.AreEqual(2, cleared);
        Assert.AreEqual(1, playfield.Blocks.Count);
        Assert.AreEqual(new Point(5, HEIGHT - 1), playfield.Blocks[0].Point);
    }

    /// <summary>
    /// Uma linha com uma célula vazia não é removida.
    /// </summary>
    [Test]
    public void NaoDeveLimparLinhaIncompleta()
    {
        var almostFull = Enumerable.Range(1, WIDTH - 1).Select(x => new Block(x, HEIGHT - 1, PieceColor.Blue));
        var playfield = new Playfield(_dimension, almostFull);

        var cleared = playfield.ClearFullLines();

        Assert.AreEqual(0, cleared);
        Assert.AreEqual(WIDTH - 1, playfield.Blocks.Count);
    }

    /// <summary>
    /// Assentar uma peça preenche as células dos seus blocos, e outra peça na mesma posição colide com eles.
    /// </summary>
    [Test]
    public void DeveColidirComPecaAssentada()
    {
        var playfield = new Playfield(_dimension);
        var piece = CreatePiece(PieceType.O, new Point(4, 10));

        Assert.IsFalse(playfield.Collides(piece));

        playfield.Lock(piece);

        Assert.IsTrue(playfield.Collides(piece));
        Assert.AreEqual(PieceColor.Yellow, playfield.BlockAt(4, 10).Color);
        Assert.AreEqual(4, playfield.Blocks.Count);
    }

    /// <summary>
    /// Uma célula vazia devolve um bloco sem cor.
    /// </summary>
    [Test]
    public void DeveDevolverBlocoSemCorEmCelulaVazia()
    {
        var playfield = new Playfield(_dimension);

        Assert.AreEqual(PieceColor.None, playfield.BlockAt(0, 0).Color);
    }

    /// <summary>
    /// Limpar a grade remove todos os blocos assentados.
    /// </summary>
    [Test]
    public void DeveEsvaziarAGrade()
    {
        var playfield = new Playfield(_dimension, FullLines(HEIGHT - 2, 2));

        playfield.Clear();

        Assert.AreEqual(0, playfield.Blocks.Count);
    }

    /// <summary>
    /// Blocos que preenchem linhas inteiras.
    /// </summary>
    /// <param name="firstLine">A primeira linha preenchida.</param>
    /// <param name="count">Quantas linhas, a partir de <paramref name="firstLine"/>, preencher.</param>
    /// <returns>Os blocos das linhas.</returns>
    private static IEnumerable<Block> FullLines(int firstLine, int count)
    {
        return from y in Enumerable.Range(firstLine, count)
               from x in Enumerable.Range(0, WIDTH)
               select new Block(x, y, PieceColor.Cyan);
    }

    /// <summary>
    /// Cria uma peça na rotação inicial, só para obter os blocos dela.
    /// </summary>
    /// <param name="type">O tipo da peça.</param>
    /// <param name="position">A posição da peça.</param>
    /// <returns>A peça criada.</returns>
    private static Piece CreatePiece(PieceType type, Point position)
    {
        return new PieceBuilder().OfType(type).OnPosition(position).WithRotation(0).Build();
    }
}
