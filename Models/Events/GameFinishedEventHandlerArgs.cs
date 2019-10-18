using System;
using System.Collections.Generic;

namespace Models.Events {
    public class GameFinishedEventHandlerArgs : EventArgs {
        public GameFinishedEventHandlerArgs(bool isVictory, List<Position> minePositions) {
            IsVictory = isVictory;
            MinePositions = minePositions;
        }

        public bool IsVictory { get; }
        public List<Position> MinePositions { get; }
    }
}