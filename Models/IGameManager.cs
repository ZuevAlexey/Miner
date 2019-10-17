using Models.Events;

namespace Models {
    public interface IGameManager {
        void StartGame(PlaySettings settings);
        void TryChangeState(byte row, byte column, CellState newState);
        CellState GetCellState(byte row, byte column);
        event CellStateChangedEventHandler OnCellStateChanged;
        event GameFinishedEventHandler OnGameFinished;
        event GameStartedEventHandler OnGameStarted;
    }
}