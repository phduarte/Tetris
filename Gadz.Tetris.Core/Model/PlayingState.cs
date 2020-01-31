namespace Gadz.Tetris.Model
{
    internal class PlayingState : IBoardState
    {
        public bool IsPlaying => true;
        public bool CanChangeStats => true;
        public bool CanStart => false;
        public bool CanMove => true;
        public bool CanPause => true;
        public bool CanRestart => true;
        public bool CanFinish => true;

        public override string ToString() => "Playing";
    }
}