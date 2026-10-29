using Gadz.Tetris.Domain.Models;
using NUnit.Framework;

namespace Gadz.Tetris.Tests.Model;

/// <summary>
/// Testes de <see cref="Size"/>.
/// </summary>
public class DimensaoTests
{
    /// <summary>
    /// A dimensão é criada com a largura e a altura informadas.
    /// </summary>
    [Test]
    public void DeveInstanciar()
    {
        var dimensao = new Size(10, 20);
        Assert.AreEqual("10:20", dimensao.ToString());
    }
}
