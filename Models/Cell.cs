namespace Models {
    public class Cell {
        public Cell(Position position) {
            Position = position;
        }

        public Position Position { get; }
        public bool IsOpened { get; private set; }
        public bool IsMineHere { get; private set; }
        public byte MineAroundCount { get; set; }
        protected internal string DisplayString => IsMineHere ? "M" : MineAroundCount.ToString();

        public virtual bool TryDropMine() {
            if (IsMineHere) {
                return false;
            }

            IsMineHere = true;
            return true;
        }

        public override string ToString() {
            return $"[{Position.Row},{Position.Column}] - {DisplayString}";
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
            return Position.Row == other.Position.Row && Position.Column == other.Position.Column;
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
                return (Position.Row.GetHashCode() * 397) ^ Position.Column.GetHashCode();
            }
        }

        #endregion
    }
}