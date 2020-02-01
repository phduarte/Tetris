namespace Gadz.Tetris.Model
{
    /// <summary>
    /// Defines the <see cref="TetraminoI" />
    /// </summary>
    internal class TetraminoI : Tetramino
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TetraminoI"/> class.
        /// </summary>
        /// <param name="position">The position<see cref="Point"/></param>
        /// <param name="rotation">The rotation<see cref="int"/></param>
        /// <param name="color">The color<see cref="PieceColor"/></param>
        public TetraminoI(Point position, int rotation, PieceColor color)
        {
            var blocks = new Block[4];
            var x = position.X;
            var y = position.Y;

            if (rotation == 0 || rotation == 2)
            {
                blocks[0] = new Block(x, y, color);
                blocks[1] = new Block(x, y + 1, color);
                blocks[2] = new Block(x, y + 2, color);
                blocks[3] = new Block(x, y + 3, color);
            }
            else
            {
                blocks[0] = new Block(x, y, color);
                blocks[1] = new Block(x + 1, y, color);
                blocks[2] = new Block(x + 2, y, color);
                blocks[3] = new Block(x + 3, y, color);
            }

            Blocks = blocks;
        }
    }
}
