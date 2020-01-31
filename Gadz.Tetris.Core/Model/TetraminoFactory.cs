using System;

namespace Gadz.Tetris.Model
{
    internal class TetraminoFactory
    {
        private TetraminoFactory()
        {
        }

        public static Tetramino Draw(PieceType type, Point position, int rotation)
        {
            var cor = Piece.GetPieceColor(type);

            switch (type)
            {
                case PieceType.T:
                    return new TetraminoT(position, rotation, cor);

                case PieceType.O:
                    return new TetraminoO(position, cor);

                case PieceType.I:
                    return new TetraminoI(position, rotation, cor);

                case PieceType.L:
                    return new TetraminoL(position, rotation, cor);

                case PieceType.J:
                    return new TetraminoJ(position, rotation, cor);

                case PieceType.S:
                    return new TetraminoS(position, rotation, cor);

                case PieceType.Z:
                    return new TetraminoZ(position, rotation, cor);

                default:
                    throw new NotImplementedException();
            }
        }
    }
}