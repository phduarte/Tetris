using Gadz.Tetris.Core.Infrastructure.Data;
using Gadz.Tetris.Core.DomainModel.Pecas;
using Gadz.Tetris.Core.DomainModel.Tabuleiros;
using System;
using System.Collections.Generic;
using Gadz.Tetris.Core.DomainModel;

namespace Gadz.Tetris.Core.Services {

    public class GameController {

        #region fields

        ITabuleiro _tabuleiro;

        #endregion

        #region eventos

        public event Atualizou OnRefresh;
        public event Terminou OnEnd;
        public event Limpou OnClear;
        public event Moveu OnMove;
        public event Correu OnSlide;

        #endregion

        #region properties

        public bool Playing => _tabuleiro.EstaJogando;
        public int BoardWidth => _tabuleiro.Largura;
        public int BoardHeight => _tabuleiro.Altura;
        public int Speed => _tabuleiro.Velocidade;
        public int Lines => _tabuleiro.Linhas;
        public Bloco[,] Matrix => _tabuleiro.Matriz;
        public TimeSpan Time => _tabuleiro.Duracao;
        public int Score => _tabuleiro.Pontos;
        public int Level => _tabuleiro.Nivel;
        public ITabuleiroEstado State => _tabuleiro.Estado;
        public Ponto CurrentPiecePosition => _tabuleiro.PecaAtual.Posicao;
        public ITabuleiro CurrentBoard => _tabuleiro;

        #endregion

        #region constructors

        private GameController(int largura, int altura) {

            if(largura < 10) {
                throw new ArgumentException(nameof(largura));
            }

            if(altura < 10) {
                throw new ArgumentException(nameof(altura));
            }

            _tabuleiro = new Tabuleiro(new EstatisticasRepository(), largura, altura);
            _tabuleiro.QuandoAtualizar += () => { OnRefresh?.Invoke(); };
            _tabuleiro.QuandoTerminar += () => { OnEnd?.Invoke(); };
            _tabuleiro.QuandoLimpar += () => { OnClear?.Invoke(); };
            _tabuleiro.QuandoMover += () => { OnMove?.Invoke(); };
            _tabuleiro.QuandoDeslizar += () => { OnSlide?.Invoke(); };
        }

        #endregion

        public static GameController Create(int largura, int altura) {
            return new GameController(largura, altura);
        }

        public void SmashDown() {
            _tabuleiro.Cair();
        }

        public void RunRight()=> _tabuleiro.CorrerDireita();

        public void RunLeft()=> _tabuleiro.CorrerEsquerda();

        public void Start() => _tabuleiro.Iniciar();

        public void MoveDown() => _tabuleiro.MoverBaixo();

        public void MoveRight()=> _tabuleiro.MoverDireita();

        public void MoveLeft()=> _tabuleiro.MoverEsquerda();

        public void Pause()=> _tabuleiro.Pausar();

        public void Save()=> _tabuleiro.Salvar();

        public void Exit() {
            _tabuleiro.Terminar();
            Save();
        }

        public void Rotate()=> _tabuleiro.Rotacionar();

        public void Continue()=> _tabuleiro.Continuar();

        public IEnumerable<Bloco> GetActualBlocks()=> _tabuleiro.PecaAtual.Blocos;

        public IEnumerable<Bloco> GetNextBlocks()=> _tabuleiro.ProximaPeca.Blocos;

        public int GetMaxScore() {
            return _tabuleiro.RecordeMaximo;
        }
    }
}
