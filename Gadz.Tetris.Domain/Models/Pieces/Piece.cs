using Gadz.Tetris.Domain.Models.Boards;

namespace Gadz.Tetris.Domain.Models.Pieces;

/// <summary>
/// Defines the <see cref="Piece" />
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="Piece"/> class.
/// </remarks>
/// <param name="configuration">The configuration<see cref="PieceConfiguration"/></param>
public abstract class Piece(PieceConfiguration configuration) : Entity
{
    /// <summary>
    /// Defines the _colors
    /// </summary>
    private static IDictionary<PieceType, PieceColor> _colors = new Dictionary<PieceType, PieceColor> {
            { PieceType.I, PieceColor.Cyan },
            { PieceType.T, PieceColor.Purple},
            { PieceType.L, PieceColor.Orange},
            { PieceType.J, PieceColor.Blue},
            { PieceType.O, PieceColor.Yellow},
            { PieceType.S, PieceColor.Green},
            { PieceType.Z, PieceColor.Red}
    };

    /// <summary>
    /// Gets the Rotation
    /// </summary>
    public int Rotation { get; private set; } = configuration.Rotation;

    /// <summary>
    /// Gets the Color
    /// </summary>
    public PieceColor Color => GetPieceColor(Type);

    /// <summary>
    /// Gets the Position
    /// </summary>
    public Point Position { get; private set; } = configuration.Position;

    /// <summary>
    /// Gets the Type
    /// </summary>
    public PieceType Type { get; protected set; } = configuration.Type;

    /// <summary>
    /// Gets the Blocks
    /// </summary>
    public abstract Block[] Blocks { get; }

    /// <summary>
    /// Gira a peça para a próxima rotação, sem verificar limites nem colisão (isso cabe ao <see cref="Board"/>).
    /// </summary>
    public void Rotate()
    {
        Rotation = Rotation == 3 ? 0 : ++Rotation;
    }

    /// <summary>
    /// The MoveDown
    /// </summary>
    public void MoveDown() => MoveBy(0, 1);

    /// <summary>
    /// The MoveRight
    /// </summary>
    public void MoveRight() => MoveBy(1, 0);

    /// <summary>
    /// The MoveLeft
    /// </summary>
    public void MoveLeft() => MoveBy(-1, 0);

    /// <summary>
    /// Desloca a peça pelo deslocamento informado, sem verificar limites nem colisão (isso cabe ao <see cref="Board"/>).
    /// </summary>
    /// <param name="dx">Deslocamento horizontal (negativo = esquerda).</param>
    /// <param name="dy">Deslocamento vertical (positivo = para baixo).</param>
    internal void MoveBy(int dx, int dy)
    {
        Position = new Point(Position.X + dx, Position.Y + dy);
    }

    /// <summary>
    /// The Clone
    /// </summary>
    /// <returns>The <see cref="Piece"/></returns>
    public virtual Piece Clone()
    {
        var clone = new PieceBuilder().OfType(Type).OnPosition(Position).WithRotation(Rotation).Build();
        clone.Id = Id;
        return clone;
    }

    /// <summary>
    /// The GetPieceColor
    /// </summary>
    /// <param name="index">The index<see cref="PieceType"/></param>
    /// <returns>The <see cref="PieceColor"/></returns>
    public static PieceColor GetPieceColor(PieceType index)
    {
        try
        {
            return _colors[index];
        }
        catch (KeyNotFoundException)
        {
            return PieceColor.None;
        }
    }

    /// <summary>
    /// The Equals
    /// </summary>
    /// <param name="obj">The obj<see cref="object"/></param>
    /// <returns>The <see cref="bool"/></returns>
    public override bool Equals(object? obj)
    {
        if (obj is Piece p)
        {
            return ToString().Equals(p.ToString());
        }
        return false;
    }

    /// <summary>
    /// The GetHashCode
    /// </summary>
    /// <returns>The <see cref="int"/></returns>
    public override int GetHashCode()
    {
        return ToString().GetHashCode();
    }

    public override string ToString()
    {
        return $"{Blocks[0]},{Blocks[1]},{Blocks[2]},{Blocks[3]}";
    }
}
