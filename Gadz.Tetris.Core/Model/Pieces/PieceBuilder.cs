using Gadz.Tetris.Model.Boards;
using System;

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
        /// 
        /// </summary>
        public PieceConfiguration Configuration => new PieceConfiguration { Type = _pieceType, Position = _position, Rotation = _rotation };

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
            switch (_pieceType)
            {
                case PieceType.T:
                    return new TetraminoT(_board, Configuration);

                case PieceType.O:
                    return new TetraminoO(_board, Configuration);

                case PieceType.I:
                    return new TetraminoI(_board, Configuration);

                case PieceType.L:
                    return new TetraminoL(_board, Configuration);

                case PieceType.J:
                    return new TetraminoJ(_board, Configuration);

                case PieceType.S:
                    return new TetraminoS(_board, Configuration);

                case PieceType.Z:
                    return new TetraminoZ(_board, Configuration);

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
