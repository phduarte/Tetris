using Gadz.Tetris.Domain.Models.Pieces;

namespace Gadz.Tetris.Tests.Model;

/// <summary>
/// Gerador de peças determinístico: devolve as peças informadas em ordem, recomeçando do início ao chegar ao fim.
/// </summary>
/// <param name="pieces">A sequência de peças a devolver.</param>
internal class SequencePieceGenerator(params (PieceType Type, int Rotation)[] pieces) : IPieceGenerator
{
    /// <summary>
    /// Índice da próxima peça da sequência.
    /// </summary>
    private int _index;

    /// <summary>
    /// Devolve a próxima peça da sequência.
    /// </summary>
    /// <returns>O tipo e a rotação da próxima peça.</returns>
    public (PieceType Type, int Rotation) Next()
    {
        var piece = pieces[_index];
        _index = (_index + 1) % pieces.Length;
        return piece;
    }
}
