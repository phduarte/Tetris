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
        /// <param name="config">The position<see cref="TetraminoConfiguration"/></param>
        public TetraminoS(TetraminoConfiguration config)
        {
            var blocks = new Block[4];
            int x = config.Position.X, y = config.Position.Y;

            switch (config.Rotation)
            {
                case 0:
                case 2:
                    {
                        blocks[0] = new Block(x, y, config.Color);
                        blocks[1] = new Block(x, y + 1, config.Color);
                        blocks[2] = new Block(x + 1, y + 1, config.Color);
                        blocks[3] = new Block(x + 1, y + 2, config.Color);

                        break;
                    }
                case 1:
                case 3:
                    {
                        blocks[0] = new Block(x, y + 1, config.Color);
                        blocks[1] = new Block(x + 1, y + 1, config.Color);
                        blocks[2] = new Block(x + 1, y, config.Color);
                        blocks[3] = new Block(x + 2, y, config.Color);

                        break;
                    }
                default:
                    return;
            }

            Blocks = blocks;
        }
    }
}
