namespace Gadz.Tetris.Domain.Models.Pieces;

/// <summary>
/// Gerador padrão: sorteia tipo e rotação com a mesma probabilidade.
/// </summary>
internal class RandomPieceGenerator : IPieceGenerator
{
    /// <summary>
    /// Quantidade de rotações possíveis de uma peça.
    /// </summary>
    private const int ROTATIONS = 4;

    /// <summary>
    /// Os tipos de peça que podem ser sorteados.
    /// </summary>
    private static readonly PieceType[] _types = Enum.GetValues<PieceType>();

    /// <summary>
    /// Sorteia a próxima peça.
    /// </summary>
    /// <returns>O tipo e a rotação sorteados.</returns>
    public (PieceType Type, int Rotation) Next()
    {
        return (_types[Random.Shared.Next(_types.Length)], Random.Shared.Next(ROTATIONS));
    }
}
