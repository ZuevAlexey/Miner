using System;

namespace Models.Events {
    public class GameFinishedEventHandlerArgs : EventArgs {
        public GameFinishedEventHandlerArgs(bool isVictory) {
            IsVictory = isVictory;
        }
        public bool IsVictory { get;}
    }
}