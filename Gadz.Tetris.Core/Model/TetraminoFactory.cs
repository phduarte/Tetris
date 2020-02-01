using System;

namespace Gadz.Tetris.Model
{
    /// <summary>
    /// Defines the <see cref="TetraminoFactory" />
    /// </summary>
    internal class TetraminoFactory
    {
        /// <summary>
        /// Prevents a default instance of the <see cref="TetraminoFactory"/> class from being created.
        /// </summary>
        private TetraminoFactory()
        {
        }

        /// <summary>
        /// The Draw
        /// </summary>
        /// <param name="type">The type<see cref="PieceType"/></param>
        /// <param name="position">The position<see cref="Point"/></param>
        /// <param name="rotation">The rotation<see cref="int"/></param>
        /// <returns>The <see cref="Tetramino"/></returns>
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
