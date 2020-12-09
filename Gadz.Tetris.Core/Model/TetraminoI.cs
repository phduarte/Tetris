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
        /// <param name="config">The configuration<see cref="TetraminoConfiguration"/></param>
        public TetraminoI(TetraminoConfiguration config)
        {
            var blocks = new Block[4];
            var x = config.Position.X;
            var y = config.Position.Y;

            if (config.Rotation == 0 || config.Rotation == 2)
            {
                blocks[0] = new Block(x, y, config.Color);
                blocks[1] = new Block(x, y + 1, config.Color);
                blocks[2] = new Block(x, y + 2, config.Color);
                blocks[3] = new Block(x, y + 3, config.Color);
            }
            else
            {
                blocks[0] = new Block(x, y, config.Color);
                blocks[1] = new Block(x + 1, y, config.Color);
                blocks[2] = new Block(x + 2, y, config.Color);
                blocks[3] = new Block(x + 3, y, config.Color);
            }

            Blocks = blocks;
        }
    }
}
