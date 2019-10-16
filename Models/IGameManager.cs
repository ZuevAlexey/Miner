using Models.Events;

namespace Models {
    public interface IGameManager {
        void StartGame(PlaySettings settings);
        void ChangeState(byte row, byte column, CellState newState);
        event CellStateChangedEventHandler OnCellStateChanged;
        event GameFinishedEventHandler OnGameFinished;
    }
}