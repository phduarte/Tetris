using Gadz.Tetris.Model.Boards;

namespace Gadz.Tetris.Model.Pieces
{
    /// <summary>
    /// Defines the <see cref="Tetramino" />
    /// </summary>
    public abstract class Tetramino : Piece
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="board"></param>
        /// <param name="configuration"></param>
        protected Tetramino(Board board, PieceConfiguration configuration) 
            : base(board, configuration)
        {
        }

        /// <summary>
        /// The ToString
        /// </summary>
        /// <returns>The <see cref="string"/></returns>
        public override string ToString()
        {
            return $"{Type} {Blocks[0]},{Blocks[1]},{Blocks[2]},{Blocks[3]}";
        }
    }
}
