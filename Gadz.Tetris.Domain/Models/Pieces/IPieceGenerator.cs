namespace Gadz.Tetris.Domain.Models.Pieces;

/// <summary>
/// Escolhe o tipo e a rotação de cada nova peça que entra no tabuleiro.
/// </summary>
public interface IPieceGenerator
{
    /// <summary>
    /// Escolhe a próxima peça.
    /// </summary>
    /// <returns>O tipo e a rotação (0–3) da próxima peça.</returns>
    (PieceType Type, int Rotation) Next();
}
