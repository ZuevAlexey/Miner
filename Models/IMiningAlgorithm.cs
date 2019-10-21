namespace Models {
    /// <summary>
    ///     Алгоритм для минирования поля игры
    /// </summary>
    public interface IMiningAlgorithm {
        /// <summary>
        ///     Заминировать поле
        /// </summary>
        /// <param name="field">Поле</param>
        /// <param name="minesCount">Количество мин</param>
        void DropMines(Field field, int minesCount);
    }
}
