namespace Models {
    public enum CellState : byte {
        /// <summary>
        ///     The cell is closed. User hasn’t done anything with her yet.
        /// </summary>
        Closed = 0,

        /// <summary>
        ///     User opened this cell
        /// </summary>
        Opened = 1,

        /// <summary>
        ///     User set the "mine here" flag on the cell
        /// </summary>
        MineHere = 2,

        /// <summary>
        ///     User marked cell as undefined
        /// </summary>
        Undefined = 3
    }
}