using Gadz.Tetris.Domain.Models.Boards;

namespace Gadz.Tetris.Domain.Models.Pieces;

/// <summary>
/// Um tetraminó: tipo, posição e rotação. A peça só sabe se mover e girar; quem decide se o movimento
/// é válido (limites e colisão) é o <see cref="Board"/>. Crie peças com o <see cref="PieceBuilder"/>.
/// </summary>
/// <param name="configuration">Tipo, posição e rotação iniciais.</param>
public abstract class Piece(PieceConfiguration configuration) : Entity
{
    /// <summary>
    /// Quantidade de rotações de uma peça; depois da última, volta à rotação 0.
    /// </summary>
    private const int ROTATION_COUNT = 4;

    /// <summary>
    /// A cor fixa de cada tipo de peça.
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
    /// A rotação atual, de 0 a 3.
    /// </summary>
    public int Rotation { get; private set; } = configuration.Rotation;

    /// <summary>
    /// A cor da peça, definida pelo seu tipo.
    /// </summary>
    public PieceColor Color => GetPieceColor(Type);

    /// <summary>
    /// O canto superior esquerdo da forma da peça no tabuleiro.
    /// </summary>
    public Point Position { get; private set; } = configuration.Position;

    /// <summary>
    /// O tetraminó que a peça representa.
    /// </summary>
    public PieceType Type { get; protected set; } = configuration.Type;

    /// <summary>
    /// A forma da peça: para cada rotação (0 a 3), o deslocamento de cada um dos 4 blocos em relação a <see cref="Position"/>.
    /// Cada tetraminó define a sua (Strategy); a montagem dos blocos é comum a todos.
    /// </summary>
    protected abstract Point[][] Shapes { get; }

    /// <summary>
    /// Os 4 blocos que a peça ocupa no tabuleiro, na posição e rotação atuais.
    /// </summary>
    public Block[] Blocks =>
        [.. Shapes[Rotation].Select(offset => new Block(Position.X + offset.X, Position.Y + offset.Y, Color))];

    /// <summary>
    /// Gira a peça para a próxima rotação, sem verificar limites nem colisão (isso cabe ao <see cref="Board"/>).
    /// </summary>
    public void Rotate()
    {
        Rotation = (Rotation + 1) % ROTATION_COUNT;
    }

    /// <summary>
    /// Desce a peça uma linha, sem verificar limites nem colisão.
    /// </summary>
    public void MoveDown() => MoveBy(0, 1);

    /// <summary>
    /// Move a peça uma coluna para a direita, sem verificar limites nem colisão.
    /// </summary>
    public void MoveRight() => MoveBy(1, 0);

    /// <summary>
    /// Move a peça uma coluna para a esquerda, sem verificar limites nem colisão.
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
    /// Cria uma cópia independente da peça, com a mesma identidade. Usada para testar um movimento antes de aplicá-lo.
    /// </summary>
    /// <returns>A cópia.</returns>
    public virtual Piece Clone()
    {
        var clone = new PieceBuilder().OfType(Type).OnPosition(Position).WithRotation(Rotation).Build();
        clone.Id = Id;
        return clone;
    }

    /// <summary>
    /// A cor fixa de um tipo de peça.
    /// </summary>
    /// <param name="index">O tipo da peça.</param>
    /// <returns>A cor do tipo, ou <see cref="PieceColor.None"/> se o tipo for desconhecido.</returns>
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
    /// Duas peças são iguais quando ocupam as mesmas células, na mesma ordem de blocos.
    /// </summary>
    /// <param name="obj">O objeto a comparar.</param>
    /// <returns><c>true</c> se <paramref name="obj"/> for uma peça com os mesmos blocos.</returns>
    public override bool Equals(object? obj)
    {
        if (obj is Piece p)
        {
            return ToString().Equals(p.ToString());
        }
        return false;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return ToString().GetHashCode();
    }

    /// <summary>
    /// As células ocupadas pela peça, separadas por vírgula (ex.: <c>0x0,0x1,0x2,0x3</c>).
    /// </summary>
    /// <returns>Os blocos em texto.</returns>
    public override string ToString()
    {
        return string.Join(",", Blocks);
    }
}
