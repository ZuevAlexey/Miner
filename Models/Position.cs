namespace Models {
    public class Position {
        public Position(byte row, byte column) {
            Row = row;
            Column = column;
        }
        public byte Row { get; }
        public byte Column { get; }

        protected bool Equals(Position other) {
            return Row == other.Row && Column == other.Column;
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) {
                return false;
            }

            if (ReferenceEquals(this, obj)) {
                return true;
            }

            if (obj.GetType() != this.GetType()) {
                return false;
            }

            return Equals((Position) obj);
        }

        public override int GetHashCode() {
            unchecked {
                return (Row.GetHashCode() * 397) ^ Column.GetHashCode();
            }
        }
    }
}