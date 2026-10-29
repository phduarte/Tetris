using Gadz.Tetris.Domain.Models.Pieces;
using NUnit.Framework;

namespace Gadz.Tetris.Tests.Model;

/// <summary>
/// Testes de <see cref="Block"/>.
/// </summary>
public class BlocoTests
{
    /// <summary>
    /// Blocos na mesma célula colidem, mesmo com cores diferentes.
    /// </summary>
    [Test]
    public void DeveColidir()
    {
        var blocoA = new Block(0, 0, PieceColor.Yellow);

        var blocoB = new Block(0, 0, PieceColor.Blue);

        Assert.IsTrue(blocoA.CollideWith(blocoB));
    }

    /// <summary>
    /// Blocos em células diferentes não colidem.
    /// </summary>
    [Test]
    public void NaoDeveColidir()
    {
        var blocoA = new Block(0, 0, PieceColor.Yellow);

        var blocoB = new Block(1, 0, PieceColor.Blue);

        Assert.IsFalse(blocoA.CollideWith(blocoB));
    }

    /// <summary>
    /// O bloco é criado na célula informada.
    /// </summary>
    [Test]
    public void DeveInstanciar()
    {
        var bloco = new Block(1, 2, PieceColor.None);
        Assert.AreEqual("1x2", bloco.ToString());
    }
}
