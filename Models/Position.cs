namespace Models {
    /// <summary>
    ///     Позиция ячейки
    /// </summary>
    public class Position {
        /// <summary>
        ///     Конструктор
        /// </summary>
        /// <param name="row">Строка</param>
        /// <param name="column">Столбец</param>
        public Position(byte row, byte column) {
            Row = row;
            Column = column;
        }

        /// <summary>
        ///     Строка
        /// </summary>
        public byte Row { get; }

        /// <summary>
        ///     Столбец
        /// </summary>
        public byte Column { get; }

        protected bool Equals(Position other) {
            return Row == other.Row && Column == other.Column;
        }

        public override bool Equals(object obj) {
            if(ReferenceEquals(null, obj)) {
                return false;
            }

            if(ReferenceEquals(this, obj)) {
                return true;
            }

            if(obj.GetType() != GetType()) {
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
