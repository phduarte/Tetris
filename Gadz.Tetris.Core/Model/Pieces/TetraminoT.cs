using Gadz.Tetris.Model.Boards;

namespace Gadz.Tetris.Model.Pieces
{
    /// <summary>
    /// Defines the <see cref="TetraminoT" />
    /// </summary>
    internal class TetraminoT : Piece
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TetraminoT"/> class.
        /// </summary>
        /// <param name="config">The position<see cref="PieceConfiguration"/></param>
        public TetraminoT(Board board, PieceConfiguration config)
            : base(board, config)
        {

        }

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
}
