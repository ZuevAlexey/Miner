using System;
using System.Collections.Generic;

namespace Models.Events {
    public class GameFinishedEventHandlerArgs : EventArgs {
        public GameFinishedEventHandlerArgs(bool isVictory) {
            IsVictory = isVictory;
        }

        public bool IsVictory { get; }
        public List<Position> MinePositions { get; set; }
    }
}