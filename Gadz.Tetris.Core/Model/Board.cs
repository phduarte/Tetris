using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Gadz.Tetris.Model
{
    public class Board
    {
        #region fields

        private IStatsRepository _repository;
        bool _atingiuTopo => ThereIsCollision(NextPiece);
        private const int MAXIMO_LINHAS_POSSIVEIS = 4;
        private IList<Block> _playedBlocks = new List<Block>();

        //o propósito dessa variável é evitar que ocorra 2 avaliações ao mesmo tempo sem precisar de nenhum lock.
        private bool _refreshing;

        #endregion

        #region properties

        public Size Dimension { get; private set; }
        public IBoardState State { get; private set; }
        public Stats Stats { get; private set; }
        public bool IsPlaying { get { return State.IsPlaying; } }
        public Block[,] Matrix { get; }
        public Piece CurrentPiece { get; private set; }
        public Piece NextPiece { get; private set; }
        public int Level => Stats.Level;
        public int Lines => Stats.Lines;
        public IList<Block> Blocks => _playedBlocks;
        public int Record { get; private set; }
        public TimeSpan Duration => Stats.Duration;
        public int Height => Dimension.Height;
        public int Width => Dimension.Width;
        public int Speed => Stats.Speed;
        public int Score => Stats.Score;
        public int Frequency => Stats.Speed <= 1000 ? 1000 - Stats.Speed : 0;

        #endregion

        #region events

        public event GameActionEventHandler OnRefresh;

        public event GameActionEventHandler OnFinish;

        public event GameActionEventHandler OnClear;

        public event GameActionEventHandler OnMove;

        public event GameActionEventHandler OnSlide;

        #endregion

        #region constructors

        public Board(IStatsRepository repository, int width, int height)
        {
            _repository = repository;
            Dimension = new Size(width, height);
            Stats = new Stats();
            State = new PausedState();
            Matrix = new Block[width, height];
            Record = _repository.MaxScore();
        }

        #endregion

        #region public methods

        public void SmashDown()
        {
            while (CanMoveDown(CurrentPiece))
            {
                CurrentPiece.MoveDown();
            }

            Checkout();
            OnSlide?.Invoke();
        }

        public void SlideRight()
        {
            if (!CanMoveRight(CurrentPiece))
                return;

            while (CanMoveRight(CurrentPiece))
            {
                CurrentPiece.MoveRight();
            }
            Checkout();
            OnSlide?.Invoke();
        }

        public void SlideLeft()
        {
            if (!CanMoveLeft(CurrentPiece))
                return;

            while (CanMoveLeft(CurrentPiece))
            {
                CurrentPiece.MoveLeft();
            }
            Checkout();
            OnSlide?.Invoke();
        }

        public void Pause()
        {
            State = new PausedState();
            Stats.Pause();
        }

        public void Continue()
        {
            State = new PlayingState();
            Stats.Continue();
            RefreshAsync();
        }

        public void Finish()
        {
            OnRefresh?.Invoke();
            Stats.Finish();
            State = new FinishedState();
            OnFinish?.Invoke();
        }

        public void Start()
        {
            CurrentPiece = CreateRandomBlock();
            NextPiece = CreateRandomBlock();
            Stats.Start();
            ResetMatrix();
            State = new PlayingState();
            RefreshAsync();
        }

        public void Restart()
        {
            Finish();
            Start();
        }

        public void Save()
        {
            _repository.Save(Stats);
        }

        public void Load(Identity id)
        {
            Stats = _repository.Load(id);
        }

        public void MoveDown()
        {
            if (CanMoveDown(CurrentPiece))
            {
                CurrentPiece.MoveDown();
                OnMove?.Invoke();
            }

            Checkout();
        }

        public void MoveRight()
        {
            if (CanMoveRight(CurrentPiece))
            {
                CurrentPiece.MoveRight();
                OnMove?.Invoke();
                ResetMatrix();
            }
        }

        public void MoveLeft()
        {
            if (CanMoveLeft(CurrentPiece))
            {
                CurrentPiece.MoveLeft();
                OnMove?.Invoke();
                ResetMatrix();
            }
        }

        public void Rotate()
        {
            if (!State.CanMove || ThereIsCollision(CurrentPiece))
            {
                return;
            }

            CurrentPiece.Rotate();
            OnMove?.Invoke();
            ResetMatrix();
        }

        #endregion

        #region private methods

        private bool CanMoveLeft(Piece piece)
        {
            if (!State.CanMove)
                return false;

            var newPosition = new Point(piece.Position.X - 1, piece.Position.Y);
            var testPiece = new PieceBuilder()
                                    .OfType(piece.Type)
                                    .OnPosition(newPosition)
                                    .WithRotation(piece.Rotation)
                                    .OnBoard(this)
                                    .Build();

            //check if current piece's blocks don't go out the board width edge
            foreach (var i in testPiece.Blocks)
            {
                if (i.X < 0)
                {
                    return false;
                }
            }

            return !ThereIsCollision(testPiece);
        }

        private bool CanMoveRight(Piece piece)
        {
            if (!State.CanMove)
                return false;

            var newPosition = new Point(piece.Position.X + 1, piece.Position.Y);
            var testPiece = new PieceBuilder()
                .OfType(piece.Type)
                .OnPosition(newPosition)
                .WithRotation(piece.Rotation)
                .OnBoard(this)
                .Build();

            //check if current piece's blocks don't go out the board width edge
            foreach (var i in testPiece.Blocks)
            {
                if (i.X > Dimension.Width - 1)
                {
                    return false;
                }
            }

            return !ThereIsCollision(testPiece);
        }

        private bool CanMoveDown(Piece piece)
        {
            if (!State.CanMove)
                return false;

            var newPosition = new Point(piece.Position.X, piece.Position.Y + 1);
            var testPiece = new PieceBuilder()
                .OfType(piece.Type)
                .OnPosition(newPosition)
                .WithRotation(piece.Rotation)
                .OnBoard(this)
                .Build();

            //check if current piece's blocks don't go out the board height edge
            foreach (var i in testPiece.Blocks)
            {
                if (i.Y > Dimension.Height - 1)
                {
                    return false;
                }
            }

            return !ThereIsCollision(testPiece);
        }

        private void Checkout()
        {
            if (!IsPlaying)
            {
                return;
            }

            if (_refreshing)
            {
                return;
            }

            _refreshing = true;

            //o movimento não será válido se as casas horizontais do tabuleiro já estiverem sido percorridas
            //neste caso, o hogo é finalizado
            if (!CanMoveDown(CurrentPiece))
            {
                if (_atingiuTopo)
                {
                    Finish();
                    return;
                }
                else
                {
                    TrocarPecaAtual();

                    if (_atingiuTopo)
                    {
                        Finish();
                        return;
                    }
                }
            }
            else
            {
                CurrentPiece.MoveDown();
            }

            ResetMatrix();
            VerificarSePreencheuLinha();

            _refreshing = false;
        }

        private void VerificarSePreencheuLinha()
        {
            var linhas = LimparLinhas();

            if (linhas > 0)
            {
                Pontuar(linhas);
            }
        }

        private void ResetMatrix()
        {
            //limpar a matriz
            for (int y = 0; y < Dimension.Height; y++)
            {
                for (int x = 0; x < Dimension.Width; x++)
                {
                    Matrix[x, y] = new Block(x, y, PieceColor.None);
                }
            }

            //pintar os blocos que já saíram
            foreach (var i in _playedBlocks)
            {
                Matrix[i.X, i.Y] = i;
            }

            //pinta o bloco atual
            foreach (var i in CurrentPiece.Blocks)
            {
                Matrix[i.X, i.Y] = i;
            }

            OnRefresh?.Invoke();
        }

        private void RefreshAsync()
        {
            new Thread(() =>
            {
                while (IsPlaying)
                {
                    Thread.Sleep(Frequency);
                    Checkout();
                }
            })
            { IsBackground = true }.Start();
        }

        private Piece CreateRandomBlock()
        {
            var x = new Random().Next(0, 7);
            var tipo = (PieceType)x;
            var rotacao = new Random().Next(0, 4);
            var posicao = new Point(0, 0);
            var peca = new PieceBuilder().OfType(tipo)
                .OnPosition(posicao)
                .WithRotation(rotacao)
                .OnBoard(this)
                .Build();

            return peca.Clone();
        }

        private bool ThereIsCollision(Piece peca)
        {
            if (_playedBlocks.Count <= 1)
                return false;

            foreach (var a in _playedBlocks)
            {
                foreach (var b in peca.Blocks)
                {
                    if (a.CollideWith(b))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private void TrocarPecaAtual()
        {
            GuardarBlocos();
            CurrentPiece = NextPiece.Clone();
            NextPiece = CreateRandomBlock();
            Stats.IncludeBlock();
            OnRefresh?.Invoke();
        }

        private void GuardarBlocos()
        {
            foreach (var i in CurrentPiece.Clone().Shape.Blocks)
            {
                _playedBlocks.Add(i);
            }
        }

        private int LimparLinhas()
        {
            var linhas = 0;

            //note que se no tabuleiro houver menos de 10 blocos, logicamente não haverá linha total preenchida
            //então isso economiza um pouco o processamento
            if (_playedBlocks.Count < 10)
                return linhas;

            var linha = Dimension.Height - 1;

            //esse laço é invertido (decremental) para que a validação de linhas completadas ocorra debaixo pra cima.
            do
            {
                if (linhas == MAXIMO_LINHAS_POSSIVEIS)
                    break;

                if (ContarBlocosVaziosNaLinha(linha) > 0)
                {
                    linha--;
                }
                else
                {
                    linhas++;
                    RemoverLinha(linha);
                    MoverBlocosParaBaixo(linha);
                }
            } while (linha >= 0);

            return linhas;
        }

        private int ContarBlocosVaziosNaLinha(int linha)
        {
            return Dimension.Width - _playedBlocks.Count(_ => _.Y == linha);
        }

        private void MoverBlocosParaBaixo(int linha)
        {
            for (var i = 0; i < _playedBlocks.Count; i++)
            {
                if (_playedBlocks[i].Y < linha)
                {
                    _playedBlocks[i] = new Block(_playedBlocks[i].X, _playedBlocks[i].Y + 1, _playedBlocks[i].Color);
                }
            }
        }

        private void RemoverLinha(int linha)
        {
            var blocosExcluidos = _playedBlocks.Where(x => x.Y == linha).ToList();

            foreach (var bloco in blocosExcluidos)
            {
                _playedBlocks.Remove(bloco);
            }
        }

        private void Pontuar(int linhas)
        {
            Stats.Gain(linhas);
            OnClear?.Invoke();
        }

        #endregion
    }
}