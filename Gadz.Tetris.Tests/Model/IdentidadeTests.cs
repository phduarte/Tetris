using Gadz.Tetris.Domain.Models;
using NUnit.Framework;

namespace Gadz.Tetris.Tests.Model;

/// <summary>
/// Testes de <see cref="Identity"/> e das suas conversões.
/// </summary>
public class IdentidadeTests
{
    /// <summary>
    /// Uma identidade pode ser criada vazia.
    /// </summary>
    [Test]
    public void DeveInstanciar()
    {
        var id = new Identity();
        Assert.IsNotNull(id);
    }

    /// <summary>
    /// Uma identidade nova pode ser gerada.
    /// </summary>
    [Test]
    public void DeveGerarNova()
    {
        var id = Identity.New();
        Assert.IsNotNull(id);
    }

    /// <summary>
    /// Uma identidade numérica converte para <c>int</c>.
    /// </summary>
    [Test]
    public void DeveConverterIdentidadeParaInteiro()
    {
        var id = new Identity(0);
        var inteiro = 1;
        inteiro = id;
        Assert.AreEqual(0, inteiro);
    }

    /// <summary>
    /// Uma identidade converte para <c>string</c>.
    /// </summary>
    [Test]
    public void DeveConverterIdentidadeParaTexto()
    {
        var id = new Identity("A");
        var texto = "B";
        texto = id;
        Assert.AreEqual("A", texto);
    }

    /// <summary>
    /// Um texto converte para identidade.
    /// </summary>
    [Test]
    public void DeveConverterTextoParIdentidade()
    {
        var id = new Identity(0);
        var texto = "B";
        id = texto;
        Assert.IsTrue("B" == id);
    }

    /// <summary>
    /// Um número converte para identidade.
    /// </summary>
    [Test]
    public void DeveConverterInteiroParaIdentidade()
    {
        var id = new Identity("A");
        var inteiro = 1;
        id = inteiro;
        Assert.IsTrue(1 == id);
    }
}
