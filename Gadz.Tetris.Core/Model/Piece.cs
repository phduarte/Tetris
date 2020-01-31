using System.Collections.Generic;

namespace Gadz.Tetris.Model
{
    public class Piece
    {
        private static IDictionary<PieceType, PieceColor> _colors = new Dictionary<PieceType, PieceColor> {
                { PieceType.I, PieceColor.Cyan },
                { PieceType.T, PieceColor.Purple},
                { PieceType.L, PieceColor.Orange},
                { PieceType.J, PieceColor.Blue},
                { PieceType.O, PieceColor.Yellow},
                { PieceType.S, PieceColor.Green},
                { PieceType.Z, PieceColor.Red}
        };

        #region properties

        public int Rotation { get; private set; }
        public PieceColor Color => GetPieceColor(Type);
        public Tetramino Shape => TetraminoFactory.Draw(Type, Position, Rotation);
        public Point Position { get; private set; }
        public PieceType Type { get; private set; }
        public Board Board { get; private set; }
        public Block[] Blocks => Shape.Blocks;

        #endregion properties

        #region constructors

        internal Piece(PieceType type, Point position, int rotation, Board board)
        {
            Type = type;
            Position = position;
            Board = board;
            Rotation = rotation;
        }

        private Piece(Piece clone)
        {
            Rotation = clone.Rotation;
            Position = clone.Position;
            Type = clone.Type;
            Board = clone.Board;
        }

        #endregion constructors

        #region methods

        public void Rotate()
        {
            Rotation = Rotation == 3 ? 0 : ++Rotation;

            int maxX = Position.X;
            int minX = Position.X;

            foreach (var b in Blocks)
            {
                if (b.X > maxX)
                {
                    maxX = b.X;
                }
            }

            foreach (var b in Blocks)
            {
                if (b.X < minX)
                {
                    minX = b.X;
                }
            }

            if (maxX > Board.Width - 1)
            {
                int excesso = maxX - (Board.Width - 1);
                for (int i = 0; i < excesso; i++)
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

        public void MoveDown()
        {
            Position = new Point(Position.X, Position.Y + 1);
        }

        public void MoveRight()
        {
            Position = new Point(Position.X + 1, Position.Y);
        }

        public void MoveLeft()
        {
            Position = new Point(Position.X - 1, Position.Y);
        }

        public Piece Clone()
        {
            return new Piece(this);
        }

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

        #endregion methods

        #region overrided methods

        public override string ToString()
        {
            return $"{Type.ToString()} ({Shape.ToString()})";
        }

        public override bool Equals(object obj)
        {
            if (obj is Piece p)
            {
                return ToString().Equals(p.ToString());
            }
            return false;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        #endregion overrided methods
    }
}