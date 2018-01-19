using System;

namespace Gadz.Tetris.Core.DomainModel {
    public struct Identidade {

        string _id;

        public Identidade(string id) {
            _id = id;
        }

        public Identidade(int id) {
            _id = id.ToString();
        }

        public static Identidade New() {
            return new Identidade(Guid.NewGuid().ToString());
        }

        public static implicit operator Identidade(int id) {
            return new Identidade(id);
        }
        
        public static implicit operator int(Identidade id) {

            if (int.TryParse(id.ToString(), out int u))
                return u;

            throw new InvalidCastException();
        }

        public static implicit operator string(Identidade id) {
            return id.ToString();
        }

        public static implicit operator Identidade(string id) {
            return new Identidade(id);
        }

        public override string ToString() {
            return _id;
        }
    }
}