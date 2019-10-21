namespace Models {
    /// <summary>
    ///     Представляет собой ячейку поля игры
    /// </summary>
    public class Cell {
        /// <summary>
        ///     Конструктор
        /// </summary>
        /// <param name="position">Позиция ячейки</param>
        public Cell(Position position) {
            Position = position;
        }

        /// <summary>
        ///     Позиция ячейки
        /// </summary>
        public Position Position { get; }

        /// <summary>
        ///     Открыта ли ячейка?
        /// </summary>
        public bool IsOpened { get; private set; }

        /// <summary>
        ///     Содержит ли ячейка мину?
        /// </summary>
        public bool IsMineHere { get; private set; }

        /// <summary>
        ///     Количество мин вокруг ячейки
        /// </summary>
        public byte MineAroundCount { get; set; }

        protected internal string DisplayString => IsMineHere ? "M" : MineAroundCount.ToString();

        /// <summary>
        ///     Попробовать сбросить бомбу в ячейку
        /// </summary>
        /// <returns>Успешность результата сбоса бомбы</returns>
        public virtual bool TryDropMine() {
            if(IsMineHere) {
                return false;
            }

            IsMineHere = true;
            return true;
        }

        public override string ToString() {
            return $"[{Position.Row},{Position.Column}] - {DisplayString}";
        }

        /// <summary>
        ///     Попробовать открыть ячейку
        /// </summary>
        /// <returns>Успешность результата открытия ячейки</returns>
        public bool TryOpen() {
            if(!CanOpen()) {
                return false;
            }

            IsOpened = true;
            return true;
        }

        /// <summary>
        ///     Можно ли открыть ячейку?
        /// </summary>
        /// <returns></returns>
        protected virtual bool CanOpen() {
            return !IsOpened;
        }

        #region Equals

        protected bool Equals(Cell other) {
            return Position.Equals(other.Position);
        }

        public override bool Equals(object obj) {
            if(ReferenceEquals(null, obj)) {
                return false;
            }

            if(ReferenceEquals(this, obj)) {
                return true;
            }

            return obj.GetType() == GetType() && Equals((Cell) obj);
        }

        public override int GetHashCode() {
            return Position.GetHashCode();
        }

        #endregion
    }
}
