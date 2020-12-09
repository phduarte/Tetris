using Gadz.Tetris.Model.Boards;

namespace Gadz.Tetris.Model.Pieces
{
    /// <summary>
    /// Defines the <see cref="TetraminoI" />
    /// </summary>
    internal class TetraminoI : Piece
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TetraminoI"/> class.
        /// </summary>
        /// <param name="config">The configuration<see cref="PieceConfiguration"/></param>
        internal TetraminoI(Board board, PieceConfiguration config)
            : base(board, config)
        {
        }

        public override Block[] Blocks
        {
            get
            {
                var blocks = new Block[4];
                var x = Position.X;
                var y = Position.Y;

                if (Rotation == 0 || Rotation == 2)
                {
                    blocks[0] = new Block(x, y, Color);
                    blocks[1] = new Block(x, y + 1, Color);
                    blocks[2] = new Block(x, y + 2, Color);
                    blocks[3] = new Block(x, y + 3, Color);
                }
                else
                {
                    blocks[0] = new Block(x, y, Color);
                    blocks[1] = new Block(x + 1, y, Color);
                    blocks[2] = new Block(x + 2, y, Color);
                    blocks[3] = new Block(x + 3, y, Color);
                }

                return blocks;
            }
        }
    }
}
