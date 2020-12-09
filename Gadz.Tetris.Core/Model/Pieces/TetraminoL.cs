namespace Gadz.Tetris.Model.Pieces
{
    /// <summary>
    /// Defines the <see cref="TetraminoL" />
    /// </summary>
    internal class TetraminoL : Tetramino
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TetraminoL"/> class.
        /// </summary>
        /// <param name="config">The configuration<see cref="TetraminoConfiguration"/></param>
        public TetraminoL(TetraminoConfiguration config)
        {
            var blocks = new Block[4];
            int x = config.Position.X, y = config.Position.Y;

            if (config.Rotation == 0)
            {
                blocks[0] = new Block(x, y, config.Color);
                blocks[1] = new Block(x, y + 1, config.Color);
                blocks[2] = new Block(x, y + 2, config.Color);
                blocks[3] = new Block(x + 1, y + 2, config.Color);
            }
            else if (config.Rotation == 1)
            {
                blocks[0] = new Block(x, y + 1, config.Color);
                blocks[1] = new Block(x + 1, y + 1, config.Color);
                blocks[2] = new Block(x + 2, y + 1, config.Color);
                blocks[3] = new Block(x + 2, y, config.Color);
            }
            else if (config.Rotation == 2)
            {
                blocks[0] = new Block(x, y, config.Color);
                blocks[1] = new Block(x + 1, y, config.Color);
                blocks[2] = new Block(x + 1, y + 1, config.Color);
                blocks[3] = new Block(x + 1, y + 2, config.Color);
            }
            else if (config.Rotation == 3)
            {
                blocks[0] = new Block(x, y, config.Color);
                blocks[1] = new Block(x + 1, y, config.Color);
                blocks[2] = new Block(x + 2, y, config.Color);
                blocks[3] = new Block(x, y + 1, config.Color);
            }

            Blocks = blocks;
        }
    }
}
