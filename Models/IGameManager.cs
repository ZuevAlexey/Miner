using Models.Events;

namespace Models {
    /// <summary>
    ///     Объект, управляющий игрой
    /// </summary>
    public interface IGameManager {
        /// <summary>
        ///     Начать игру с заданными настройками
        /// </summary>
        /// <param name="settings">Настройки игры</param>
        void StartGame(GameSettings settings);

        /// <summary>
        ///     Попробовать открыть ячейку
        /// </summary>
        /// <param name="position">Позиция открываемой ячейки</param>
        void TryOpen(Position position);

        /// <summary>
        ///     Событие, возникающее при открытии ячейки
        /// </summary>
        event CellOpenedEventHandler OnCellOpened;

        /// <summary>
        ///     Событие возникающее при окончании игры
        /// </summary>
        event GameFinishedEventHandler OnGameFinished;

        /// <summary>
        ///     Событие возникающее при начале игры
        /// </summary>
        event GameStartedEventHandler OnGameStarted;
    }
}
