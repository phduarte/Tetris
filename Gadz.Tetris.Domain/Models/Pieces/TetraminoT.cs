namespace Gadz.Tetris.Domain.Models.Pieces;

/// <summary>
/// Defines the <see cref="TetraminoT" />
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="TetraminoT"/> class.
/// </remarks>
/// <param name="config">The position<see cref="PieceConfiguration"/></param>
internal class TetraminoT(PieceConfiguration config) : Piece(config)
{
    public override Block[] Blocks
    {
        get
        {
            var blocks = new Block[4];
            int x = Position.X, y = Position.Y;

            switch (Rotation)
            {
                case 0:
                    blocks[0] = new Block(x, y, Color);
                    blocks[1] = new Block(x, y + 1, Color);
                    blocks[2] = new Block(x, y + 2, Color);
                    blocks[3] = new Block(x + 1, y + 1, Color);
                    break;

                case 1:
                    blocks[0] = new Block(x, y + 1, Color);
                    blocks[1] = new Block(x + 1, y + 1, Color);
                    blocks[2] = new Block(x + 2, y + 1, Color);
                    blocks[3] = new Block(x + 1, y, Color);
                    break;

                case 2:
                    blocks[0] = new Block(x + 2, y, Color);
                    blocks[1] = new Block(x + 2, y + 1, Color);
                    blocks[2] = new Block(x + 2, y + 2, Color);
                    blocks[3] = new Block(x + 1, y + 1, Color);
                    break;

                case 3:
                    blocks[0] = new Block(x, y, Color);
                    blocks[1] = new Block(x + 1, y, Color);
                    blocks[2] = new Block(x + 2, y, Color);
                    blocks[3] = new Block(x + 1, y + 1, Color);
                    break;

                default:
                    break;
            }

            return blocks;
        }
    }
}
