using Gadz.Tetris.Model.Boards;
using System.Collections.Generic;
using System.Linq;

namespace Gadz.Tetris.Model.Pieces
{
    /// <summary>
    /// Defines the <see cref="Piece" />
    /// </summary>
    public class Piece : Entity
    {
        /// <summary>
        /// Defines the _colors
        /// </summary>
        private static IDictionary<PieceType, PieceColor> _colors = new Dictionary<PieceType, PieceColor> {
                { PieceType.I, PieceColor.Cyan },
                { PieceType.T, PieceColor.Purple},
                { PieceType.L, PieceColor.Orange},
                { PieceType.J, PieceColor.Blue},
                { PieceType.O, PieceColor.Yellow},
                { PieceType.S, PieceColor.Green},
                { PieceType.Z, PieceColor.Red}
        };

        /// <summary>
        /// Gets the Rotation
        /// </summary>
        public int Rotation { get; private set; }

        /// <summary>
        /// Gets the Color
        /// </summary>
        public PieceColor Color => GetPieceColor(Type);

        /// <summary>
        /// Gets the Shape
        /// </summary>
        public Tetramino Shape => TetraminoFactory.Draw(new TetraminoConfiguration { Position = Position, Rotation = Rotation, Type = Type });
        /// <summary>
        /// Gets the Position
        /// </summary>
        public Point Position { get; private set; }

        /// <summary>
        /// Gets the Type
        /// </summary>
        public PieceType Type { get; protected set; }

        /// <summary>
        /// Gets the Board
        /// </summary>
        public Board Board { get; private set; }

        /// <summary>
        /// Gets the Blocks
        /// </summary>
        public Block[] Blocks => Shape.Blocks;

        /// <summary>
        /// Initializes a new instance of the <see cref="Piece"/> class.
        /// </summary>
        /// <param name="type">The type<see cref="PieceType"/></param>
        /// <param name="position">The position<see cref="Point"/></param>
        /// <param name="rotation">The rotation<see cref="int"/></param>
        /// <param name="board">The board<see cref="Board"/></param>
        internal Piece(PieceType type, Point position, int rotation, Board board)
        {
            Type = type;
            Position = position;
            Board = board;
            Rotation = rotation;
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="Piece"/> class from being created.
        /// </summary>
        /// <param name="clone">The clone<see cref="Piece"/></param>
        private Piece(Piece clone)
        {
            Rotation = clone.Rotation;
            Position = clone.Position;
            Type = clone.Type;
            Board = clone.Board;
        }

        /// <summary>
        /// The Rotate
        /// </summary>
        public void Rotate()
        {
            Rotation = Rotation == 3 ? 0 : ++Rotation;
            PreventOverlap();
        }

        private void PreventOverlap()
        {
            int maxX = Blocks.Max(x => x.X);
            int minX = Blocks.Min(x => x.X);

            if (maxX > Board.Width - 1)
            {
                int excess = maxX - (Board.Width - 1);
                for (int i = 0; i < excess; i++)
                {
                    MoveLeft();
                }
            }

            if (minX < 0)
            {
                for (int i = minX; i < 0; i++)
                {
                    MoveRight();
                }
            }
        }

        /// <summary>
        /// The MoveDown
        /// </summary>
        public void MoveDown()
        {
            Position = new Point(Position.X, Position.Y + 1);
        }

        /// <summary>
        /// The MoveRight
        /// </summary>
        public void MoveRight()
        {
            Position = new Point(Position.X + 1, Position.Y);
        }

        /// <summary>
        /// The MoveLeft
        /// </summary>
        public void MoveLeft()
        {
            Position = new Point(Position.X - 1, Position.Y);
        }

        /// <summary>
        /// The Clone
        /// </summary>
        /// <returns>The <see cref="Piece"/></returns>
        public Piece Clone()
        {
            return new Piece(this);
        }

        /// <summary>
        /// The GetPieceColor
        /// </summary>
        /// <param name="index">The index<see cref="PieceType"/></param>
        /// <returns>The <see cref="PieceColor"/></returns>
        public static PieceColor GetPieceColor(PieceType index)
        {
            try
            {
                return _colors[index];
            }
            catch (KeyNotFoundException)
            {
                return PieceColor.None;
            }
        }

        /// <summary>
        /// The ToString
        /// </summary>
        /// <returns>The <see cref="string"/></returns>
        public override string ToString()
        {
            return $"{Type} ({Shape})";
        }

        /// <summary>
        /// The Equals
        /// </summary>
        /// <param name="obj">The obj<see cref="object"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public override bool Equals(object obj)
        {
            if (obj is Piece p)
            {
                return ToString().Equals(p.ToString());
            }
            return false;
        }

        /// <summary>
        /// The GetHashCode
        /// </summary>
        /// <returns>The <see cref="int"/></returns>
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }
}
