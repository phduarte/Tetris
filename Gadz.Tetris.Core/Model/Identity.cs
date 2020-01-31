using System;

namespace Gadz.Tetris.Model {
    public struct Identity {

        string _id;

        public Identity(string id) {
            _id = id;
        }

        public Identity(int id) {
            _id = id.ToString();
        }

        public static Identity New() {
            return new Identity(Guid.NewGuid().ToString());
        }

        public static implicit operator Identity(int id) {
            return new Identity(id);
        }
        
        public static implicit operator int(Identity id) {

            if (int.TryParse(id.ToString(), out int u))
                return u;

            return -1;
        }

        public static implicit operator string(Identity id) {
            return id.ToString();
        }

        public static implicit operator Identity(string id) {
            return new Identity(id);
        }

        public override string ToString() {
            return _id;
        }
    }
}