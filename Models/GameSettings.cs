namespace Models {
    public class GameSettings {
        public GameSettings(byte rows, byte columns, int minesCount, bool canOpenMineFirstTry) {
            Rows = rows;
            Columns = columns;
            MinesCount = minesCount;
            CanOpenMineFirstTry = canOpenMineFirstTry;
        }

        public byte Rows { get; set; }
        public byte Columns { get; set; }
        public int MinesCount { get; set; }
        public bool CanOpenMineFirstTry { get; set; }
    }
}