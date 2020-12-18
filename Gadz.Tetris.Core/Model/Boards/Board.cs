using Gadz.Tetris.Model.Pieces;
using Gadz.Tetris.Model.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Gadz.Tetris.Model.Boards
{
    /// <summary>
    /// Defines the <see cref="Board" />
    /// </summary>
    public class Board : Entity
    {
        private PieceBuilder _pieceBuilder;

        /// <summary>
        /// Defines the _repository
        /// </summary>
        private IStatsRepository _repository;

        /// <summary>
        /// Gets a value indicating whether _atingiuTopo
        /// </summary>
        internal bool _atingiuTopo => ThereIsCollision(NextPiece);

        /// <summary>
        /// Defines the MAXIMO_LINHAS_POSSIVEIS
        /// </summary>
        private const int MAXIMO_LINHAS_POSSIVEIS = 4;

        /// <summary>
        /// Defines the _playedBlocks
        /// </summary>
        private List<Block> _playedBlocks = new List<Block>();

        //o propósito dessa variável é evitar que ocorra 2 avaliações ao mesmo tempo sem precisar de nenhum lock.
        /// <summary>
        /// Defines the _refreshing
        /// </summary>
        private bool _refreshing;

        /// <summary>
        /// Gets the Dimension
        /// </summary>
        public Size Dimension { get; private set; }

        /// <summary>
        /// Gets the State
        /// </summary>
        public IBoardState State { get; private set; }

        /// <summary>
        /// Gets the Stats
        /// </summary>
        public Stats Stats { get; private set; }

        /// <summary>
        /// Gets a value indicating whether IsPlaying
        /// </summary>
        public bool IsPlaying
        {
            get { return State.IsPlaying; }
        }

        /// <summary>
        /// Gets the Matrix
        /// </summary>
        public Block[,] Matrix { get; }

        /// <summary>
        /// Gets the CurrentPiece
        /// </summary>
        public Piece CurrentPiece { get; private set; }

        /// <summary>
        /// Gets the NextPiece
        /// </summary>
        public Piece NextPiece { get; private set; }

        /// <summary>
        /// Gets the Level
        /// </summary>
        public int Level => Stats.Level;

        /// <summary>
        /// Gets the Lines
        /// </summary>
        public int Lines => Stats.Lines;

        /// <summary>
        /// Gets the Blocks
        /// </summary>
        public IReadOnlyList<Block> Blocks => _playedBlocks;

        /// <summary>
        /// Gets the Record
        /// </summary>
        public int Record { get; private set; }

        /// <summary>
        /// Gets the Duration
        /// </summary>
        public TimeSpan Duration => Stats.Duration;

        /// <summary>
        /// Gets the Height
        /// </summary>
        public int Height => Dimension.Height;

        /// <summary>
        /// Gets the Width
        /// </summary>
        public int Width => Dimension.Width;

        /// <summary>
        /// Gets the Speed
        /// </summary>
        public int Speed => Stats.Speed;

        /// <summary>
        /// Gets the Score
        /// </summary>
        public int Score => Stats.Score;

        /// <summary>
        /// Gets the Frequency
        /// </summary>
        public int Frequency => Stats.Speed <= 1000 ? 1000 - Stats.Speed : 0;

        /// <summary>
        /// 
        /// </summary>
        public int Tetris => Stats.Tetris;

        /// <summary>
        /// 
        /// </summary>
        public float TetrisRate => Stats.TetrisRate;

        /// <summary>
        /// Defines the OnRefresh
        /// </summary>
        public event GameActionEventHandler OnRefresh;

        /// <summary>
        /// Defines the OnFinish
        /// </summary>
        public event GameActionEventHandler OnFinish;

        /// <summary>
        /// Defines the OnClear
        /// </summary>
        public event GameActionEventHandler OnClear;

        /// <summary>
        /// Defines the OnMove
        /// </summary>
        public event GameActionEventHandler OnMove;

        /// <summary>
        /// Defines the OnSlide
        /// </summary>
        public event GameActionEventHandler OnSlide;

        /// <summary>
        /// Defines the OnDrop
        /// </summary>
        public event GameActionEventHandler OnDrop;

        /// <summary>
        /// Defines the OnPause
        /// </summary>
        public event GameActionEventHandler OnPause;

        /// <summary>
        /// Defines the OnContinue
        /// </summary>
        public event GameActionEventHandler OnContinue;

        /// <summary>
        /// Defines the OnContinue
        /// </summary>
        public event GameActionEventHandler OnLose;

        /// <summary>
        /// Initializes a new instance of the <see cref="Board"/> class.
        /// </summary>
        /// <param name="repository">The repository<see cref="IStatsRepository"/></param>
        /// <param name="width">The width<see cref="int"/></param>
        /// <param name="height">The height<see cref="int"/></param>
        public Board(IStatsRepository repository, int width, int height)
        {
            _repository = repository;
            _pieceBuilder = new PieceBuilder().OnBoard(this);
            Dimension = new Size(width, height);
            Stats = new Stats();
            State = new PausedState();
            Matrix = new Block[width, height];
            Record = _repository.MaxScore();
        }

        /// <summary>
        /// The SmashDown
        /// </summary>
        public void SmashDown()
        {
            while (CanMoveDown(CurrentPiece))
            {
                CurrentPiece.MoveDown();
                OnSlide?.Invoke();
            }

            Checkout();
            OnDrop?.Invoke();
        }

        /// <summary>
        /// The SlideRight
        /// </summary>
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

        /// <summary>
        /// The SlideLeft
        /// </summary>
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

        /// <summary>
        /// The Pause
        /// </summary>
        public void Pause()
        {
            OnPause?.Invoke();
            State = new PausedState();
            Stats.Pause();
        }

        /// <summary>
        /// The Continue
        /// </summary>
        public void Continue()
        {
            OnContinue?.Invoke();
            State = new PlayingState();
            Stats.Continue();
            RefreshAsync();
        }

        /// <summary>
        /// The Finish
        /// </summary>
        public void Finish()
        {
            OnRefresh?.Invoke();
            Stats.Finish();
            State = new FinishedState();
            OnFinish?.Invoke();
        }

        /// <summary>
        /// The Finish
        /// </summary>
        public void Lose()
        {
            OnRefresh?.Invoke();
            Stats.Finish();
            State = new FinishedState();
            //OnFinish?.Invoke();
            OnLose?.Invoke();
        }

        /// <summary>
        /// The Start
        /// </summary>
        public void Start()
        {
            CurrentPiece = CreateRandomBlock();
            NextPiece = CreateRandomBlock();
            Stats.Start();
            ResetMatrix();
            State = new PlayingState();
            RefreshAsync();
        }

        /// <summary>
        /// The Restart
        /// </summary>
        public void Restart()
        {
            Finish();
            Start();
        }

        /// <summary>
        /// The Save
        /// </summary>
        public void Save()
        {
            _repository.Save(Stats);
        }

        /// <summary>
        /// The Load
        /// </summary>
        /// <param name="id">The id<see cref="Identity"/></param>
        public void Load(Identity id)
        {
            Stats = _repository.Load(id);
        }

        /// <summary>
        /// The MoveDown
        /// </summary>
        public void MoveDown()
        {
            if (CanMoveDown(CurrentPiece))
            {
                CurrentPiece.MoveDown();
                OnMove?.Invoke();
            }

            Checkout();
        }

        /// <summary>
        /// The MoveRight
        /// </summary>
        public void MoveRight()
        {
            if (CanMoveRight(CurrentPiece))
            {
                CurrentPiece.MoveRight();
                OnMove?.Invoke();
                ResetMatrix();
            }
        }

        /// <summary>
        /// The MoveLeft
        /// </summary>
        public void MoveLeft()
        {
            if (CanMoveLeft(CurrentPiece))
            {
                CurrentPiece.MoveLeft();
                OnMove?.Invoke();
                ResetMatrix();
            }
        }

        /// <summary>
        /// The Rotate
        /// </summary>
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

        /// <summary>
        /// The CanMoveLeft
        /// </summary>
        /// <param name="piece">The piece<see cref="Piece"/></param>
        /// <returns>The <see cref="bool"/></returns>
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

        /// <summary>
        /// The CanMoveRight
        /// </summary>
        /// <param name="piece">The piece<see cref="Piece"/></param>
        /// <returns>The <see cref="bool"/></returns>
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

        /// <summary>
        /// The CanMoveDown
        /// </summary>
        /// <param name="piece">The piece<see cref="Piece"/></param>
        /// <returns>The <see cref="bool"/></returns>
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

        /// <summary>
        /// The Checkout
        /// </summary>
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
                    Lose();
                    return;
                }
                else
                {
                    TrocarPecaAtual();

                    if (_atingiuTopo)
                    {
                        Lose();
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

        /// <summary>
        /// The VerificarSePreencheuLinha
        /// </summary>
        private void VerificarSePreencheuLinha()
        {
            var linhas = LimparLinhas();

            if (linhas > 0)
            {
                Pontuar(linhas);
            }
        }

        /// <summary>
        /// The ResetMatrix
        /// </summary>
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

        /// <summary>
        /// The RefreshAsync
        /// </summary>
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

        /// <summary>
        /// The CreateRandomBlock
        /// </summary>
        /// <returns>The <see cref="Piece"/></returns>
        private Piece CreateRandomBlock()
        {
            var x = new Random(DateTime.Now.Millisecond).Next(0, 7);
            var tipo = (PieceType)x;
            var rotacao = new Random(DateTime.Now.Millisecond).Next(0, 4);
            var posicao = new Point(0, 0);
            var peca = _pieceBuilder.OfType(tipo)
                .OnPosition(posicao)
                .WithRotation(rotacao)
                .Build();

            return peca.Clone();
        }

        /// <summary>
        /// The ThereIsCollision
        /// </summary>
        /// <param name="peca">The peca<see cref="Piece"/></param>
        /// <returns>The <see cref="bool"/></returns>
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

        /// <summary>
        /// The TrocarPecaAtual
        /// </summary>
        private void TrocarPecaAtual()
        {
            GuardarBlocos();
            CurrentPiece = NextPiece.Clone();
            NextPiece = CreateRandomBlock();
            Stats.IncludeBlock();
            OnRefresh?.Invoke();
        }

        /// <summary>
        /// The GuardarBlocos
        /// </summary>
        private void GuardarBlocos()
        {
            foreach (var i in CurrentPiece.Blocks)
            {
                _playedBlocks.Add(i);
            }
        }

        /// <summary>
        /// The LimparLinhas
        /// </summary>
        /// <returns>The <see cref="int"/></returns>
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

        /// <summary>
        /// The ContarBlocosVaziosNaLinha
        /// </summary>
        /// <param name="linha">The linha<see cref="int"/></param>
        /// <returns>The <see cref="int"/></returns>
        private int ContarBlocosVaziosNaLinha(int linha)
        {
            return Dimension.Width - _playedBlocks.Count(_ => _.Y == linha);
        }

        /// <summary>
        /// The MoverBlocosParaBaixo
        /// </summary>
        /// <param name="linha">The linha<see cref="int"/></param>
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

        /// <summary>
        /// The RemoverLinha
        /// </summary>
        /// <param name="linha">The linha<see cref="int"/></param>
        private void RemoverLinha(int linha)
        {
            var blocosExcluidos = _playedBlocks.Where(x => x.Y == linha).ToList();

            foreach (var bloco in blocosExcluidos)
            {
                _playedBlocks.Remove(bloco);
            }
        }

        /// <summary>
        /// The Pontuar
        /// </summary>
        /// <param name="linhas">The linhas<see cref="int"/></param>
        private void Pontuar(int linhas)
        {
            Stats.Gain(linhas);
            OnClear?.Invoke();
        }
    }
}
