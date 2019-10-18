using Models.Events;

namespace Models {
    public interface IGameManager {
        void StartGame(PlaySettings settings);
        void TryOpen(Position position);
        event CellOpenedEventHandler OnCellOpened;
        event GameFinishedEventHandler OnGameFinished;
        event GameStartedEventHandler OnGameStarted;
    }
}