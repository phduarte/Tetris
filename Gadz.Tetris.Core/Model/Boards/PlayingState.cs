namespace Gadz.Tetris.Model.Boards
{
    /// <summary>
    /// Defines the <see cref="PlayingState" />
    /// </summary>
    internal class PlayingState : IBoardState
    {
        /// <summary>
        /// Gets a value indicating whether IsPlaying
        /// </summary>
        public bool IsPlaying => true;

        /// <summary>
        /// Gets a value indicating whether CanChangeStats
        /// </summary>
        public bool CanChangeStats => true;

        /// <summary>
        /// Gets a value indicating whether CanStart
        /// </summary>
        public bool CanStart => false;

        /// <summary>
        /// Gets a value indicating whether CanMove
        /// </summary>
        public bool CanMove => true;

        /// <summary>
        /// Gets a value indicating whether CanPause
        /// </summary>
        public bool CanPause => true;

        /// <summary>
        /// Gets a value indicating whether CanRestart
        /// </summary>
        public bool CanRestart => true;

        /// <summary>
        /// Gets a value indicating whether CanFinish
        /// </summary>
        public bool CanFinish => true;

        /// <summary>
        /// The ToString
        /// </summary>
        /// <returns>The <see cref="string"/></returns>
        public override string ToString() => "Playing";
    }
}
