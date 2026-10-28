using Gadz.Tetris.Domain.Models.Boards;

namespace Gadz.Tetris.Domain.Models.Pieces;

/// <summary>
/// Defines the <see cref="PieceBuilder" />
/// </summary>
public class PieceBuilder
{
    /// <summary>
    /// Defines the _pieceType
    /// </summary>
    private PieceType _pieceType;

    /// <summary>
    /// Defines the _position
    /// </summary>
    private Point _position;

    /// <summary>
    /// Defines the _rotation
    /// </summary>
    private int _rotation;

    /// <summary>
    /// Defines the _board
    /// </summary>
    private Board? _board;

    /// <summary>
    /// 
    /// </summary>
    public PieceConfiguration Configuration => new() { Type = _pieceType, Position = _position, Rotation = _rotation };

    /// <summary>
    /// The OfType
    /// </summary>
    /// <param name="type">The type<see cref="PieceType"/></param>
    /// <returns>The <see cref="PieceBuilder"/></returns>
    public PieceBuilder OfType(PieceType type)
    {
        _pieceType = type;
        return this;
    }

    /// <summary>
    /// The OnPosition
    /// </summary>
    /// <param name="position">The position<see cref="Point"/></param>
    /// <returns>The <see cref="PieceBuilder"/></returns>
    public PieceBuilder OnPosition(Point position)
    {
        _position = position;
        return this;
    }

    /// <summary>
    /// The WithRotation
    /// </summary>
    /// <param name="rotation">The rotation<see cref="int"/></param>
    /// <returns>The <see cref="PieceBuilder"/></returns>
    public PieceBuilder WithRotation(int rotation)
    {
        _rotation = rotation;
        return this;
    }

    /// <summary>
    /// The OnBoard
    /// </summary>
    /// <param name="board">The board<see cref="Board"/></param>
    /// <returns>The <see cref="PieceBuilder"/></returns>
    public PieceBuilder OnBoard(Board board)
    {
        _board = board;
        return this;
    }

    /// <summary>
    /// The Build
    /// </summary>
    /// <returns>The <see cref="Piece"/></returns>
    public Piece Build()
    {
        var board = _board ?? throw new InvalidOperationException($"Informe o tabuleiro com {nameof(OnBoard)} antes de construir a peça.");

        return _pieceType switch
        {
            PieceType.T => new TetraminoT(board, Configuration),
            PieceType.O => new TetraminoO(board, Configuration),
            PieceType.I => new TetraminoI(board, Configuration),
            PieceType.L => new TetraminoL(board, Configuration),
            PieceType.J => new TetraminoJ(board, Configuration),
            PieceType.S => new TetraminoS(board, Configuration),
            PieceType.Z => new TetraminoZ(board, Configuration),
            _ => throw new NotImplementedException()
        };
    }
}
