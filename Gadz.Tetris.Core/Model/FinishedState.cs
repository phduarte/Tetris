namespace Gadz.Tetris.Model {
    internal class FinishedState : IBoardState {

        public bool IsPlaying => false;
        public bool CanChangeStats => false;
        public bool CanStart => true;
        public bool CanMove => false;
        public bool CanPause => false;
        public bool CanRestart => true;
        public bool CanFinish => false;
        public override string ToString() => "Finished";
    }
}
