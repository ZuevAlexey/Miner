namespace Models {
    public class PlaySettings {
        public byte Rows { get; set; }
        public byte Columns { get; set; }
        public int MinesCount { get; set; }
        public bool CanOpenMineFirstTry { get; set; }
    }
}