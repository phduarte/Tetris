using Gadz.Tetris.Model.Boards;

namespace Gadz.Tetris.Model.Pieces
{
    /// <summary>
    /// Defines the <see cref="PieceBuilder" />
    /// </summary>
    public class PieceBuilder
    {
        /// <summary>
        /// Defines the _pieceType
        /// </summary>
        private PieceType _pieceType;

        /// <summary>
        /// Defines the _position
        /// </summary>
        private Point _position;

        /// <summary>
        /// Defines the _rotation
        /// </summary>
        private int _rotation;

        /// <summary>
        /// Defines the _board
        /// </summary>
        private Board _board;

        /// <summary>
        /// The OfType
        /// </summary>
        /// <param name="type">The type<see cref="PieceType"/></param>
        /// <returns>The <see cref="PieceBuilder"/></returns>
        public PieceBuilder OfType(PieceType type)
        {
            _pieceType = type;
            return this;
        }

        /// <summary>
        /// The OnPosition
        /// </summary>
        /// <param name="position">The position<see cref="Point"/></param>
        /// <returns>The <see cref="PieceBuilder"/></returns>
        public PieceBuilder OnPosition(Point position)
        {
            _position = position;
            return this;
        }

        /// <summary>
        /// The WithRotation
        /// </summary>
        /// <param name="rotation">The rotation<see cref="int"/></param>
        /// <returns>The <see cref="PieceBuilder"/></returns>
        public PieceBuilder WithRotation(int rotation)
        {
            _rotation = rotation;
            return this;
        }

        /// <summary>
        /// The OnBoard
        /// </summary>
        /// <param name="board">The board<see cref="Board"/></param>
        /// <returns>The <see cref="PieceBuilder"/></returns>
        public PieceBuilder OnBoard(Board board)
        {
            _board = board;
            return this;
        }

        /// <summary>
        /// The Build
        /// </summary>
        /// <returns>The <see cref="Piece"/></returns>
        public Piece Build()
        {
            return new Piece(_pieceType, _position, _rotation, _board);
        }
    }
}
