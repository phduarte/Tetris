using Gadz.Tetris.Domain.Models;
using Gadz.Tetris.Domain.Models.Pieces;
using NUnit.Framework;

namespace Gadz.Tetris.Tests.Model;

/// <summary>
/// Testes da forma de cada tetraminó, criada pelo <see cref="PieceBuilder"/>.
/// </summary>
public class FormasTests
{
    private PieceBuilder _pieceBuilder;

    [SetUp]
    public void Setup()
    {
        _pieceBuilder = new PieceBuilder();
    }

    /// <summary>
    /// A peça I começa na vertical.
    /// </summary>
    [Test]
    public void DeveDesenharFormaI()
    {
        var piece = _pieceBuilder.OfType(PieceType.I).Build();
        Assert.AreEqual(4, piece.Blocks.Count());
        Assert.AreEqual("0x0,0x1,0x2,0x3", piece.ToString());
    }

    /// <summary>
    /// A peça J começa em pé, com o pé para a esquerda.
    /// </summary>
    [Test]
    public void DeveDesenharFormaJ()
    {
        var forma = _pieceBuilder.OfType(PieceType.J).Build();
        Assert.AreEqual(4, forma.Blocks.Count());
        Assert.AreEqual("1x0,1x1,1x2,0x2", forma.ToString());
    }

    /// <summary>
    /// A peça L começa em pé, com o pé para a direita.
    /// </summary>
    [Test]
    public void DeveDesenharFormaL()
    {
        var forma = _pieceBuilder.OfType(PieceType.L).Build();
        Assert.AreEqual("0x0,0x1,0x2,1x2", forma.ToString());
    }

    /// <summary>
    /// A peça O é um quadrado 2×2.
    /// </summary>
    [Test]
    public void DeveDesenharFormaO()
    {
        var piece = _pieceBuilder.OfType(PieceType.O).Build();
        Assert.AreEqual(4, piece.Blocks.Count());
        Assert.AreEqual("0x0,0x1,1x0,1x1", piece.ToString());
    }

    /// <summary>
    /// A peça S começa em pé.
    /// </summary>
    [Test]
    public void DeveDesenharFormaS()
    {
        var piece = _pieceBuilder.OfType(PieceType.S).Build();
        Assert.AreEqual("0x0,0x1,1x1,1x2", piece.ToString());
    }

    /// <summary>
    /// A peça T começa em pé, com a ponta para a direita.
    /// </summary>
    [Test]
    public void DeveDesenharFormaT()
    {
        var piece = _pieceBuilder.OfType(PieceType.T).Build();
        Assert.AreEqual(4, piece.Blocks.Count());
        Assert.AreEqual("0x0,0x1,0x2,1x1", piece.ToString());
    }

    /// <summary>
    /// A peça Z começa em pé.
    /// </summary>
    [Test]
    public void DeveDesenharFormaZ()
    {
        var piece = _pieceBuilder.OfType(PieceType.Z).Build();
        Assert.AreEqual(4, piece.Blocks.Count());
        Assert.AreEqual("1x0,1x1,0x1,0x2", piece.ToString());
    }

    /// <summary>
    /// Cada tipo de peça, em cada rotação, ocupa sempre as mesmas células relativas à sua posição.
    /// </summary>
    [TestCase(PieceType.I, 0, "0x0,0x1,0x2,0x3")]
    [TestCase(PieceType.I, 1, "0x0,1x0,2x0,3x0")]
    [TestCase(PieceType.I, 2, "0x0,0x1,0x2,0x3")]
    [TestCase(PieceType.I, 3, "0x0,1x0,2x0,3x0")]
    [TestCase(PieceType.J, 0, "1x0,1x1,1x2,0x2")]
    [TestCase(PieceType.J, 1, "0x0,1x0,2x0,2x1")]
    [TestCase(PieceType.J, 2, "0x0,0x1,0x2,1x0")]
    [TestCase(PieceType.J, 3, "0x0,0x1,1x1,2x1")]
    [TestCase(PieceType.L, 0, "0x0,0x1,0x2,1x2")]
    [TestCase(PieceType.L, 1, "0x1,1x1,2x1,2x0")]
    [TestCase(PieceType.L, 2, "0x0,1x0,1x1,1x2")]
    [TestCase(PieceType.L, 3, "0x0,1x0,2x0,0x1")]
    [TestCase(PieceType.O, 0, "0x0,0x1,1x0,1x1")]
    [TestCase(PieceType.O, 1, "0x0,0x1,1x0,1x1")]
    [TestCase(PieceType.O, 2, "0x0,0x1,1x0,1x1")]
    [TestCase(PieceType.O, 3, "0x0,0x1,1x0,1x1")]
    [TestCase(PieceType.S, 0, "0x0,0x1,1x1,1x2")]
    [TestCase(PieceType.S, 1, "0x1,1x1,1x0,2x0")]
    [TestCase(PieceType.S, 2, "0x0,0x1,1x1,1x2")]
    [TestCase(PieceType.S, 3, "0x1,1x1,1x0,2x0")]
    [TestCase(PieceType.T, 0, "0x0,0x1,0x2,1x1")]
    [TestCase(PieceType.T, 1, "0x1,1x1,2x1,1x0")]
    [TestCase(PieceType.T, 2, "2x0,2x1,2x2,1x1")]
    [TestCase(PieceType.T, 3, "0x0,1x0,2x0,1x1")]
    [TestCase(PieceType.Z, 0, "1x0,1x1,0x1,0x2")]
    [TestCase(PieceType.Z, 1, "0x0,1x0,1x1,2x1")]
    [TestCase(PieceType.Z, 2, "1x0,1x1,0x1,0x2")]
    [TestCase(PieceType.Z, 3, "0x0,1x0,1x1,2x1")]
    public void DeveDesenharAFormaEmCadaRotacao(PieceType type, int rotation, string expected)
    {
        var piece = _pieceBuilder.OfType(type).WithRotation(rotation).Build();

        Assert.AreEqual(expected, piece.ToString());
    }

    /// <summary>
    /// Os blocos acompanham a posição da peça, mantendo a forma.
    /// </summary>
    [Test]
    public void DeveDesenharAFormaNaPosicaoDaPeca()
    {
        var piece = _pieceBuilder.OfType(PieceType.T).OnPosition(new Point(3, 5)).WithRotation(1).Build();

        Assert.AreEqual("3x6,4x6,5x6,4x5", piece.ToString());
    }

    /// <summary>
    /// Todos os blocos de uma peça têm a cor do seu tipo.
    /// </summary>
    [Test]
    public void DeveColorirTodosOsBlocosComACorDaPeca()
    {
        var piece = _pieceBuilder.OfType(PieceType.Z).Build();

        Assert.IsTrue(piece.Blocks.All(block => block.Color == PieceColor.Red));
    }

    /// <summary>
    /// Um tipo de peça desconhecido não pode ser construído.
    /// </summary>
    [Test]
    public void NaoDeveDesenharNenhumaForma()
    {
        Assert.Throws<NotImplementedException>(() => _pieceBuilder.OfType((PieceType)8).Build());
    }
}
