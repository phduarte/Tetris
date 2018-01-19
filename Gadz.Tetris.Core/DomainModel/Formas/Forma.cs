using Gadz.Tetris.Core.DomainModel.Pecas;

namespace Gadz.Tetris.Core.DomainModel.Formas {
    public abstract class Forma {

        public Bloco[] Blocos { get; protected set; }

        #region overrided methods

        public override string ToString() {
            return $"{Blocos[0]},{Blocos[1]},{Blocos[2]},{Blocos[3]}";
        }

        #endregion
    }
}
