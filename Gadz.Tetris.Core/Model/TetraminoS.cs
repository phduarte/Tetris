namespace Gadz.Tetris.Model
{
    /// <summary>
    /// Defines the <see cref="TetraminoS" />
    /// </summary>
    internal class TetraminoS : Tetramino
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TetraminoS"/> class.
        /// </summary>
        /// <param name="position">The position<see cref="Point"/></param>
        /// <param name="rotation">The rotation<see cref="int"/></param>
        /// <param name="color">The color<see cref="PieceColor"/></param>
        public TetraminoS(Point position, int rotation, PieceColor color)
        {
            var blocks = new Block[4];
            int x = position.X, y = position.Y;

            switch (rotation)
            {
                case 0:
                case 2:
                    {
                        blocks[0] = new Block(x, y, color);
                        blocks[1] = new Block(x, y + 1, color);
                        blocks[2] = new Block(x + 1, y + 1, color);
                        blocks[3] = new Block(x + 1, y + 2, color);

                        break;
                    }
                case 1:
                case 3:
                    {
                        blocks[0] = new Block(x, y + 1, color);
                        blocks[1] = new Block(x + 1, y + 1, color);
                        blocks[2] = new Block(x + 1, y, color);
                        blocks[3] = new Block(x + 2, y, color);

                        break;
                    }
                default:
                    return;
            }

            Blocks = blocks;
        }
    }
}
