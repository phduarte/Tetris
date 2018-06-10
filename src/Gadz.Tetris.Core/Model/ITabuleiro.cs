using System;
using System.Collections.Generic;

namespace Gadz.Tetris.Model {
    public interface ITabuleiro {

        #region fields

        ITabuleiroEstado Estado { get; }
        Bloco[,] Matriz { get; }
        Peca PecaAtual { get; }
        Peca ProximaPeca { get; }
        int Nivel { get; }
        int Linhas { get; }
        TimeSpan Duracao { get; }
        IList<Bloco> Blocos { get; }
        bool EstaJogando { get; }
        int RecordeMaximo { get; }
        int Altura { get;  }
        int Largura { get; }
        int Velocidade { get; }
        int Pontos { get; }
        int FrequenciaDeAtualizacao { get; }

        #endregion

        #region movimentos

        void MoverEsquerda();
        void MoverDireita();
        void MoverBaixo();
        void Rotacionar();
        void CorrerEsquerda();
        void CorrerDireita();
        void Cair();

        #endregion

        #region eventos

        event GameActionEventHandler QuandoAtualizar;
        event GameActionEventHandler QuandoTerminar;
        event GameActionEventHandler QuandoLimpar;
        event GameActionEventHandler QuandoMover;
        event GameActionEventHandler QuandoDeslizar;

        #endregion

        #region methods

        void Pausar();
        void Continuar();
        void Terminar();
        void Reiniciar();
        void Iniciar();
        void Carregar(Identidade id);
        void Salvar();

        #endregion
    }
}
