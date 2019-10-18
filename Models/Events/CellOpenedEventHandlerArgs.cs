using System;

namespace Models.Events {
    public class CellOpenedEventHandlerArgs : EventArgs {
        public CellOpenedEventHandlerArgs(byte row, byte column, string displayString) {
            Row = row;
            Column = column;
            DisplayString = displayString;
        }

        public byte Row { get; }
        public byte Column { get; }

        public string DisplayString { get; }

        public override string ToString() {
            return $"{Row}; {Column}; {DisplayString}";
        }
    }
}