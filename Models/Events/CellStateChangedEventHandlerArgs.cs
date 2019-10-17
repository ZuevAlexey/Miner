using System;

namespace Models.Events {
    public class CellStateChangedEventHandlerArgs : EventArgs {
        public CellStateChangedEventHandlerArgs(byte row, byte column, CellState newState, CellState oldState,
                                                string displayString) {
            Row = row;
            Column = column;
            NewState = newState;
            OldState = oldState;
            DisplayString = displayString;
        }

        public byte Row { get; }
        public byte Column { get; }
        public CellState NewState { get; }
        public CellState OldState { get; }

        public string DisplayString { get; }

        public override string ToString() {
            return $"{Row}; {Column}; {NewState}; {OldState}; {DisplayString}";
        }
    }
}