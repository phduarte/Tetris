namespace Gadz.Tetris.Model
{
    /// <summary>
    /// Defines the <see cref="TetraminoL" />
    /// </summary>
    internal class TetraminoL : Tetramino
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TetraminoL"/> class.
        /// </summary>
        /// <param name="position">The position<see cref="Point"/></param>
        /// <param name="rotation">The rotation<see cref="int"/></param>
        /// <param name="color">The color<see cref="PieceColor"/></param>
        public TetraminoL(Point position, int rotation, PieceColor color)
        {
            var blocks = new Block[4];
            int x = position.X, y = position.Y;

            if (rotation == 0)
            {
                blocks[0] = new Block(x, y, color);
                blocks[1] = new Block(x, y + 1, color);
                blocks[2] = new Block(x, y + 2, color);
                blocks[3] = new Block(x + 1, y + 2, color);
            }
            else if (rotation == 1)
            {
                blocks[0] = new Block(x, y + 1, color);
                blocks[1] = new Block(x + 1, y + 1, color);
                blocks[2] = new Block(x + 2, y + 1, color);
                blocks[3] = new Block(x + 2, y, color);
            }
            else if (rotation == 2)
            {
                blocks[0] = new Block(x, y, color);
                blocks[1] = new Block(x + 1, y, color);
                blocks[2] = new Block(x + 1, y + 1, color);
                blocks[3] = new Block(x + 1, y + 2, color);
            }
            else if (rotation == 3)
            {
                blocks[0] = new Block(x, y, color);
                blocks[1] = new Block(x + 1, y, color);
                blocks[2] = new Block(x + 2, y, color);
                blocks[3] = new Block(x, y + 1, color);
            }

            Blocks = blocks;
        }
    }
}
