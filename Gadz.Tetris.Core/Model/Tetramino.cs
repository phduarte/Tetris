namespace Gadz.Tetris.Model
{
    /// <summary>
    /// Defines the <see cref="Tetramino" />
    /// </summary>
    public abstract class Tetramino
    {
        /// <summary>
        /// Gets or sets the Blocks
        /// </summary>
        public Block[] Blocks { get; protected set; }

        /// <summary>
        /// The Draw
        /// </summary>
        /// <param name="configuration">The configuration<see cref="TetraminoConfiguration"/></param>
        /// <returns>The <see cref="Tetramino"/></returns>
        public static Tetramino Draw(TetraminoConfiguration configuration)
        {
            return TetraminoFactory.Draw(configuration);
        }

        /// <summary>
        /// The Draw
        /// </summary>
        /// <param name="type">The type<see cref="PieceType"/></param>
        /// <returns>The <see cref="Tetramino"/></returns>
        public static Tetramino Draw(PieceType type)
        {
            return Draw(new TetraminoConfiguration { Type = type});
        }

        /// <summary>
        /// The Draw
        /// </summary>
        /// <param name="type">The type<see cref="PieceType"/></param>
        /// <param name="point">The point<see cref="Point"/></param>
        /// <returns>The <see cref="Tetramino"/></returns>
        public static Tetramino Draw(PieceType type, Point point)
        {
            return Draw(new TetraminoConfiguration { Type = type, Position = point});
        }

        /// <summary>
        /// The Draw
        /// </summary>
        /// <param name="type">The type<see cref="PieceType"/></param>
        /// <param name="rotation">The rotation<see cref="int"/></param>
        /// <returns>The <see cref="Tetramino"/></returns>
        public static Tetramino Draw(PieceType type, int rotation)
        {
            return Draw(new TetraminoConfiguration { Type = type, Rotation = rotation });
        }

        /// <summary>
        /// The ToString
        /// </summary>
        /// <returns>The <see cref="string"/></returns>
        public override string ToString()
        {
            return $"{Blocks[0]},{Blocks[1]},{Blocks[2]},{Blocks[3]}";
        }
    }
}
