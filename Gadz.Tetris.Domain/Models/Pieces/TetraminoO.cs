using Gadz.Tetris.Domain.Models.Boards;

namespace Gadz.Tetris.Domain.Models.Pieces;

/// <summary>
/// Defines the <see cref="TetraminoO" />
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="TetraminoO"/> class.
/// </remarks>
/// <param name="config">The position<see cref="PieceConfiguration"/></param>
internal class TetraminoO(Board board, PieceConfiguration config) : Piece(board, config)
{
    public override Block[] Blocks
    {
        get
        {
            var blocks = new Block[4];
            int x = Position.X, y = Position.Y;

            blocks[0] = new Block(x, y, Color);
            blocks[1] = new Block(x, y + 1, Color);
            blocks[2] = new Block(x + 1, y, Color);
            blocks[3] = new Block(x + 1, y + 1, Color);

            return blocks;
        }
    }
}
