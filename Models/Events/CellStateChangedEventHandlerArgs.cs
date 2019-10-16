using System;

namespace Models.Events {
    public class CellStateChangedEventHandlerArgs : EventArgs {
        public CellStateChangedEventHandlerArgs(byte row, byte column, CellState newState, CellState oldState) {
            Row = row;
            Column = column;
            NewState = newState;
            OldState = oldState;
        }

        public byte Row { get; }
        public byte Column { get; }
        public CellState NewState { get; }
        public CellState OldState { get; }
    }
}