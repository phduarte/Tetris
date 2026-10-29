using Gadz.Tetris.Domain.Models.Pieces;
using NUnit.Framework;

namespace Gadz.Tetris.Tests.Model;

/// <summary>
/// Testes dos movimentos de uma <see cref="Piece"/> isolada, sem tabuleiro.
/// </summary>
public class PecaTests
{
    /// <summary>
    /// Peça I na origem, recriada a cada teste.
    /// </summary>
    private Piece peca;

    /// <summary>
    /// Cria uma peça I na rotação inicial.
    /// </summary>
    [SetUp]
    public void Setup()
    {
        peca = new PieceBuilder()
            .OfType(PieceType.I)
            .WithRotation(0)
            .Build();
    }

    /// <summary>
    /// A peça criada tem o tipo, a cor, os 4 blocos, a posição e a rotação pedidos.
    /// </summary>
    [Test]
    public void DeveInstanciar()
    {
        Assert.IsNotNull(peca, "A peça não foi instanciada.");
        Assert.AreEqual(PieceType.I, peca.Type, "O tipo da peça não é I");
        Assert.AreEqual(4, peca.Blocks.Count(), "O tetraminó não possui 4 blocos");
        Assert.AreEqual(Piece.GetPieceColor(PieceType.I), peca.Color, "A cor da peça não é amarela");
        Assert.AreEqual(0, peca.Position.X, "A posição da peça no eixo X não é 0.");
        Assert.AreEqual(0, peca.Position.Y, "A posição da peça no eixo Y não é 0.");
        Assert.AreEqual(0, peca.Rotation, "A rotação da peça não é 0.");
    }

    /// <summary>
    /// Girar avança para a próxima rotação.
    /// </summary>
    [Test]
    public void DeveGirar()
    {
        peca.Rotate();
        Assert.AreEqual(1, peca.Rotation);
    }

    /// <summary>
    /// Mover para a direita avança uma coluna.
    /// </summary>
    [Test]
    public void DeveMoverParaDireita()
    {
        peca.MoveRight();
        Assert.AreEqual(1, peca.Position.X);
    }

    /// <summary>
    /// Mover para a esquerda volta uma coluna.
    /// </summary>
    [Test]
    public void DeveMoverParaEsquerda()
    {
        DeveMoverParaDireita();

        peca.MoveLeft();
        Assert.AreEqual(0, peca.Position.X);
    }

    /// <summary>
    /// Mover para baixo avança uma linha.
    /// </summary>
    [Test]
    public void DeveMoverParaBaixo()
    {
        peca.MoveDown();
        Assert.AreEqual(1, peca.Position.Y);
    }

    /// <summary>
    /// O clone é igual à peça original, mas é outro objeto.
    /// </summary>
    [Test]
    public void DeveSerClonada()
    {
        var pecaB = peca.Clone();
        Assert.AreEqual(peca, pecaB, "Não são iguais");
        Assert.AreNotSame(peca, pecaB, "São as mesmas");
    }

    /// <summary>
    /// Atribuir a peça a outra variável não cria uma cópia.
    /// </summary>
    [Test]
    public void NaoDeveSerClonada()
    {
        var pecaB = peca;
        Assert.AreEqual(peca, pecaB, "Não são iguais");
        Assert.AreSame(peca, pecaB, "Não são as mesmas");
    }
}
