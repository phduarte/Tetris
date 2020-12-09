using Gadz.Tetris.Model.Boards;

namespace Gadz.Tetris.Model.Pieces
{
    /// <summary>
    /// Defines the <see cref="TetraminoS" />
    /// </summary>
    internal class TetraminoS : Piece
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TetraminoS"/> class.
        /// </summary>
        /// <param name="config">The position<see cref="PieceConfiguration"/></param>
        public TetraminoS(Board board, PieceConfiguration config)
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
                    case 2:
                        {
                            blocks[0] = new Block(x, y, Color);
                            blocks[1] = new Block(x, y + 1, Color);
                            blocks[2] = new Block(x + 1, y + 1, Color);
                            blocks[3] = new Block(x + 1, y + 2, Color);

                            break;
                        }
                    case 1:
                    case 3:
                        {
                            blocks[0] = new Block(x, y + 1, Color);
                            blocks[1] = new Block(x + 1, y + 1, Color);
                            blocks[2] = new Block(x + 1, y, Color);
                            blocks[3] = new Block(x + 2, y, Color);

                            break;
                        }
                    default:
                        break;
                }

                return blocks;
            }
        }
    }
}
