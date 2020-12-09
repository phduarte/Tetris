using Gadz.Tetris.Model.Boards;
using System.Collections.Generic;
using System.Linq;

namespace Gadz.Tetris.Model.Pieces
{
    /// <summary>
    /// Defines the <see cref="Piece" />
    /// </summary>
    public abstract class Piece : Entity
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
        public abstract Block[] Blocks { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Piece"/> class.
        /// </summary>
        /// <param name="board">The board<see cref="Board"/></param>
        /// <param name="configuration">The configuration<see cref="PieceConfiguration"/></param>
        protected Piece(Board board, PieceConfiguration configuration)
        {
            Board = board;
            Type = configuration.Type;
            Position = configuration.Position;
            Rotation = configuration.Rotation;
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
        public virtual Piece Clone()
        {
            var clone = new PieceBuilder().OnBoard(Board).OfType(Type).OnPosition(Position).WithRotation(Rotation).Build();
            clone.Id = Id;
            return clone;
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

        public override string ToString()
        {
            return $"{Blocks[0]},{Blocks[1]},{Blocks[2]},{Blocks[3]}";
        }
    }
}
