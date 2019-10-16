using System.Collections.Generic;

namespace Models {
    public class Cell {
        private static readonly Dictionary<CellState, HashSet<CellState>> AllowedСonversions =
            new Dictionary<CellState, HashSet<CellState>> {
                [CellState.Closed] = new HashSet<CellState> {
                    CellState.Opened,
                    CellState.Undefined,
                    CellState.MineHere
                },
                [CellState.MineHere] = new HashSet<CellState> {
                    CellState.Closed,
                    CellState.Undefined
                },
                [CellState.Undefined] = new HashSet<CellState> {
                    CellState.Closed,
                    CellState.MineHere
                }
            };

        public Cell(byte row, byte column) {
            Row = row;
            Column = column;
            MineCount = 0;
        }

        public bool IsMineHere { get; private set; }
        public byte MineCount { get; private set; }

        public byte Row { get; }
        public byte Column { get; }

        public CellState State { get; private set; }

        public byte MineAroundCount { get; set; }
        public string DisplayString => IsMineHere ? "M" : MineAroundCount.ToString();

        public virtual bool TryDropMine() {
            if (IsMineHere) {
                return false;
            }

            IsMineHere = true;
            MineCount = 1;
            return true;
        }

        public override string ToString() {
            return $"[{Row},{Column}] - {DisplayString}";
        }

        public bool TryChangeState(CellState newState, out CellState oldState) {
            oldState = State;
            if (!CanChangeState(newState)) {
                return false;
            }

            State = newState;
            return true;
        }

        /// <summary>
        ///     Is it possible to switch to a new state
        /// </summary>
        /// <returns></returns>
        protected virtual bool CanChangeState(CellState newState) {
            return AllowedСonversions[State].Contains(newState);
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