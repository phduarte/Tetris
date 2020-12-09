using Gadz.Tetris.Model.Boards;

namespace Gadz.Tetris.Model.Pieces
{
    /// <summary>
    /// Defines the <see cref="TetraminoO" />
    /// </summary>
    internal class TetraminoO : Piece
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TetraminoO"/> class.
        /// </summary>
        /// <param name="config">The position<see cref="PieceConfiguration"/></param>
        public TetraminoO(Board board, PieceConfiguration config)
            : base(board, config)
        {
        }

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
}
