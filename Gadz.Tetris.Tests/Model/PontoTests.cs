using Gadz.Tetris.Domain.Models;
using NUnit.Framework;

namespace Gadz.Tetris.Tests.Model;

/// <summary>
/// Testes de <see cref="Point"/>.
/// </summary>
public class PontoTests
{
    /// <summary>
    /// O ponto é criado com as coordenadas informadas.
    /// </summary>
    [Test]
    public void DeveInstanciar()
    {
        var ponto = new Point(0, 0);
        Assert.AreEqual(0, ponto.X);
        Assert.AreEqual(0, ponto.Y);
    }

    /// <summary>
    /// Pontos com as mesmas coordenadas são iguais.
    /// </summary>
    [Test]
    public void DeveSerIguais()
    {
        var pontoA = new Point(0, 0);
        var pontoB = new Point(0, 0);
        Assert.IsTrue(pontoA.Equals(pontoB));
        //Assert.That(pontoA, Is.Not.SameAs(pontoB));
        Assert.IsTrue(pontoA == pontoB);
    }

    /// <summary>
    /// Pontos com coordenadas diferentes são diferentes.
    /// </summary>
    [Test]
    public void DeveSerDiferentes()
    {
        var pontoA = new Point(0, 0);
        var pontoB = new Point(1, 0);
        Assert.IsFalse(pontoA.Equals(pontoB));
        Assert.IsTrue(pontoA != pontoB);
    }

    /// <summary>
    /// Pontos na mesma célula colidem.
    /// </summary>
    [Test]
    public void DevemColidir()
    {
        var pontoA = new Point(0, 0);
        var pontoB = new Point(0, 0);
        Assert.IsTrue(pontoA.ColideCom(pontoB));
    }

    /// <summary>
    /// Pontos em células diferentes não colidem.
    /// </summary>
    [Test]
    public void NaoDevemColidir()
    {
        var pontoA = new Point(0, 0);
        var pontoB = new Point(1, 0);
        Assert.IsFalse(pontoA.ColideCom(pontoB));
    }
}
