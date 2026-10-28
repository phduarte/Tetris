using Gadz.Tetris.Domain.Models.Boards;

namespace Gadz.Tetris.Domain.Models.Pieces;

/// <summary>
/// Defines the <see cref="TetraminoL" />
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="TetraminoL"/> class.
/// </remarks>
/// <param name="board">The parent board<see cref="Board"/></param>
/// <param name="config">The configuration<see cref="PieceConfiguration"/></param>
internal class TetraminoL(Board board, PieceConfiguration config) : Piece(board, config)
{
    public override Block[] Blocks
    {
        get
        {
            var blocks = new Block[4];
            int x = Position.X, y = Position.Y;

            if (Rotation == 0)
            {
                blocks[0] = new Block(x, y, Color);
                blocks[1] = new Block(x, y + 1, Color);
                blocks[2] = new Block(x, y + 2, Color);
                blocks[3] = new Block(x + 1, y + 2, Color);
            }
            else if (Rotation == 1)
            {
                blocks[0] = new Block(x, y + 1, Color);
                blocks[1] = new Block(x + 1, y + 1, Color);
                blocks[2] = new Block(x + 2, y + 1, Color);
                blocks[3] = new Block(x + 2, y, Color);
            }
            else if (Rotation == 2)
            {
                blocks[0] = new Block(x, y, Color);
                blocks[1] = new Block(x + 1, y, Color);
                blocks[2] = new Block(x + 1, y + 1, Color);
                blocks[3] = new Block(x + 1, y + 2, Color);
            }
            else if (Rotation == 3)
            {
                blocks[0] = new Block(x, y, Color);
                blocks[1] = new Block(x + 1, y, Color);
                blocks[2] = new Block(x + 2, y, Color);
                blocks[3] = new Block(x, y + 1, Color);
            }

            return blocks;
        }
    }
}
