using System;

namespace Models.Events {
    public class CellStateChangedEventHandlerArgs : EventArgs {
        public CellStateChangedEventHandlerArgs(byte row, byte column, CellState newState, CellState oldState, bool isSuccess, string displayString) {
            Row = row;
            Column = column;
            NewState = newState;
            OldState = oldState;
            IsSuccess = isSuccess;
            DisplayString = displayString;
        }

        public byte Row { get; }
        public byte Column { get; }
        public CellState NewState { get; }
        public CellState OldState { get; }
        public bool IsSuccess { get; }

        public string DisplayString { get; }

        public override string ToString() {
            return $"{Row}; {Column}; {NewState}; {OldState}; {IsSuccess}; {DisplayString}";
        }
    }
}