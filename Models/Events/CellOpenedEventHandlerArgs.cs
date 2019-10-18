using System;
using System.Runtime;

namespace Models.Events {
    public class CellOpenedEventHandlerArgs : EventArgs {
        public CellOpenedEventHandlerArgs(Position position, bool isMineHere, byte minesAroundCount) {
            Position = position;
            IsMineHere = isMineHere;
            MinesAroundCount = minesAroundCount;
        }

        public Position Position { get; }
        public bool IsMineHere { get; }
        public byte MinesAroundCount { get; }

        public override string ToString() {
            return $"{Position.Row}; {Position.Column}; {IsMineHere}; {MinesAroundCount}";
        }
    }
}