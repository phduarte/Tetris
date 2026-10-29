using Gadz.Tetris.Domain.Models.Pieces;
using NUnit.Framework;

namespace Gadz.Tetris.Tests.Model;

/// <summary>
/// Testes do <see cref="RandomPieceGenerator"/>.
/// </summary>
public class GeradorDePecasTests
{
    /// <summary>
    /// Quantas peças sortear: o bastante para que todos os tipos e rotações apareçam.
    /// </summary>
    private const int DRAWS = 1000;

    /// <summary>
    /// O gerador padrão sorteia todos os tipos de peça e todas as rotações, sem valores fora da faixa.
    /// </summary>
    [Test]
    public void DeveSortearTodosOsTiposERotacoes()
    {
        var generator = new RandomPieceGenerator();

        var pieces = Enumerable.Range(0, DRAWS).Select(_ => generator.Next()).ToList();

        CollectionAssert.AreEquivalent(Enum.GetValues<PieceType>(), pieces.Select(p => p.Type).Distinct());
        CollectionAssert.AreEquivalent(new[] { 0, 1, 2, 3 }, pieces.Select(p => p.Rotation).Distinct());
    }
}
