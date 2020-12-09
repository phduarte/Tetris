namespace Gadz.Tetris
{
    /// <summary>
    /// Defines the <see cref="Size" />
    /// </summary>
    public struct Size
    {
        /// <summary>
        /// Defines the Height, Width
        /// </summary>
        public int Height, Width;

        /// <summary>
        /// Initializes a new instance of the <see cref=""/> class.
        /// </summary>
        /// <param name="largura">The largura<see cref="int"/></param>
        /// <param name="altura">The altura<see cref="int"/></param>
        public Size(int largura, int altura)
        {
            Height = altura;
            Width = largura;
        }

        /// <summary>
        /// The ToString
        /// </summary>
        /// <returns>The <see cref="string"/></returns>
        public override string ToString()
        {
            return $"{Width}:{Height}";
        }
    }
}
