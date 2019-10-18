namespace Models {
    public class Position {
        public Position(byte row, byte column) {
            Row = row;
            Column = column;
        }
        public byte Row { get; }
        public byte Column { get; }
    }
}