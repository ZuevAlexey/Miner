namespace WpfApplication.Views.Cell {
    /// <summary>
    /// Данные ячейки
    /// </summary>
    public class CellData {
        /// <summary>
        /// Состояние ячейки
        /// </summary>
        public CellState State { get; set; }
                
        /// <summary>
        /// Есть ли мина в данной ячейке?
        /// </summary>
        public bool? IsMineHere { get; set; }
        
        /// <summary>
        /// Количество мин вокруг ячейки
        /// </summary>
        public byte? MinesAroundCount { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="state">Состояние ячейки</param>
        /// <param name="isMineHere">Есть ли мина в данной ячейке?</param>
        /// <param name="minesAroundCount">Количество мин вокруг ячейки</param>
        public CellData(CellState state, bool? isMineHere = null, byte? minesAroundCount = null) {
            State = state;
            IsMineHere = isMineHere;
            MinesAroundCount = minesAroundCount;
        }
    }
}
