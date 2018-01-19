using Gadz.Tetris.Core.DomainModel.Formas;
using Gadz.Tetris.Core.DomainModel.Tabuleiros;

namespace Gadz.Tetris.Core.DomainModel.Pecas {
    public interface IPeca : IMovel {
        int Rotacao { get; }
        Forma Forma { get; }
        Ponto Posicao { get; }
        TipoPeca Tipo { get; }
        CorPeca Cor { get; }
        ITabuleiro Tabuleiro { get; }
        IPeca Clonar();
        Bloco[] Blocos { get; }
    }
}