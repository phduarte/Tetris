namespace Gadz.Tetris.Model {

    public abstract class Entity {

        public Identity Id { get; private set; }

        protected Entity(Identity id) {
            Id = id;
        }

        protected Entity() {
            Id = Identity.New();
        }

        public override string ToString() {
            return Id.ToString(); 
        }
    }
}
