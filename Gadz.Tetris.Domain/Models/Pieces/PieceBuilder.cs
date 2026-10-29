namespace Gadz.Tetris.Domain.Models.Pieces;

/// <summary>
/// Única forma de criar peças (Builder): escolhe o tetraminó concreto pelo <see cref="PieceType"/>,
/// mantendo as classes <c>Tetramino*</c> internas ao domínio.
/// </summary>
public class PieceBuilder
{
    /// <summary>
    /// O tipo da peça a construir.
    /// </summary>
    private PieceType _pieceType;

    /// <summary>
    /// A posição da peça a construir.
    /// </summary>
    private Point _position;

    /// <summary>
    /// A rotação da peça a construir, de 0 a 3.
    /// </summary>
    private int _rotation;

    /// <summary>
    /// A configuração acumulada até aqui, entregue ao tetraminó em <see cref="Build"/>.
    /// </summary>
    public PieceConfiguration Configuration => new() { Type = _pieceType, Position = _position, Rotation = _rotation };

    /// <summary>
    /// Define o tipo da peça.
    /// </summary>
    /// <param name="type">O tetraminó.</param>
    /// <returns>O próprio builder, para encadear chamadas.</returns>
    public PieceBuilder OfType(PieceType type)
    {
        _pieceType = type;
        return this;
    }

    /// <summary>
    /// Define a posição da peça (o canto superior esquerdo da sua forma).
    /// </summary>
    /// <param name="position">A posição no tabuleiro.</param>
    /// <returns>O próprio builder, para encadear chamadas.</returns>
    public PieceBuilder OnPosition(Point position)
    {
        _position = position;
        return this;
    }

    /// <summary>
    /// Define a rotação da peça.
    /// </summary>
    /// <param name="rotation">A rotação, de 0 a 3.</param>
    /// <returns>O próprio builder, para encadear chamadas.</returns>
    public PieceBuilder WithRotation(int rotation)
    {
        _rotation = rotation;
        return this;
    }

    /// <summary>
    /// Cria a peça com a configuração definida.
    /// </summary>
    /// <returns>Uma nova peça.</returns>
    /// <exception cref="NotImplementedException">Se o tipo não corresponder a nenhum tetraminó.</exception>
    public Piece Build()
    {
        return _pieceType switch
        {
            PieceType.T => new TetraminoT(Configuration),
            PieceType.O => new TetraminoO(Configuration),
            PieceType.I => new TetraminoI(Configuration),
            PieceType.L => new TetraminoL(Configuration),
            PieceType.J => new TetraminoJ(Configuration),
            PieceType.S => new TetraminoS(Configuration),
            PieceType.Z => new TetraminoZ(Configuration),
            _ => throw new NotImplementedException()
        };
    }
}
