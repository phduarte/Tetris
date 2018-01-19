using Gadz.Tetris.Data;
using Gadz.Tetris.Model.Pecas;
using Gadz.Tetris.Model.Tabuleiros;
using System;
using System.Collections.Generic;

namespace Gadz.Tetris.Services {

    public class GameController {

        #region fields

        ITabuleiro _tabuleiro;
        readonly TabuleiroRepository _repository;

        #endregion

        #region eventos

        public event Atualizou QuandoAtualizar;
        public event Terminou QuandoTerminar;
        public event Limpou QuandoLimpar;
        public event Moveu QuandoMover;
        public event Correu QuandoDeslizar;

        #endregion

        #region properties

        public bool EstaJogando => _tabuleiro.EstaJogando;
        public int AlturaTabuleiro => _tabuleiro.Dimensao.Altura;
        public int LarguraTabuleiro => _tabuleiro.Dimensao.Largura;
        public int Velocidade => _tabuleiro.Estatisticas.Velocidade;
        public int Linhas => _tabuleiro.Linhas;
        public Bloco[,] Matriz => _tabuleiro.Matriz;
        public TimeSpan Duracao => _tabuleiro.Estatisticas.Duracao;
        public int Pontos => _tabuleiro.Estatisticas.Pontos;
        public int Nivel => _tabuleiro.Nivel;

        #endregion

        #region constructors

        public GameController(int largura, int altura) {
            _tabuleiro = new Tabuleiro(largura, altura);
            _repository = new TabuleiroRepository();
            _tabuleiro.QuandoAtualizar += ()=>{ QuandoAtualizar?.Invoke(); };
            _tabuleiro.QuandoTerminar += ()=> { QuandoTerminar?.Invoke(); };
            _tabuleiro.QuandoLimpar += ()=> { QuandoLimpar?.Invoke(); };
            _tabuleiro.QuandoMover += ()=> { QuandoMover?.Invoke(); };
            _tabuleiro.QuandoDeslizar += ()=> { QuandoDeslizar?.Invoke(); };
        }

        #endregion

        public void Cair() {
            _tabuleiro.Cair();
        }

        public void Carregar(string id) {
            //_tabuleiro = _repository.Load(id);
        }

        public void CorrerParaDireita() {
            _tabuleiro.CorrerDireita();
        }

        public void CorrerParaEsquerda() {
            _tabuleiro.CorrerEsquerda();
        }

        public void Iniciar() {
            _tabuleiro.Iniciar();
        }

        public void MoverParaBaixo() {
            _tabuleiro.MoverBaixo();
        }

        public void MoverParaDireita() {
            _tabuleiro.MoverDireita();
        }

        public void MoverParaEsquerda() {
            _tabuleiro.MoverEsquerda();
        }

        public void Pausar() {
            _tabuleiro.Pausar();
        }

        public void Salvar() {
            _repository.Save(_tabuleiro);
        }

        public void Terminar() {
            _tabuleiro.Terminar();
            Salvar();
        }

        public void Rotacionar() {
            _tabuleiro.Rotacionar();
        }

        public void CorrerEsquerda() {
            _tabuleiro.CorrerEsquerda();
        }

        public void MoverEsquerda() {
            _tabuleiro.MoverEsquerda();
        }

        public void CorrerDireita() {
            _tabuleiro.CorrerDireita();
        }

        public void MoverDireita() {
            _tabuleiro.MoverDireita();
        }

        public void Continuar() {
            _tabuleiro.Continuar();
        }

        public void MoverBaixo() {
            _tabuleiro.MoverBaixo();
        }

        public IEnumerable<BlocoModel> PegarBlocosPecaAtual() {
            foreach (var i in _tabuleiro.PecaAtual.Blocos) {
                yield return new BlocoModel(i.X, i.Y, i.Cor.ToString());
            }
        }

        public IEnumerable<BlocoModel> PegarBlocosProximaPeca() {
            foreach(var i in _tabuleiro.ProximaPeca.Blocos) {
                yield return new BlocoModel(i.X, i.Y, i.Cor.ToString());
            }
        }

        public void Reiniciar() {
            _tabuleiro.Terminar();
            _tabuleiro.Iniciar();
        }
    }
}
