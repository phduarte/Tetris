using System;

namespace Gadz.Tetris.Model.Pieces
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
        /// <param name="config">The position<see cref="TetraminoConfiguration"/></param>
        /// <returns>The <see cref="Tetramino"/></returns>
        public static Tetramino Draw(TetraminoConfiguration config)
        {
            switch (config.Type)
            {
                case PieceType.T:
                    return new TetraminoT(config);

                case PieceType.O:
                    return new TetraminoO(config);

                case PieceType.I:
                    return new TetraminoI(config);

                case PieceType.L:
                    return new TetraminoL(config);

                case PieceType.J:
                    return new TetraminoJ(config);

                case PieceType.S:
                    return new TetraminoS(config);

                case PieceType.Z:
                    return new TetraminoZ(config);

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
