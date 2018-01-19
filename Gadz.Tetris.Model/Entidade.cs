namespace Gadz.Tetris.Model {

    public abstract class Entidade {

        public Identidade Id { get; private set; }

        protected Entidade(Identidade id) {
            Id = id;
        }

        protected Entidade() {
            Id = Identidade.New();
        }

        public override string ToString() {
            return Id.ToString(); 
        }
    }
}
