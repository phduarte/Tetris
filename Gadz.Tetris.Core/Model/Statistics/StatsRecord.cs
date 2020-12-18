namespace Gadz.Tetris.Model.Statistics
{
    /// <summary>
    /// 
    /// </summary>
    public struct StatsRecord
    {
        /// <summary>
        /// 
        /// </summary>
        public Identity Id;
        /// <summary>
        /// 
        /// </summary>
        public int Score;
        /// <summary>
        /// 
        /// </summary>
        public int Lines;
        /// <summary>
        /// 
        /// </summary>
        public int Level;
        /// <summary>
        /// 
        /// </summary>
        public int Speed;
        /// <summary>
        /// 
        /// </summary>
        public int Moves;
        /// <summary>
        /// 
        /// </summary>
        public int Blocks;
        /// <summary>
        /// 
        /// </summary>
        public long Seconds;
        /// <summary>
        /// 
        /// </summary>
        public int Tetris { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public float TetrisRate { get; set; }
    }
}
