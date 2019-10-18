namespace Models {
    public class Cell {
        public Cell(byte row, byte column) {
            Row = row;
            Column = column;
        }

        public bool IsMineHere { get; private set; }
        public byte Row { get; }
        public byte Column { get; }

        public bool IsOpened { get; private set; }

        public byte MineAroundCount { get; set; }
        public string DisplayString => IsMineHere ? "M" : MineAroundCount.ToString();

        public virtual bool TryDropMine() {
            if (IsMineHere) {
                return false;
            }

            IsMineHere = true;
            return true;
        }

        public override string ToString() {
            return $"[{Row},{Column}] - {DisplayString}";
        }

        public bool TryOpen() {
            if (!CanOpen()) {
                return false;
            }

            IsOpened = true;
            return true;
        }

        /// <summary>
        ///     Is it possible to open Cell
        /// </summary>
        /// <returns></returns>
        protected virtual bool CanOpen() {
            return !IsOpened;
        }

        #region Equals

        protected bool Equals(Cell other) {
            return Row == other.Row && Column == other.Column;
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) {
                return false;
            }

            if (ReferenceEquals(this, obj)) {
                return true;
            }

            return obj.GetType() == GetType() && Equals((Cell) obj);
        }

        public override int GetHashCode() {
            unchecked {
                return (Row.GetHashCode() * 397) ^ Column.GetHashCode();
            }
        }

        #endregion
    }
}