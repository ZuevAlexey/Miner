namespace WpfApplication.Views.Cell {
    /// <summary>
    /// Состояние ячейки
    /// </summary>
    public enum CellState: byte {
        /// <summary>
        /// Ячейка закрыта
        /// </summary>
        Closed = 0,

        /// <summary>
        /// Ячейка открыта
        /// </summary>
        Opened = 1,

        /// <summary>
        /// Ячейка помечена как "Мина здесь!"
        /// </summary>
        MineHere = 2,

        /// <summary>
        /// Ячейка помечена как неопределенная
        /// </summary>
        Undefined = 3
    }
}
