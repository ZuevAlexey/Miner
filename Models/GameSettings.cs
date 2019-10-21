namespace Models {
    /// <summary>
    ///     Настройки игры
    /// </summary>
    public class GameSettings {
        /// <summary>
        ///     Конструктор
        /// </summary>
        /// <param name="rows">Количетсво строк</param>
        /// <param name="columns">Количество столбцов</param>
        /// <param name="minesCount">Количетсов мин</param>
        /// <param name="canOpenMineFirstTry">Можно ли открыть мину с первого раза?</param>
        public GameSettings(byte rows, byte columns, int minesCount, bool canOpenMineFirstTry) {
            Rows = rows;
            Columns = columns;
            MinesCount = minesCount;
            CanOpenMineFirstTry = canOpenMineFirstTry;
        }

        /// <summary>
        ///     Количетсво строк
        /// </summary>
        public byte Rows { get; }

        /// <summary>
        ///     Количество столбцов
        /// </summary>
        public byte Columns { get; }

        /// <summary>
        ///     Количетсов мин
        /// </summary>
        public int MinesCount { get; }

        /// <summary>
        ///     Можно ли открыть мину с первого раза?
        /// </summary>
        public bool CanOpenMineFirstTry { get; }
    }
}
