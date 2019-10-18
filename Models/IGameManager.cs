using Models.Events;

namespace Models {
    public interface IGameManager {
        void StartGame(PlaySettings settings);
        void TryOpen(byte row, byte column);
        event CellOpenedEventHandler OnCellStateChanged;
        event GameFinishedEventHandler OnGameFinished;
        event GameStartedEventHandler OnGameStarted;
    }
}